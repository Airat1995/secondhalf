using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.IO;


[Serializable]
public class Local
{
	[Serializable]
	public struct LocalString
	{
		public string Key { get; set; }
		public string Value { get; set; }
	}
	
	public LocalString[] Locales { get; set; }
}

public class Localization : MonoBehaviour {

	private string _locPref = "Localization";

	private string _fileLocation;

	private Local _currentLocalization;
	
	// Use this for initialization
	void Awake () {

		string defLoca = PlayerPrefs.GetString (_locPref, null);
		if (defLoca == null) {
			PlayerPrefs.SetString (_locPref, Application.systemLanguage.ToString ());
			PlayerPrefs.Save ();
		}
		defLoca = PlayerPrefs.GetString (_locPref, "English");
		string locFolder = Path.Combine (Application.streamingAssetsPath, "Localization");
		_fileLocation = Path.Combine (locFolder, defLoca);
		using (var reader = new StreamReader (_fileLocation + ".txt")) {
			_currentLocalization = JsonUtility.FromJson<Local>(reader.ReadToEnd());
			Debug.Log (reader.ReadToEnd());
		}
		Debug.Log (_currentLocalization.Locales[0].Key);
	}

	public string GetString(string key) {
		key = key.Trim(new char[]{'{','}',' '});
		for (int i = 0; i < _currentLocalization.Locales.Length; ++i) {
			if (_currentLocalization.Locales[i].Key.Equals (key))
				return _currentLocalization.Locales[i].Value;
		}
		return "Not found!";
	}
}