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
		public string Key;
		public string Value;
	}
	
	public LocalString[] Locales;
}

public class Localization : MonoBehaviour {

	private string _locPref = "Localization";

	private string _fileLocation;

	private Local _currentLocalization;
	
	// Use this for initialization
	void Awake () {

		string defLoca = PlayerPrefs.GetString (_locPref, null);
		if (string.IsNullOrEmpty(defLoca)) {
			PlayerPrefs.SetString (_locPref, Application.systemLanguage.ToString ());
			PlayerPrefs.Save ();
		}
		defLoca = PlayerPrefs.GetString (_locPref);
		string locFolder = Path.Combine (Application.streamingAssetsPath, "Localization");
		_fileLocation = Path.Combine (locFolder, defLoca);
#if UNITY_ANDROID
		WWW wwf = new WWW (_fileLocation);
		while (!wwf.isDone){}
		_currentLocalization = JsonUtility.FromJson<Local>(wwf.text);
#else
		using (var reader = new StreamReader (_fileLocation)) {
			_currentLocalization = JsonUtility.FromJson<Local>(reader.ReadToEnd().Trim());
		}
#endif
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
