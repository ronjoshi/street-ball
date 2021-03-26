using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionChecker : MonoBehaviour {
	[SerializeField]
	int collisionnum;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Ball") {
			if(collisionnum == 1) {
				LevelManager.buckets1 = true;
			} else if (collisionnum == 2) {
				LevelManager.buckets2 = true;
			}

		}
	}
}
