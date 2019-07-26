using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelManager : MonoBehaviour {

	public GameObject FirstPlayersGameObject;

	public GameObject SecondPlayersGameObject;

	public GameObject MobileInput;

	public GameObject RulesGameObject;

	public GameObject InputManager;

	public GameObject GameCam;

	public GameObject GameCanvas;

    private string LevelChoose { get{ return "LevelChoose";}}

	private const string _dbName = "LevelChoose.json";

	public const int LevelsCount = 13;

    public string LevelChooseDB
    {
        get
        {
			return  Path.Combine(Application.persistentDataPath, _dbName);
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
		_gCanvas = Instantiate(GameCanvas, Vector3.zero, Quaternion.identity);
		_gCanvas.name = "Canvas";
		DontDestroyOnLoad(gameObject);
		if(!File.Exists(LevelChooseDB))
				File.Create(LevelChooseDB);
	}


	private GameObject _gCanvas;
    
	public void LoadLevelChooseScene()
    {
        SceneManager.LoadScene(LevelChoose);
    }

	public void LoadNextScene() {
		int currentScene = int.Parse (SceneManager.GetActiveScene ().name);
		++currentScene;
		if(!Application.CanStreamedLevelBeLoaded(currentScene))
			return;
		_gCanvas.GetComponent<UIManager>().ShowPause();
		SceneManager.LoadScene (currentScene.ToString (), LoadSceneMode.Single);
	}

	private void InstGameObjects(Scene scene, LoadSceneMode mode) {
		if (SceneManager.GetActiveScene ().name == "MainMenu")
			return;

		var camera = GameObject.FindGameObjectWithTag ("MainCamera");
		if (camera == null)
			camera = Instantiate (GameCam, Vector3.zero, Quaternion.identity);

        if (SceneManager.GetActiveScene().name == LevelChoose)
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

		var minput = GameObject.Find("MobileInput");
		if (minput == null)
			minput = Instantiate (MobileInput, Vector3.zero, Quaternion.identity);
		foreach (GameObject controller in GameObject.FindGameObjectsWithTag("Controllers"))
			controller.GetComponent<RectTransform> ().localScale = Vector3.one;

		var imanager = GameObject.FindGameObjectWithTag ("InputManager");
		if (imanager == null)
			imanager = Instantiate (InputManager, Vector3.zero, Quaternion.identity);

		GameObject.Find ("LevelDone").GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

	public void LoadScene(string sceneName) {
		_gCanvas.GetComponent<UIManager>().PrepeareGameUI();
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

	public void ReloadLevel() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}

	public void Win(int medals, float levelTime) {
		int currentScene = int.Parse (SceneManager.GetActiveScene ().name);
		

		var levelsInfo = GetLevelsInfo();
		
		foreach (var level in levelsInfo)
			if(level.LevelNum == currentScene)
				return;

		LevelInfo info = new LevelInfo();
		info.Medal = medals;
		info.Time = levelTime;
		info.LevelNum = currentScene;

		levelsInfo.Add(info);
		SaveLevelsInfo(levelsInfo);

		++currentScene;
		if(currentScene <= LevelsCount)
			PlayerPrefs.SetInt("LastLevel", currentScene);
	}
    
	public List<LevelInfo> GetLevelsInfo()
	{
		var levelsList = new List<LevelInfo>();
		if(!File.Exists(LevelChooseDB))
			File.Create(LevelChooseDB);
		var format = new BinaryFormatter();
		FileStream stream = File.Open(LevelChooseDB, FileMode.Open);
		if(stream.Length == 0)
			return levelsList;
		levelsList.AddRange(format.Deserialize(stream) as List<LevelInfo>);
		stream.Close();
		return levelsList;

	}

	protected void SaveLevelsInfo(List<LevelInfo> levelsInfo)
	{
		if(File.Exists(LevelChooseDB))
			File.Delete(LevelChooseDB);
		BinaryFormatter format = new BinaryFormatter();
		FileStream stream = new FileStream(LevelChooseDB, FileMode.CreateNew);
		format.Serialize(stream, levelsInfo);
		stream.Close();
	}

	public void LoadMainScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
