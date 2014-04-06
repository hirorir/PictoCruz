﻿using UnityEngine;
using System.Collections;
using NTextSize; // Might use this later for more accurate wrapping

public class Chatroom : MonoBehaviour {
	private string fieldMsg = "";
	private int msgCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Transform cam = GameObject.Find("Main Camera").transform;
		if (Input.GetKey("down")) {
			if (cam.transform.position.y > 0) {
				cam.Translate(new Vector2(0, -0.1f));
			}
		} else if (Input.GetKey("up")) {
			if (cam.transform.position.y < 2.55f * (msgCount - 1)) {
				cam.Translate(new Vector2(0, 0.1f));
			}
		}
	}

	private void OnGUI() {
		GUI.SetNextControlName("dummy");
		GUI.Label(new Rect(100, 100, -1, -1), "");

		GUI.SetNextControlName("MyTextField");
		fieldMsg = GUI.TextArea(new Rect(Screen.width * 0.35f, Screen.height * 0.9f, 350, 40), fieldMsg, 100);
		if (fieldMsg != "" && (GUI.Button(new Rect(Screen.width * 0.35f + 355, Screen.height * 0.9f, 45, 40), "Send") || (!Event.current.shift && Event.current.keyCode == KeyCode.Return))) {
			GUI.FocusControl("dummy");
			if (fieldMsg != "\n")
				postMessage(fieldMsg);
			fieldMsg = "";
		}
	}

	public void postMessage(string message) {
		msgCount++;
		GameObject newMsg = GameObject.Instantiate(Resources.Load("ChatBubble"), new Vector2(0, -5.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
		message = splitTextMesh(message, 17);
		newMsg.transform.FindChild("Message").GetComponent<TextMesh>().text = message;
		Vector2 transVec = new Vector2(0, newMsg.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

		foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) { // Placeholder name
			bubble.transform.Translate(transVec);
		}
	}

	public void drawMessage() {

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
