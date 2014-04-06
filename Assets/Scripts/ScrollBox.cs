using UnityEngine;
using System.Collections;

public class ScrollBox : MonoBehaviour {
	private Vector2 mousePos;
	private Vector2 prevPos;
	private bool scrollActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		mousePos = Input.mousePosition;
		if (Input.GetMouseButton(0) && scrollActive) {
			int dir = 0;
			if (mousePos.y - prevPos.y > 0) {
				dir = -1;
			} else if (mousePos.y - prevPos.y < 0) {
				dir = 1;
			}
			foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) {
				bubble.transform.Translate(new Vector2(0f, dir * 0.035f));
			}
		}
		prevPos = mousePos;
	}

	void OnMouseEnter() {
		scrollActive = true;
	}

	void OnMouseExit() {
		scrollActive = false;
	}

	void OnMouseDrag() {
		/*if (scrollActive) {
			mousePos = Input.mousePosition;
			Vector2 newPos = Camera.main.WorldToScreenPoint(this.transform.position);
			Vector2 direction = newPos - mousePos;
			int dir;
			if (direction.y >= 0)
				dir = 1;
			else
				dir = -1;
			foreach (GameObject bubble in GameObject.FindGameObjectsWithTag("ChatBubble")) {
				bubble.transform.Translate(new Vector2(0f, dir * 0.02f));
			}
		}*/
	}
}
