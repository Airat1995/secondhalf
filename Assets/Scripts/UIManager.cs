using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using GoogleMobileAds.Api;

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

	public const string PLAY_STRING = "PlayMusic";

	private GameObject _pauseGO;
	private GameObject _backGO;
	private GameObject _levelManager;

	#region LevelDonePanelGO
	private GameObject _levelDone;
	
	private	 GameObject _replay;
	private	 GameObject _next;
	private	 GameObject _bronze;
	private	 GameObject _silver;
	private	 GameObject _gold;
	#endregion

	private GameObject _audioManager;
//	private BannerView _banner;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad (GameObject.Find ("EventSystem"));
		_settingsPanel = GameObject.Find("Settings");

		_levelManager = GameObject.Find("LevelManger");

		int invX = PlayerPrefs.GetInt(_inversedX, 1);
		int invY = PlayerPrefs.GetInt(_inversedY, 1);
		int vol =  PlayerPrefs.GetInt(PLAY_STRING, 1);

		transform.Find("Title").GetComponent<RectTransform>().localScale = Vector3.one;	
		transform.Find("Start").GetComponent<RectTransform>().localScale = Vector3.one;
		transform.Find("StartText").GetComponent<RectTransform>().localScale = Vector3.one;
		transform.Find("SettingsButton").GetComponent<RectTransform>().localScale = Vector3.one;
		transform.Find("Pause").GetComponent<RectTransform>().localScale = Vector3.zero;

		GameObject.Find("InvertX").GetComponent<Toggle>().isOn = invX == -1;
		GameObject.Find("InvertY").GetComponent<Toggle>().isOn = invY == -1;
		GameObject.Find("PlayMusic").GetComponent<Toggle>().isOn = vol == 1;

		_backGO = transform.Find("Back").gameObject;
		_levelDone = transform.Find("LevelDone").gameObject;
		_pauseGO = transform.Find("Pause").gameObject;
		_bronze = GameObject.Find("Bronze").gameObject;
		_silver = GameObject.Find("Silver").gameObject;
		_gold = GameObject.Find("Gold").gameObject;
		_replay = GameObject.Find("Replay").gameObject;
		_next = GameObject.Find("Next").gameObject;

		_audioManager = GameObject.Find("AudioManager");

//		RequestBanner ();

	}
//
//	private void RequestBanner()
//	{
//	    #if UNITY_EDITOR
//	        string adUnitId = "unused";
//	    #elif UNITY_ANDROID
//			string adUnitId = "ca-app-pub-9838663717682289/8871357086";
//	    #elif UNITY_IPHONE
//			string adUnitId = "ca-app-pub-9838663717682289/8544054021";
//	    #else
//	        string adUnitId = "unexpected_platform";
//	    #endif
//	
//	    // Create a 320x50 banner at the top of the screen.
//		_banner = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
//	    // Create an empty ad request.
//		AdRequest request = new AdRequest.Builder().Build();
//	    // Load the banner with the request.
//	    _banner.LoadAd(request);
//		_banner.Show();
//	}	

    public void StartGame()
    {
//		_banner.Hide ();
        int level = PlayerPrefs.GetInt("LastLevel", 1);
        _levelManager.GetComponent<LevelManager>().LoadScene(level.ToString());
		PrepeareGameUI();
	}

	public void PrepeareGameUI()
	{
		foreach (Transform child in transform)
        {
            child.GetComponent<RectTransform>().localScale = Vector3.zero;
        }
        GameObject.Find("Timer").GetComponent<RectTransform>().localScale = Vector3.one;
		_pauseGO.GetComponent<RectTransform>().localScale = Vector3.one;
//		_banner.Hide ();
	}

    public void HideMedals()
    {
		_bronze.GetComponent<RectTransform>().localScale = Vector3.zero;
	    _silver.GetComponent<RectTransform>().localScale = Vector3.zero;
	    _gold.GetComponent<RectTransform>().localScale = Vector3.zero;
	}

	public void ShowPause()
	{
		_pauseGO.GetComponent<RectTransform>().localScale = Vector3.one;
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
        PlayerPrefs.SetInt(PLAY_STRING, isMusicPlay ? 1 : 0);
        PlayerPrefs.Save();

		if(GameObject.FindGameObjectWithTag("InputManager")!=null)
			GameObject.FindGameObjectWithTag("InputManager").GetComponent<InputManager>().UpdateInver();

        GameObject.Find("AudioManager").GetComponent<AudioManager>().UpdateSoundVolume(isMusicPlay ? 1 : 0);
		if (GameObject.Find ("Goal") != null)
			GameObject.Find ("Goal").GetComponent<Goal> ().UnPause ();
    }

	public void Win(int medals, float levelTime)
	{
//		_banner.Show ();
		HidePause();
		HideInput ();
		HideMedals();
		_audioManager.GetComponent<AudioManager>().LevelWin();
		_replay.GetComponent<Button>().interactable = false;

		_next.GetComponent<RectTransform>().localScale = Vector3.one;
		_next.GetComponent<Button>().interactable = false;
		_levelDone.GetComponent<Animation>().Play();	
        
		StartCoroutine(WaitForAnimation(_bronze.GetComponent<Animation>(), new Button[]{_replay.GetComponent<Button>(), _next.GetComponent<Button>()}));
		_bronze.GetComponent<Animation>().Play();
		if(medals >= 2)
			_silver.GetComponent<Animation>().Play();
		if(medals == 3)
			_gold.GetComponent<Animation>().Play();
		GameObject.Find("DoneText").GetComponent<Text>().text = _levelDoneCaption;
		
		_levelManager.GetComponent<LevelManager>().Win(medals, levelTime);
	}
	
	

	public void Lose()
	{
//		_banner.Show ();
		_audioManager.GetComponent<AudioManager>().Lose();
		HideInput();
		HidePause ();
		HideMedals();
		_replay.GetComponent<Button>().interactable = false;
		GameObject.Find ("DoneText").GetComponent<Text> ().text = _gameOverCaption;
		GameObject.Find ("LevelDone").GetComponent<Animation> ().Play ();
		StartCoroutine(WaitForAnimation(_levelDone.GetComponent<Animation>(), new Button[]{ _replay.GetComponent<Button>()}));
	}
	
	private IEnumerator WaitForAnimation ( Animation animation, Button[] activeButtons )
	{
    	do
    	{
        	yield return null;
    	} while ( animation.isPlaying );
		foreach (var button in activeButtons)
			button.interactable = true;
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
			//_banner.Hide ();
			ShowInput();	
            transform.Find("Back").GetComponent<RectTransform>().localScale = Vector3.zero;
            GameObject.Find("Goal").GetComponent<Goal>().UnPause();
        }
        else
        {
			HideInput();
            GameObject.Find("Goal").GetComponent<Goal>().Pause();
            transform.Find("Back").GetComponent<RectTransform>().localScale = Vector3.one;
        }
    }

	public void HidePause() {
		_pauseGO.GetComponent<RectTransform> ().localScale = Vector3.zero;
	}

	public void ReloadLevel()
	{
		_levelManager.GetComponent<LevelManager>().ReloadLevel();
		HideMedals();
		_next.GetComponent<RectTransform>().localScale = Vector3.zero;
		_levelDone.GetComponent<RectTransform>().localScale = Vector3.zero;
		_pauseGO.GetComponent<RectTransform>().localScale = Vector3.one;
//		_banner.Hide ();
	}

	public void NextLevel()
	{
		int level;
	   	if (int.TryParse(SceneManager.GetActiveScene().name, out level))
			if(level >= LevelManager.LevelsCount)
				return;
		_levelManager.GetComponent<LevelManager>().LoadNextScene();
		HideMedals();
		_next.GetComponent<RectTransform>().localScale = Vector3.zero;
		_pauseGO.GetComponent<RectTransform>().localScale = Vector3.one;
//		_banner.Hide ();
		
	}

    public void Back()
	{
//		_banner.Show ();
		string sceneName = SceneManager.GetActiveScene ().name;
		int levelNum;
		transform.Find ("Timer").GetComponent<RectTransform> ().localScale = Vector3.zero;
		HidePause ();
		if (int.TryParse (sceneName, out levelNum)) {
			_levelManager.GetComponent<LevelManager>().LoadLevelChooseScene ();
			transform.Find("Back").GetComponent<RectTransform>().localScale = Vector3.zero;
			transform.Find("SettingsButton").gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
		}
	}

	public void HideBanner()
	{
//		_banner.Hide ();
	}
}
