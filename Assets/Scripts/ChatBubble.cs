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
		timer -= Time.deltaTime;
		if (timer <= 0) {
			GameObject.Find("Chatroom").GetComponent<Chatroom>().msgDead();
			Destroy(gameObject);
		}
	}
}
