using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class Locationpls : MonoBehaviour {
	LocationInfo li;
	public bool anonymous = false;
	ParseUser user;

	// Use this for initialization
	void Start () {
		if (anonymous) {

		}
		else{

		}
		StartCoroutine(updateLocation());
	}

	void Update () {

	}

	IEnumerator updateLocation(){
		gameObject.GetComponent<GUIText>().text = "Updating Location...";
		if (!Input.location.isEnabledByUser)
			gameObject.GetComponent<GUIText>().text = "Location not supported on this platform";
			yield return new WaitForSeconds(0);
		Input.location.Start();
		int maxWait = 10;
		while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0) {
			gameObject.GetComponent<GUIText>().text = "Waiting " + maxWait + " seconds";
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		if (maxWait < 1) {
			gameObject.GetComponent<GUIText>().text = "Timed out";
			yield return new WaitForSeconds(0);
		}
		if (Input.location.status == LocationServiceStatus.Failed) {
			gameObject.GetComponent<GUIText>().text = "Unable to determine device location";
			yield return new WaitForSeconds(0);
		} else
			li = Input.location.lastData;
		Input.location.Stop();

		gameObject.GetComponent<GUIText>().text = "LAT: " + li.latitude + " LON: " + li.longitude + " ALT: " + li.altitude;

		yield return new WaitForSeconds(0);
	}

	/*float getDistanceFromLatLonInKm(float lat1,float lon1,float lat2,float lon2) {
		float R = 6371; // Radius of the earth in km
		float dLat = deg2rad(lat2-lat1);  // deg2rad below
		float dLon = deg2rad(lon2-lon1); 
		float a = 
			Mathf.Sin(dLat/2) * Mathf.Sin(dLat/2) +
				Mathf.Cos(deg2rad(lat1)) * Mathf.Cos(deg2rad(lat2)) * 
				Mathf.Sin(dLon/2) * Mathf.Sin(dLon/2)
				; 
		float c = 2 * Mathf.Atan2(Mathf.Sqrt(a), Mathf.Sqrt(1-a)); 
		float d = R * c; // Distance in km
		return d;
	}
	
	float deg2rad(float deg) {
		return ((float) deg * (Mathf.PI / 180));
	}*/
	
}