using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMove : MonoBehaviour {
	
    public float Angle = 0;
    public float Speed =  10;
    private float _radius = 4;
	// Update is called once per frame
	
	void Update () {
    if (Angle >= 360)
        Angle = 0;
    Angle += Speed * Time.deltaTime; //if you want to switch direction, use -= instead of +=
    transform.position = new Vector3(Mathf.Cos(Mathf.Deg2Rad * Angle) * _radius, transform.position.y,
        Mathf.Sin(Mathf.Deg2Rad * Angle) * _radius);
	}
}
