using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

	private const string _bronzeColor = "#FB7200";

	public string BronzeColor { get { return _bronzeColor; } }

	private const string _silverColor = "#C0C0C0";

	public string SilverColor { get { return _silverColor; } }

	private const string _goldColor = "#FFD700";

	public string GoldColor { get { return _goldColor; } }

    private GameObject _settingsPanel;

    private const string _animationName = "ShowPanel";

	private const string _levelDoneCaption = "<color=#FF5D4A>LEVEL</color> <color=#3088FF>DONE</color>";

	private const string _gameOverCaption = "<color=#FF5D4A>GAME</color> <color=#3088FF>OVER</color>";

	private const string _inversedX = "InversedX";

	public string InversedXName{get {return _inversedX;}}

	private const string _inversedY = "InversedY";

	public string InversedYName{get {return _inversedY;}}

	public const string _playMusic = "PlayMusic";

	private GameObject _bronzeMedal;
	private GameObject _silverMedal;
	private GameObject _goldMedal;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		DontDestroyOnLoad (GameObject.Find ("EventSystem"));
		_settingsPanel = GameObject.Find("Settings");

		int invX = PlayerPrefs.GetInt(_inversedX, 1);
		int invY = PlayerPrefs.GetInt(_inversedY, 1);
		int vol =  PlayerPrefs.GetInt(_playMusic, 1);

		GameObject.Find("InvertX").GetComponent<Toggle>().isOn = invX == -1;
		GameObject.Find("InvertY").GetComponent<Toggle>().isOn = invY == -1;
		GameObject.Find("PlayMusic").GetComponent<Toggle>().isOn = vol == 1;


		_bronzeMedal = GameObject.Find("Bronze");
		_silverMedal = GameObject.Find("Silver");
		_goldMedal = GameObject.Find("Gold");
	}

    public void StartGame()
    {
        int level = 1;// PlayerPrefs.GetInt("LastLevel", 1);
        HideMedals();
        GameObject.FindGameObjectWithTag("LevelManger").GetComponent<LevelManager>().LoadScene(level.ToString());
		PrepeareGameUI();
	}

	public void PrepeareGameUI()
	{
		foreach (Transform child in transform)
        {
            child.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        GameObject.FindGameObjectWithTag("Pause").GetComponent<RectTransform>().localScale = Vector3.one;
        GameObject.Find("Timer").GetComponent<RectTransform>().localScale = Vector3.one;
	}

    public void HideMedals()
    {
		_bronzeMedal.GetComponent<RectTransform>().localScale = Vector3.zero;
		_silverMedal.GetComponent<RectTransform>().localScale = Vector3.zero;
		_goldMedal.GetComponent<RectTransform>().localScale = Vector3.zero;
		ShowPause();
	}

	public void ShowPause()
	{
		GameObject.Find("Pause").GetComponent<RectTransform>().localScale = Vector3.one;
	}

    public void ShowPanel()
    {
		if (GameObject.Find ("Goal") != null)
			GameObject.Find ("Goal").GetComponent<Goal> ().Pause ();
        if(_settingsPanel.GetComponent<RectTransform>().localScale == Vector3.one)
            return;
        _settingsPanel.GetComponent<Animation>()[_animationName].speed = 1;
        _settingsPanel.GetComponent<Animation>().Play(_animationName);
    }

    public void SaveSettings()
    {
        _settingsPanel.GetComponent<Animation>()[_animationName].speed = -1;
        _settingsPanel.GetComponent<Animation>()[_animationName].time = _settingsPanel.GetComponent<Animation>()[_animationName].length;
        _settingsPanel.GetComponent<Animation>().Play(_animationName);

        var isInvertedX = GameObject.Find("InvertX").GetComponent<Toggle>().isOn;
        var isInvertedY = GameObject.Find("InvertY").GetComponent<Toggle>().isOn;
        var isMusicPlay = GameObject.Find("PlayMusic").GetComponent<Toggle>().isOn;

        PlayerPrefs.SetInt(_inversedX, isInvertedX ? -1 : 1);
        PlayerPrefs.SetInt(_inversedY, isInvertedY ? -1 : 1);
        PlayerPrefs.SetInt(_playMusic, isMusicPlay ? 1 : 0);
        PlayerPrefs.Save();

		if(GameObject.FindGameObjectWithTag("InputManager")!=null)
			GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().UpdateInver();

        GameObject.Find("AudioManager").GetComponent<AudioManager>().UpdateSoundVolume(isMusicPlay ? 1 : 0);
		if (GameObject.Find ("Goal") != null)
			GameObject.Find ("Goal").GetComponent<Goal> ().UnPause ();
    }

	public void Win(int medals, float levelTime)
	{
        GameObject.Find("Next").GetComponent<RectTransform>().localScale = Vector3.one;
		GameObject.Find("LevelDone").GetComponent<Animation>().Play();
        GameObject.Find("DoneText").GetComponent<Text>().text = _levelDoneCaption;
		GameObject.FindGameObjectWithTag("LevelManger").GetComponent<LevelManager>().Win(medals, levelTime);
		StartCoroutine(StartAnimationWithDelay(_bronzeMedal));
		if(medals >= 2)
			StartCoroutine(StartAnimationWithDelay(_silverMedal));
		if(medals == 3)
			StartCoroutine(StartAnimationWithDelay(_goldMedal));
		HideInput ();
		HideMedals();
		HidePause ();
	}

	public IEnumerator StartAnimationWithDelay(GameObject medal) {
		yield return new WaitForSeconds(1);
		medal.GetComponent<Animation>().Play();
	}
	public void Lose()
	{
		GameObject.Find ("LevelDone").GetComponent<Animation> ().Play ();
		GameObject.Find ("DoneText").GetComponent<Text> ().text = _gameOverCaption;
		HideInput();
		HidePause ();
		HideMedals();
	}
    
	public void HideInput() {
		foreach (GameObject controller in GameObject.FindGameObjectsWithTag("Controllers"))
			controller.GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

    public void ShowInput() {
        foreach (GameObject controller in GameObject.FindGameObjectsWithTag("Controllers"))
			controller.GetComponent<RectTransform> ().localScale = Vector3.one;
    }

    public void Pause() {
        if (GameObject.Find("Goal").GetComponent<Goal>().OnPause)
        {            
            transform.FindChild("Back").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("Goal").GetComponent<Goal>().UnPause();
        }
        else
        {
            GameObject.Find("Goal").GetComponent<Goal>().Pause();
            transform.FindChild("Back").GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

	public void HidePause() {
		GameObject.Find ("Pause").GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

    public void Back()
    {
        string levelName = SceneManager.GetActiveScene().name;
        int level;
        var levelManger = GameObject.FindGameObjectWithTag("LevelManger").GetComponent<LevelManager>();
        if (int.TryParse(levelName, out level))
		{
            levelManger.LoadLevelChooseScene();
			transform.FindChild("Timer").GetComponent<RectTransform>().localScale = Vector3.zero;
			HidePause();
		}
        // if (levelName.Equals(GameObject.FindGameObjectWithTag("LevelManger").GetComponent<LevelManager>().LevelsInfoLocation))
            // levelManger.LoadMainScene();
    	// if (levelName == "MainMenu")
            // Application.Quit();
    }
}
