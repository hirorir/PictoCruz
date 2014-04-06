using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

public class Locationpls : MonoBehaviour {
	LocationInfo li;
	ParseUser user;
	private string username;
	private string password;
	private string email;
	bool usernamecleared = true;
	bool passwordcleared = true;
	bool emailcleared = true;
	bool loggedin = false;

	// Use this for initialization
	void Start () {
		username = "Username";
		password = "Password";
		email = "Email";
		StartCoroutine(updateLocation());
	}

	void OnGUI() {
		/*if (GUI.Button (new Rect (10, 10, 200, 20), "", "usernameField") && usernamecleared){
			username = "";
			usernamecleared = false;
		}*/
		username = GUI.TextField(new Rect(10, 10, 200, 20), username, 40);
		/*if (GUI.Button (new Rect (10, 10, 200, 20), "", "passwordField") && passwordcleared){
			password = "";
			passwordcleared = false;
		}*/
		password = GUI.PasswordField(new Rect(10, 40, 200, 20), password, '*');
		/*if (GUI.Button (new Rect (10, 10, 200, 20), "", "emailField") && emailcleared){
			email = "";
			emailcleared = false;
		}*/
		email = GUI.TextField(new Rect(10, 70, 200, 20), email, 40);

		if (GUI.Button (new Rect (10, 100, 200, 20), "SIGNUP") && !loggedin){
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

					Task signuptask = user.SignUpAsync ();
					print ("Success");
					//loggedin = true;
					try{
						ParseUser.LogInAsync(username, password);
						print ("Logged In");
					}catch(System.Exception e){
						print ("Login failed");
					}
				});
			});
		}
	}

	void Update () {

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

		gameObject.GetComponent<GUIText>().text = "LAT: " + li.latitude + " LON: " + li.longitude + " ALT: " + li.altitude;

		yield return new WaitForSeconds(0);
	}
	
}