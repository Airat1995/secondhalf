using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Goal : MonoBehaviour {
#region Medals color
	private string _bronzeColor;

	private string _silverColor;

	private string _goldColor;
#endregion

#region Times
	public float AllTime;

	private float _currentGameTime;

	public float[] Times;

	private Text _timer;

#endregion

#region Game state
	private bool _levelDone;

	private bool _onPause;

	public bool OnPause {
		get {
			return _onPause;
		}
	}
	
	public float CameraPosition = 10;
#endregion

	private GameObject _player1;
	private GameObject _player2;

	private List<AIMovement> _levelsGameObject = new List<AIMovement>();

	void Start()
	{
		_currentGameTime = AllTime;
		_timer = GameObject.Find ("Timer").GetComponent<Text>();
		_player1 = GameObject.FindGameObjectWithTag ("Player1");
		_player2 = GameObject.FindGameObjectWithTag ("Player2");
		_bronzeColor = GameObject.Find("Canvas").GetComponent<UIManager>().BronzeColor;
		_silverColor = GameObject.Find("Canvas").GetComponent<UIManager>().SilverColor;
		_goldColor = GameObject.Find("Canvas").GetComponent<UIManager>().GoldColor;
		var cam = GameObject.FindWithTag("MainCamera");
		cam.transform.position = new Vector3(0.0f, CameraPosition, 0.0f);
		cam.transform.rotation = Quaternion.Euler(90,0,0);
	}

	void Update()
	{
		if (_onPause)
			return;
		if (_levelDone)
			return;
		if (_currentGameTime > 0) {
			_currentGameTime -= Time.deltaTime;
			string timerColor;
			if (_currentGameTime >= Times [0]) {
				timerColor = _goldColor;
			} else if (_currentGameTime >= Times [1]) {
				timerColor = _silverColor;
			}
			else
				timerColor = _bronzeColor;
			_timer.text = string.Format ("<color=\"{0}\"> {1}:{2:00}</color>", timerColor, (int)(_currentGameTime / 60), (int)(_currentGameTime % 60));
		} 
		else {
			GameObject.Find ("Canvas").GetComponent<UIManager> ().Lose ();
			_levelDone = true;
		}

		if (_player1.GetComponent<PlayerComponent> ().OnFinish && _player2.GetComponent<PlayerComponent> ().OnFinish) {
			ResetPlayers();
			int winMedals = 1;
			if(_currentGameTime >= Times[0])
				winMedals = 3;
			else if(_currentGameTime >= Times[1])
				winMedals = 2;
			_levelDone = true;
			GameObject.Find ("Canvas").GetComponent<UIManager> ().Win (winMedals, AllTime - _currentGameTime);
		}
		if (_player1.transform.position.y < -0.5f || _player2.transform.position.y < -0.5f) {
			ResetPlayers();
			_levelDone = true;
			GameObject.Find ("Canvas").GetComponent<UIManager> ().Lose ();
		}
	}

	private void ResetPlayers()
	{
		_player1.GetComponent<PlayerComponent> ().OnFinish = false;
		_player2.GetComponent<PlayerComponent> ().OnFinish = false;
	}

	public void Pause()
	{
		foreach (var ai in GameObject.FindObjectsOfType<AIMovement>())
			ai.GetComponent<AIMovement>().Pause();
		_onPause = true;
	}

	public void UnPause()
	{
		foreach (var ai in GameObject.FindObjectsOfType<AIMovement>())
			ai.GetComponent<AIMovement>().UnPause();
		_onPause = false;
		if (_levelDone)
			return;
	}
}
