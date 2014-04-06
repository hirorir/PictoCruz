using UnityEngine;
using System.Collections;

public class ChatBubble : MonoBehaviour {
	public string message;
	public float timer = 120.0f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		//timer -= Time.deltaTime;
		if (timer <= 0) {
			GameObject.Find("Chatroom").GetComponent<Chatroom>().msgDead();
			Destroy(gameObject);
		}
		if (transform.localPosition.y > 4 || transform.localPosition.y < -3) {
			renderer.enabled = false;
			transform.FindChild("Message").renderer.enabled = false;
		} else {
			renderer.enabled = true;
			transform.FindChild("Message").renderer.enabled = true;
		}
	}
}
