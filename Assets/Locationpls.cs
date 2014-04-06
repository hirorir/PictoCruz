using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System;

public class Locationpls : MonoBehaviour {
	LocationInfo li;
	ParseUser user;

	private string username;
	private string password;
	private string email;

	public static double lat;
	public static double lon;
	float xhigh, yhigh, xlow, ylow;

	bool haveaccount = false;
	public static bool loggedin = false;
	delegate void dummy();
	dummy dothis;

	private GUISkin style;

	// Use this for initialization
	void Start () {
		username = "Username";
		password = "Password";
		email = "Email";
		delay ();
		StartCoroutine(wait (2, dothis));
		StartCoroutine(updateLocation());
		xhigh = (float)(37.008579 + 37.002935)/2;
		ylow = (float)(-122.070676 - 122.067273) / 2;
		xlow = (float)(36.974237 + 36.978925) / 2;
		yhigh = (float)(-122.049974 - 122.047807) / 2;
		style = Resources.Load<GUISkin>("Defaultpls");
	}

	void OnGUI() {
		if (loggedin)
			return;
		haveaccount = GUI.Toggle (new Rect(Screen.width * 0.8f, Screen.height * 0.5f, 200, 40), haveaccount, "Existing User?", style.toggle);
			
		/*if (GUI.Button (new Rect (10, 10, 200, 20), "", "usernameField") && usernamecleared){
			username = "";
			usernamecleared = false;
		}*/
		username = GUI.TextField(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.5f, Screen.height * 0.2f), username, 40);
		/*if (GUI.Button (new Rect (10, 10, 200, 20), "", "passwordField") && passwordcleared){
			password = "";
			passwordcleared = false;
		}*/
		password = GUI.PasswordField(new Rect(Screen.width * 0.1f, Screen.height * 0.35f, Screen.width * 0.5f, Screen.height * 0.2f), password, '*');
		/*if (GUI.Button (new Rect (10, 10, 200, 20), "", "emailField") && emailcleared){
			email = "";
			emailcleared = false;
		}*/


		if (!haveaccount)
			email = GUI.TextField(new Rect(Screen.width * 0.1f, Screen.height * 0.6f, Screen.width * 0.5f, Screen.height * 0.2f), email, 40);
		if(haveaccount){
			if(GUI.Button (new Rect (Screen.width * 0.2f, Screen.height * 0.85f, Screen.width * 0.3f, Screen.height * 0.1f), "LOGIN", style.button) || (!Event.current.shift && Event.current.keyCode == KeyCode.Return))
				login ();
		}else
			if(GUI.Button (new Rect (Screen.width * 0.2f, Screen.height * 0.85f, Screen.width * 0.3f, Screen.height * 0.1f), "SIGNUP", style.button) || (!Event.current.shift && Event.current.keyCode == KeyCode.Return))
				signUp ();

	}

	void login(){
		user = new ParseUser(){
			Username = username,
			Password = password			
		};

		ParseUser.LogInAsync(username, password);
		if (ParseUser.CurrentUser.IsAuthenticated) {
			print ("Logged In");
			loggedin = true;
		}
	}

	void signUp(){
		user = new ParseUser(){
			Username = username,
			Password = password,
			Email = email
		};

		int results = 0;
		ParseQuery<ParseUser> query = new ParseQuery<ParseUser>();
		query.WhereEqualTo ("username", username).CountAsync ().ContinueWith (t => {
			results += t.Result;
			query.WhereEqualTo ("email", email).CountAsync ().ContinueWith (s => {
				results += s.Result;
				if(results > 0){
					print ("User or email already exists");
					return;
				}
				if(!Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z")){
					print("Invalid email address");
					return;
				}

				user["Geolocation"] = new ParseGeoPoint( lat, lon );
				user["Messages"] = new List<string> ();
				
				Task signuptask = user.SignUpAsync ();
				print ("Success");
				ParseUser.LogInAsync(username, password);
				if (ParseUser.CurrentUser.IsAuthenticated) {
					print ("Logged In");
					loggedin = true;
				}
			});
		});
	}

	void Update () {

	}

	IEnumerator wait (float sec, dummy dothis){
		yield return new WaitForSeconds (sec);
		dothis ();
	}
	
	void delay(){
		
		dothis = () => {
			ParseUser.CurrentUser.SaveAsync().ContinueWith(t =>
			                                               {
				// Now let's update it with some new data.  In this case, only cheatMode
				// and score will get sent to the cloud.  playerName hasn't changed.
				//ParseUser.CurrentUser["Geolocation"] =  new ParseGeoPoint( 50, 50 );
				ParseUser.CurrentUser.SaveAsync();
			});
			StartCoroutine(wait (2, dothis));
		};

	}

	IEnumerator updateLocation(){
		gameObject.GetComponent<GUIText>().text = "Updating Location...";
		if (!Input.location.isEnabledByUser)
			gameObject.GetComponent<GUIText>().text = "Location not supported on this platform";
			yield return new WaitForSeconds(0);
		Input.location.Start();
		int maxWait = 10;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			gameObject.GetComponent<GUIText>().text = "Waiting " + maxWait + " seconds";
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		if (maxWait < 1) {
			gameObject.GetComponent<GUIText>().text = "Timed out";
			yield return new WaitForSeconds(0);
		}
		if (Input.location.status == LocationServiceStatus.Failed) {
			gameObject.GetComponent<GUIText>().text = "Unable to determine device location";
			yield return new WaitForSeconds(0);
		} else
			li = Input.location.lastData;
		Input.location.Stop();

		lat = 36.989291;
		lon = -122.063417;

		ParseUser.CurrentUser["Geolocation"] = new ParseGeoPoint( lat, lon );

		yield return new WaitForSeconds(30);

		StartCoroutine (updateLocation ());
	}
	
}