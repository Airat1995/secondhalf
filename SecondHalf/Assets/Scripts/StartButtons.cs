using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtons : MonoBehaviour
{
    private Image _rSlider;

    private Image _lSlider;

    public float FillingTime;

    private float _currentFillingTime;

    private int _counter = 0;

    void Start()
    {
        _rSlider = GameObject.Find("RSlider").GetComponent<Image>();
        _lSlider = GameObject.Find("LSlider").GetComponent<Image>();
    }

	// Update is called once per frame
	void Update () {
		_rSlider.fillAmount = _currentFillingTime / FillingTime;
		_lSlider.fillAmount = _currentFillingTime / FillingTime;
        if(_counter!=2)
            return;
	    _currentFillingTime += Time.deltaTime;
		if (_currentFillingTime >= FillingTime) {
			_counter = 0;
			_currentFillingTime = 0;
			transform.parent.GetComponent<UIManager> ().StartGame ();
		}
	}

    public void StartFilling()
    {
        _counter++;
        _currentFillingTime = 0;
    }

    public void StopFilling()
    {
        _counter--;
        _currentFillingTime = 0;
    }
}
