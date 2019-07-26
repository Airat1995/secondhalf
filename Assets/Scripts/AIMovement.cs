using UnityEngine;

public class AIMovement : MonoBehaviour
{
	private const float PRECISION = 0.01f;

    public GameObject FirstSpot;

    public GameObject SecondSpot;

	public float Speed = 0.05f;
	
	private bool _isPause = false;

	// if var is true then move AI to first spot, else move AI to second spot
	private bool moveToFirstSpot = true;
    // Update is called once per frame
    void FixedUpdate () {
		if(_isPause)
			return;
		if(AlmostEqual(transform.position.x, FirstSpot.transform.position.x) && AlmostEqual(transform.position.z, FirstSpot.transform.position.z) && moveToFirstSpot == true)
			moveToFirstSpot = false;
		if(AlmostEqual(transform.position.x, SecondSpot.transform.position.x) && AlmostEqual(transform.position.z, SecondSpot.transform.position.z) && moveToFirstSpot == false)
			moveToFirstSpot = true;
		if(moveToFirstSpot)
			transform.position = Vector3.MoveTowards(transform.position, FirstSpot.transform.position, Speed);
		else
			transform.position = Vector3.MoveTowards(transform.position, SecondSpot.transform.position, Speed);
	}

	private static bool AlmostEqual(float firstFloat, float secondFloat)
	{
		return Mathf.Abs (firstFloat - secondFloat) <= PRECISION;
	}

	public void Pause()
	{
		_isPause = true;
	}

	public void UnPause()
	{
		_isPause = false;
	}

}
