using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour {
	[SerializeField] private int userId;
	private List<string> messages;
	private bool isDisplayed = false;

	// Use this for initialization
	void Start() {
		messages = new List<string>();
	}

	// Update is called once per frame
	void Update() {

	}

	void OnGUI() {
		// Scrolling stuff here
	}

	public void OnMouseOver() {
		if (Input.GetMouseButtonDown(0)) {
			if (!isDisplayed) {
				gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Images/kappa")[0];
				isDisplayed = true;
				displayEntityMsgs();
			} else {
				gameObject.GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("Images/person_icon")[0];
				isDisplayed = false;
				foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) {
					Destroy(bubble);
				}
			}
		}
	}

	public void addMessage(string message) {
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

	public void postMessage(string message) {
		GameObject newMsg = GameObject.Instantiate(Resources.Load("ChatBubble")) as GameObject;
		newMsg.transform.parent = gameObject.transform;
		newMsg.transform.localPosition = new Vector2(0, -5.5f);
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