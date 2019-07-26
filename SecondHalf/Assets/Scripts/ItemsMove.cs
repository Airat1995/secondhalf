using System.Collections; 
using System.Collections.Generic; 
using System.IO; 
using UnityEngine; 
using UnityEngine.UI; 
using SQLite;
using Mono.Data.Sqlite;

public class ItemsMove : MonoBehaviour { 

	public Sprite SuccessSprite; 

	public Sprite FailrueSprite; 

	public GameObject LevelInfoGameObject; 

	public int LevelsCount;
	
	private bool _isLastItemOutsideScreen;

	private void Start() 
	{	
		using(var db = new SQLiteConnection(Path.Combine(Application.dataPath, GameObject.FindWithTag("LevelManger").GetComponent<LevelManager>().LevelChoose)))
		{
		List<LevelInfo> levels = db.Query<LevelInfo>("SELECT * from " + GameObject.FindWithTag("LevelManger").GetComponent<LevelManager>().TableName);
		Debug.Log("Levels done: " + levels.Count);
		Vector2 size = LevelInfoGameObject.GetComponent<RectTransform>().sizeDelta;
		Debug.Log(size);
		for (int i = 0; i < levels.Count; ++i) 
		{ 
			var levelInfo = Instantiate(LevelInfoGameObject, transform) as GameObject; 
			levelInfo.GetComponent<RectTransform>().sizeDelta = new Vector2(100.0f, 100.0f); 
			levelInfo.GetComponent<Image>().sprite = SuccessSprite; 
			levelInfo.transform.FindChild("LevelNum").GetComponent<Text>().text = (i + 1).ToString(); 
			levelInfo.GetComponent<RectTransform>().anchoredPosition = new Vector3(size.x / 2 + i * size.x, 0, 0); 
			levelInfo.transform.FindChild("BronzeMedal").GetComponent<RectTransform>().localScale = Vector3.one; 
			if (levels[i].Medal >= 2) 
				levelInfo.transform.FindChild("SilverMedal").GetComponent<RectTransform>().localScale = Vector3.one; 
			if (levels[i].Medal	== 3) 
				levelInfo.transform.FindChild("GoldMedal").GetComponent<RectTransform>().localScale = Vector3.one; 
			levelInfo.name = (i+1).ToString();
			levelInfo.GetComponent<Button>().onClick.AddListener(() => {RunLevel(levelInfo.name);});
		}
		if (levels.Count >= LevelsCount) 
			return;
		var lastLevel = Instantiate(LevelInfoGameObject, transform) as GameObject; 
		lastLevel.GetComponent<RectTransform>().anchoredPosition = new Vector3(size.x / 2 + levels.Count * size.x, 0, 0); 
		lastLevel.GetComponent<Image>().sprite = FailrueSprite; 
		lastLevel.transform.FindChild("LevelNum").GetComponent<Text>().text = (levels.Count + 1).ToString(); 
		lastLevel.name = (levels.Count+1).ToString();
		lastLevel.GetComponent<Button>().onClick.AddListener(() => {RunLevel(lastLevel.name);});
		_isLastItemOutsideScreen = gameObject.GetComponent<RectTransform>().sizeDelta.x < lastLevel.GetComponent<RectTransform>().anchoredPosition.x;
		Debug.Log(lastLevel.GetComponent<RectTransform>().anchoredPosition);
		db.Close();
		}
	} 

	void Update () { 
		if(Input.touchCount == 0)
			return;
		var firstChildLocation = transform.GetChild(0).GetComponent<RectTransform>();
		var lastChildLocation = transform.GetChild(transform.childCount - 1).GetComponent<RectTransform>();
		var canvasSize = gameObject.GetComponent<RectTransform>().sizeDelta;
		if(_isLastItemOutsideScreen)
		{
			if(Input.touches[0].deltaPosition.x < 0 && lastChildLocation.anchoredPosition.x > canvasSize.x + lastChildLocation.sizeDelta.x/2)
				MoveChildren(Input.touches[0].deltaPosition);
			else if(Input.touches[0].deltaPosition.x > 0 && firstChildLocation.anchoredPosition.x < firstChildLocation.sizeDelta.x)
				MoveChildren(Input.touches[0].deltaPosition);
		}
		else
		{
			if(lastChildLocation.anchoredPosition.x < canvasSize.x - 1.5 * lastChildLocation.sizeDelta.x && Input.touches[0].deltaPosition.x > 0)
				MoveChildren(Input.touches[0].deltaPosition);
			else if(firstChildLocation.anchoredPosition.x > firstChildLocation.sizeDelta.x/2 && Input.touches[0].deltaPosition.x < 0)
					MoveChildren(Input.touches[0].deltaPosition);
		}
	}

	private void MoveChildren(Vector2 moveVector)
	{
		foreach(Transform child in transform)
			child.gameObject.GetComponent<RectTransform>().anchoredPosition += new Vector2(moveVector.x, 0);
	}

	public void RunLevel(string levelName)
	{
		GameObject.FindGameObjectWithTag("LevelManger").GetComponent<LevelManager>().LoadScene(levelName);
	}
}
