using UnityEngine;
using UnityEngine.UI;

public class LocalizedText : MonoBehaviour {

	private string _key;
	
	// Use this for initialization
	void Start () {
		_key = gameObject.GetComponent<Text> ().text;
		SetText();
	}

	void UpdateText() {
		SetText();
	}

	void SetText() {
		gameObject.GetComponent<Text>().text = transform.root.GetComponent<Localization> ().GetString (_key);
	}
}
