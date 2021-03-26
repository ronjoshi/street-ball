using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePointCollision : MonoBehaviour {
	public static int player1range = 3;
	public static int player2range = 2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		if(col.gameObject.tag == "Player1") {
			player1range = 2;
		} else if(col.gameObject.tag == "Player2") {
			player2range = 2;
		}
	}

	void OnTriggerExit(Collider col) {
		if(col.gameObject.tag == "Player1") {
			player1range = 3;
		} else if(col.gameObject.tag == "Player2") {
			player2range = 3;
		}
	}
}
