using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningCarMove : MonoBehaviour {

	Vector3 StartPos;
	Vector3 EndPos;
	float speed = 5f;

	// Use this for initialization
	void Start () {
		ResetCar ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.MoveTowards (transform.position, EndPos, speed * Time.deltaTime);

		if (transform.position.Equals (EndPos)) {
			ResetCar ();
		}
	}

	void ResetCar() {
		StartPos = new Vector3 (-10, Random.Range (-3, 1), 0);
		EndPos = new Vector3 (10, StartPos.y, 0);
		transform.position = StartPos;
	}
}
