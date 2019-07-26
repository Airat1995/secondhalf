using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string key = gameObject.GetComponent<Text> ().text;
		gameObject.GetComponent<Text>().text = GameObject.Find ("LocaleGO").GetComponent<Localization> ().GetString (key);
	}
}