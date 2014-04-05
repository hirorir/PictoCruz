using UnityEngine;
using System.Collections;

public class Locationpls : MonoBehaviour {
	float lat, lon; 
	// Use this for initialization
	void Start () {

	}

	void Update () {
		Input.location.Start ();
		
		LocationInfo li = Input.location.lastData;
			
		gameObject.GetComponent<GUIText> ().text = "LAT: " + li.latitude.ToString () + " LON: " + li.longitude.ToString ();
	}
	
	
	
}