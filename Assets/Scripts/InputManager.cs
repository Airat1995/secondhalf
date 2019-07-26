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
    void FixedUpdate() {
        Debug.Log(CrossPlatformInputManager.GetAxis("HorizontalLeft"));
        Debug.Log(CrossPlatformInputManager.GetAxis("HorizontalRight"));
        Debug.Log(CrossPlatformInputManager.GetAxis("VerticalLeft"));
        Debug.Log(CrossPlatformInputManager.GetAxis("VerticalLeft"));
        var firstPlayer = new Vector3(CrossPlatformInputManager.GetAxis("HorizontalLeft") * _invertedX, 0, CrossPlatformInputManager.GetAxis("VerticalLeft") * _invertedY);
        var	secondPlayer = new Vector3(CrossPlatformInputManager.GetAxis("HorizontalRight") * _invertedX, 0, CrossPlatformInputManager.GetAxis("VerticalRight") * _invertedY);
        if (Mathf.Abs(firstPlayer.x) < MinSwipeSize && Mathf.Abs(firstPlayer.z) < MinSwipeSize)
            return;
        if(Mathf.Abs(secondPlayer.x) < MinSwipeSize && Mathf.Abs(secondPlayer.z) < MinSwipeSize)
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
