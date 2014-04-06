using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NTextSize; // Might use this later for more accurate wrapping
using System;
using Parse;
using System.Threading.Tasks;

public class Entity : MonoBehaviour {
	[SerializeField] private int userId;
	private List<string> messages;
	private bool isDisplayed = false;
	float xhigh, yhigh, xlow, ylow;

	// Use this for initialization
	void Start() {
		messages = new List<string>();
		xhigh = (float)(37.003150 + 37.006648)/2f;
		ylow = (float)(-122.073131 - 122.070429) / 2f;
		xlow = (float)(36.975710 + 36.980351) / 2f;
		yhigh = (float)(-122.047774 - 122.047644) / 2f;
	}

	// Update is called once per frame
	void Update() {
		Vector3 scaling = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, 0)) - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		gameObject.transform.position = new Vector3 ((float)(xhigh - Locationpls.lat) * (scaling.x) / (xhigh - xlow) - scaling.x / 2, (float)(yhigh - Locationpls.lon) * (scaling.y) / (yhigh - ylow) - scaling.y / 2, 0);
	}

	void OnGUI() {
		// Scrolling stuff here
	}

	public void OnMouseDown() {
		if (Input.GetMouseButtonDown(0)) {
			if (!isDisplayed) {
				//gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Images/kappa")[0];
				GameObject scrollBox = GameObject.Instantiate(Resources.Load("ScrollBox")) as GameObject;
				scrollBox.transform.parent = gameObject.transform;
				scrollBox.transform.localPosition = new Vector2(4f, 3f);
				isDisplayed = true;
				displayEntityMsgs();
			} else {
				gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Images/person_icon")[0];
				isDisplayed = false;
				Destroy(GameObject.Find("ScrollBox"));
				/*foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) {
					Destroy(bubble);
				}*/
			}
		}
	}

	public void addMessage(string message) {
		sendMessageToServer (message);
		messages.Add(message);
		if (isDisplayed) {
			postMessage(message);
		}
	}

	public void displayEntityMsgs() {
		for (int i = 0; i < messages.Count; i++) {
			postMessage(messages[i]);
		}
	}

	public bool idMatch(int otherId) {
		return (userId == otherId);
	}

	public void sendMessageToServer(string message){
		message += (" " + DateTime.Now.ToString ());
		ParseUser.CurrentUser.AddToList ("Messages", message);
		Task saveTask = ParseUser.CurrentUser.SaveAsync ();
		foreach(string s in ParseUser.CurrentUser.Get<IList<string> >("Messages")){
			print (s);
		}
	}

	public void postMessage(string message) {
		GameObject newMsg = GameObject.Instantiate(Resources.Load("ChatBubble")) as GameObject;
		newMsg.transform.parent = GameObject.FindGameObjectWithTag("ScrollBox").transform;
		newMsg.transform.localPosition = new Vector2(0, -2f);
		message = splitTextMesh(message, 17);
		newMsg.transform.FindChild("Message").GetComponent<TextMesh>().text = message;
		Vector2 transVec = new Vector2(0, newMsg.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

		foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) {
			bubble.transform.Translate(transVec);
		}
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
		for (int x = 0; x < words.Length; x++) {
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
		return result.Substring(1, result.Length - 1);
	}
}