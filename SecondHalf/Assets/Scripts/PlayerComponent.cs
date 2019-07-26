using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour {

	private const string _finishTag = "Finish";

	private float _speed = 100;

	public bool OnFinish{ get; set; }

	void OnTriggerEnter(Collider coll){
		if (coll.gameObject.tag == _finishTag + gameObject.tag)
			OnFinish = true;
	}

	void OnTriggerExit(Collider coll){
		if (coll.gameObject.tag == _finishTag + gameObject.tag)
			OnFinish = false;
	}

	public void Move(Vector3 movementVector) {
		transform.position += Vector3.Lerp (movementVector, Vector3.zero, Time.deltaTime);
	}
}