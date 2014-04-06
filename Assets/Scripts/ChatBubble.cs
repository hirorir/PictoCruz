using UnityEngine;
using System.Collections;

public class ChatBubble : MonoBehaviour {
	public string message;
	public GUIStyle style = new GUIStyle();

	// Use this for initialization
	void Start () {
		GetComponent<GUIText>().text = message;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGUI() {
		GUI.Label(new Rect(Screen.width * 0.35f, Screen.height * 0.65f, 320, 200), message, style);
	}
}
