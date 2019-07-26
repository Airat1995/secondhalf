using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class InputManager : MonoBehaviour {

    [Range(0f, 0.5f)]
    public float MovementSensetive;

    public GameObject FirstPlayersGameObject;

    public GameObject SecondPlayersGameObject;

    private float MinSwipeSize = 0.01f;

    private int _invertedX;
    private int _invertedY;

    void Start() {
        UpdateInver();
        FirstPlayersGameObject = GameObject.FindGameObjectWithTag("Player1");
        SecondPlayersGameObject = GameObject.FindGameObjectWithTag("Player2");
    }

    // Update is called once per frame
    void Update() {
        Vector3 firstPlayer = new Vector3(CrossPlatformInputManager.GetAxis("HorizontalLeft") * _invertedX, 0, CrossPlatformInputManager.GetAxis("VerticalLeft") * _invertedY);
        Vector3 secondPlayer = new Vector3(CrossPlatformInputManager.GetAxis("HorizontalRight") * _invertedX, 0, CrossPlatformInputManager.GetAxis("VerticalRight") * _invertedY);
        // Debug.Log(firstPlayer);
        // Debug.Log(secondPlayer);
        if (Mathf.Abs(firstPlayer.x) < MinSwipeSize ||  Mathf.Abs(firstPlayer.z) < MinSwipeSize)
            return;
        if(Mathf.Abs(secondPlayer.x) < MinSwipeSize || Mathf.Abs(secondPlayer.z) < MinSwipeSize)
            return;
		FirstPlayersGameObject.GetComponent<PlayerComponent> ().Move (firstPlayer * MovementSensetive);
		SecondPlayersGameObject.GetComponent<PlayerComponent> ().Move (secondPlayer * MovementSensetive);
	}

	public void UpdateInver() {
		string invX = GameObject.Find("Canvas").GetComponent<UIManager>().InversedXName;
		string invY = GameObject.Find("Canvas").GetComponent<UIManager>().InversedYName;
		_invertedX = PlayerPrefs.GetInt (invX, 1);
		_invertedY = PlayerPrefs.GetInt (invY, 1);
	}
}
