using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
	[SerializeField]
	Vector3 playeroffset;
	[SerializeField]
	float playerfollowspeed;
	[SerializeField]
	float camerafollowspeed;
	Vector3 gotopos;
	float speed;

	bool gamestarted = true;
	Vector3 gotopos1;
	Vector3 gotorot1;

	float streamertime = 1.5f;
	float _streamertime;

	// Use this for initialization
	void Start () {
		_streamertime = 0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_streamertime > 0) {
			_streamertime -= Time.deltaTime;
			if(_streamertime <= 0) {
				if(LevelManager.gameover == 1) {
					transform.GetChild(0).GetComponent<ParticleSystem>().Play();
					transform.GetChild(1).GetComponent<ParticleSystem>().Play();
				} else {
					transform.GetChild(2).GetComponent<ParticleSystem>().Play();
					transform.GetChild(3).GetComponent<ParticleSystem>().Play();
				}
			}
		}

		if(LevelManager.gameover > 0) {
			if(streamertime > 0) {
				_streamertime = streamertime;
				streamertime = -1f;
			}

			Vector3 temp = Vector3.up;
			if(LevelManager.gameover == 1) {
				temp = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>().position;
			} else {
				temp = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>().position;
			}
			gotopos = temp + playeroffset;
			speed = playerfollowspeed;
			transform.position = Vector3.Lerp(transform.position, gotopos, speed * Time.deltaTime);
		} else if (gamestarted) {
			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerMovement> ().ballincontrol) {
				Vector3 temp = GameObject.FindGameObjectWithTag ("Player1").GetComponent<Transform> ().position;
				temp.y = 0f;
				gotopos = temp + playeroffset;
				speed = playerfollowspeed;
			} else if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerMovement> ().ballincontrol) {
				Vector3 temp = GameObject.FindGameObjectWithTag ("Player2").GetComponent<Transform> ().position;
				temp.y = 0f;
				gotopos = temp + playeroffset;
				speed = playerfollowspeed;
			} else {
				Vector3 temp = GameObject.FindGameObjectWithTag ("Ball").GetComponent<Transform> ().position;
				temp.y = 0f;
				gotopos = temp + playeroffset;
				speed = camerafollowspeed;
			}
			transform.position = Vector3.Lerp (transform.position, gotopos, speed * Time.deltaTime);
		}
	}
}
