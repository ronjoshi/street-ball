using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounce : MonoBehaviour {

	[SerializeField]
	float startvel;
	float _startvel;
	[SerializeField]
	float accel;

	// Use this for initialization
	void Start () {
		_startvel = startvel;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(0f, _startvel * Time.deltaTime, 0f));
		_startvel -= accel * Time.deltaTime;
		if(transform.position.y <= 0.5) {
			_startvel = startvel;
		}
	}
}
