using UnityEngine;
using System.Collections;

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
		fieldMsg = GUI.TextArea(new Rect(Screen.width * 0.35f, Screen.height * 0.9f, 350, 40), fieldMsg, 100);
		if (fieldMsg != "" && (GUI.Button(new Rect(Screen.width * 0.35f + 355, Screen.height * 0.9f, 45, 40), "Send") || Input.GetKeyDown("enter"))) {
			postMessage(fieldMsg);
			fieldMsg = "";
		}
	}

	public void postMessage(string message) {
		msgCount++;
		GameObject newMsg = GameObject.Instantiate(Resources.Load("ChatBubble"), new Vector2(0, -5.5f), new Quaternion(0, 0, 0, 0)) as GameObject;
		newMsg.transform.FindChild("Message").GetComponent<TextMesh>().text = message;
		Vector2 transVec = new Vector2(0, newMsg.GetComponent<SpriteRenderer>().sprite.bounds.size.y);

		foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) { // Placeholder name
			bubble.transform.Translate(transVec);
		}
	}

	public void drawMessage() {

	}
}
