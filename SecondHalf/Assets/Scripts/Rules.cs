using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
		bool levelDone = players [0].GetComponent<PlayerComponent> ().OnFinish;
		foreach (var player in players) {
			levelDone &= player.GetComponent<PlayerComponent> ().OnFinish;
		}
		if (levelDone)
			Debug.Log ("Level DONE!");
	}
}
