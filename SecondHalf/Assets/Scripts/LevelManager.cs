using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Data;
using Mono.Data.Sqlite;
using SQLite;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public GameObject FirstPlayersGameObject;

	public GameObject SecondPlayersGameObject;

	public GameObject MobileInput;

	public GameObject RulesGameObject;

	public GameObject InputManager;

	public GameObject Camera;

    private const string _levelChoose = "LevelChoose";

    public string LevelChoose
    {
        get
        {
            return _levelChoose;
        }
    }

	private const string _tableName = "LevelInfo";

	public string TableName
	{
		get
		{
			return _tableName;
		}
	}

    // Use this for initialization
    void Awake(){
		SceneManager.sceneLoaded += InstGameObjects;
	}

	void Start () {
		using(var db = new SQLiteConnection(Path.Combine(Application.dataPath, _levelChoose)))
		{
			db.CreateTable<LevelInfo>();
			db.Close();
		}
		DontDestroyOnLoad (this);
		DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainCamera"));
	}

    public void LoadLevelChooseScene()
    {
        SceneManager.LoadScene(_levelChoose);
    }

	public void LoadNextScene() {
		int currentScene = int.Parse (SceneManager.GetActiveScene ().name);
		++currentScene;
		if(!Application.CanStreamedLevelBeLoaded(currentScene))
			return;
		GameObject.Find("Canvas").GetComponent<UIManager>().ShowPause();
		SceneManager.LoadScene (currentScene.ToString (), LoadSceneMode.Single);
	}

	private void InstGameObjects(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene ().name == "MainMenu")
			return;
        if (SceneManager.GetActiveScene().name == _levelChoose)
            return;
		var first = GameObject.FindGameObjectWithTag ("Player1");
		var second = GameObject.FindGameObjectWithTag ("Player2");

        var back = GameObject.Find("Back");
        back.GetComponent<RectTransform>().localScale = Vector3.zero;

        if (first == null || second == null) {
			first = Instantiate (FirstPlayersGameObject, Vector3.zero, Quaternion.identity);
			second = Instantiate (SecondPlayersGameObject, Vector3.zero, Quaternion.identity);
		}
		first.transform.position = GameObject.FindGameObjectWithTag ("Start" + first.tag).transform.position;
		second.transform.position = GameObject.FindGameObjectWithTag ("Start" + second.tag).transform.position;

		var minput = GameObject.FindGameObjectWithTag ("MobileInput");
		if (minput == null)
			minput = Instantiate (MobileInput, Vector3.zero, Quaternion.identity);
		foreach (GameObject controller in GameObject.FindGameObjectsWithTag("Controllers"))
			controller.GetComponent<RectTransform> ().localScale = Vector3.one;

		var imanager = GameObject.FindGameObjectWithTag ("InputManager");
		if (imanager == null)
			imanager = Instantiate (InputManager, Vector3.zero, Quaternion.identity);

		var camera = GameObject.FindGameObjectWithTag ("MainCamera");
		if (camera == null)
			camera = Instantiate (Camera, Camera.transform.position, Camera.transform.rotation);

		GameObject.Find ("LevelDone").GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

	public void LoadScene(string sceneName) {
		GameObject.Find("Canvas").GetComponent<UIManager>().PrepeareGameUI();
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		//var nextScene = SceneManager.GetSceneByName (sceneNum.ToString ());
		//if(nextScene.IsValid())
		//	SceneManager.SetActiveScene (nextScene);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		GameObject.Find("Canvas").GetComponent<UIManager>().HideMedals();
	}

	public void Win(int medals, float levelTime) {	
		int currentScene = int.Parse (SceneManager.GetActiveScene ().name);
		using(var db = new SQLiteConnection(Path.Combine(Application.dataPath, _levelChoose)))
		{
			LevelInfo info = new LevelInfo();
			info.LevelNum = currentScene;
			info.Medal = medals;
			info.Time = levelTime;
			db.InsertOrReplace(info);
			List<LevelInfo> levels = db.Query<LevelInfo>("SELECT * from " + _tableName);
			Debug.Log(levels.Count);
			db.Close();
		}

		++currentScene;
		if(currentScene >= (SceneManager.sceneCount+2))
			return;
		PlayerPrefs.SetInt("LastLevel", currentScene);
	}

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
