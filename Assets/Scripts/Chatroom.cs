﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NTextSize; // Might use this later for more accurate wrapping
using System;
using Parse;
using System.Threading.Tasks;

public class Chatroom : MonoBehaviour {
	private string fieldMsg = "";
	private int msgCount = 0;
	private bool posting = false;
	public GUIStyle stylin;
	public Entity entity;
	public string userId;
	List<string> parser = new List<string> ();
	bool spawn = false;

	// Use this for initialization
	void Start () {
		StartCoroutine(drawEverybodyElse ());
		stylin = Resources.Load<GUISkin>("Defaultpls").button;
	}
	
	// Update is called once per frame
	void Update () {
		if (spawn) {
			foreach(string parsed in parser){
				GameObject temp = GameObject.Instantiate(Resources.Load("Entity"), new Vector3(0f, 0f, 0f), new Quaternion(0, 0, 0, 0)) as GameObject;
				temp.GetComponent<Entity> ().setUID (parsed);
			}
			spawn = false;
		}
		Transform cam = GameObject.Find("Main Camera").transform;
		if (Input.GetKey("down")) {
			if (cam.transform.position.y > 0) {
				cam.Translate(new Vector2(0, -0.1f));
			}
		} else if (Input.GetKey("up")) {
			if (cam.transform.position.y < 2.55f * (msgCount - 1)) {
				cam.Translate(new Vector2(0, 0.1f));
			}
		} else if (Input.GetKey("left")) {
			cam.Translate(new Vector2(-0.1f, 0));
		} else if (Input.GetKey("right")) {
			cam.Translate(new Vector2(0.1f, 0));
		}
	}

	private void OnGUI() {
		if (!Locationpls.loggedin) {
			return;
		}
		if (posting) {
			GUI.SetNextControlName("dummy");
			GUI.Label(new Rect(100, 100, -1, -1), "");

			GUI.SetNextControlName("MyTextField");
			fieldMsg = GUI.TextArea(new Rect(0f, Screen.height * 0.8f, Screen.width * 0.8f, Screen.height * 0.2f), fieldMsg, 100);
			if (fieldMsg != "" && (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.8f, Screen.width * 0.2f, Screen.height * 0.2f), "Send", stylin) || (!Event.current.shift && Event.current.keyCode == KeyCode.Return))) {
				GUI.FocusControl("dummy");
				if (fieldMsg != "\n") {
					//postMessage(fieldMsg);
					entity.addMessage(fieldMsg);
				}
				fieldMsg = "";
				posting = false;
			}
		} else {
			if (GUI.Button(new Rect(Screen.width * 0.8f, Screen.height * 0.8f, Screen.width * 0.2f, Screen.height * 0.2f), "Post!", stylin) || (!Event.current.shift && Event.current.keyCode == KeyCode.Return)) {
				posting = true;
			}
		}
	}

	public IEnumerator drawEverybodyElse(){
		var query = ParseUser.Query.WhereNotEqualTo ("objectId", userId);
		query.FindAsync ().ContinueWith (t => {
			IEnumerable<ParseUser> list = t.Result;
			foreach(ParseUser user in list){
				parser.Add (user.ObjectId);
			}
			spawn = true;
		});
		yield return new WaitForSeconds (2);
	}

	public void sendMessageToServer(string message){
		message += (" " + DateTime.Now.ToString ());
		ParseUser.CurrentUser.AddToList ("Messages", message);
		Task saveTask = ParseUser.CurrentUser.SaveAsync ();
		foreach(string s in ParseUser.CurrentUser.Get<IList<string> >("Messages")){
			//print (s);
		}
	}

	public void postMessage(string message) {

		//sendMessageToServer (message);

		msgCount++;
		GameObject newMsg = GameObject.Instantiate(Resources.Load("ChatBubble"), new Vector3(0f, 0f, 0f), new Quaternion(0, 0, 0, 0)) as GameObject;
		message = splitTextMesh(message, 17);
		newMsg.transform.FindChild("Message").GetComponent<TextMesh>().text = message;
		newMsg.transform.Translate (new Vector3 (2f, -1f, 0f));
		Vector2 transVec = new Vector2(0, newMsg.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

		foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) { // Placeholder name
			bubble.transform.Translate(transVec);
		}
	}

	public void drawMessage() {

	}

	public void msgDead() {
		msgCount--;
	}

	private string splitTextMesh(string input, int lineLength) {
 
	 // Split string by char " "    
	string[] words = input.Split(" "[0]);
 
	// Prepare result
	string result = "";
 
	 // Temp line string
	string line = "";

	string s;
 
	// for each all words     
	for (int x = 0; x < words.Length; x++ ) {
		// Append current word into line
		s = words[x];
		if (s.Length > lineLength) {
			for (int i = lineLength; i < s.Length; i += lineLength) {
				s = s.Insert(i, "\n");
			}
		}

		string temp = line + " " + s;

		// If line length is bigger than lineLength
		if (temp.Length > lineLength) {

			// Append current line into result
			result += line + "\n";
			// Remain word append into new line
			line = s;
		}
			// Append current word into current line
		else {
			line = temp;
		}
	}
 
   // Append last line into result   
   result += line;
 
   // Remove first " " char
   return result.Substring(1,result.Length-1);
	}
}
