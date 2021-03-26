using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Header("General")]
	public bool ballincontrol;

	[Header("References")]
	GameObject ball;
	GameObject hoop;
	GameObject shotmeter;
	GameObject popuptext;

	[Header("Keys")]
	[SerializeField]
	KeyCode upkey;
	[SerializeField]
	KeyCode downkey;
	[SerializeField]
	KeyCode leftkey;
	[SerializeField]
	KeyCode rightkey;
	[SerializeField]
	KeyCode shootkey;

	Vector3 gotorot;
	float movementSpeed;

	bool shooting;
	public bool blocking;
	[SerializeField]
	float jumpvel;
	float _jumpvel;
	[SerializeField]
	float jumpaccel;

	[SerializeField]
	float turnoverwait;
	float _turnoverwait;

	public int pointstoadd;

	// Use this for initialization
	void Start () {
		ball = GameObject.FindGameObjectWithTag("Ball");
		hoop = GameObject.FindGameObjectWithTag("Hoop");
		popuptext = GameObject.FindGameObjectWithTag("PopUpText");
		shotmeter = GameObject.FindGameObjectWithTag("Shot meter");

		gotorot = new Vector3(0f, 0f, 1f);
		movementSpeed = 10f;
		shooting = blocking = false;
		_jumpvel = jumpvel;
		_turnoverwait = 0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_turnoverwait > 0) {
			_turnoverwait -= Time.deltaTime;
		}
		
		if (shooting) {
			_jumpvel -= jumpaccel * Time.deltaTime;
			transform.Translate (new Vector3 (0f, _jumpvel * Time.deltaTime, 0f));

			if (transform.position.y <= 0) {
				transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
				shooting = false;
				ballincontrol = false;
				_jumpvel = jumpvel;
			}
		} else {
			if (ballincontrol) {
				ball.GetComponent<BallMovement>().isbouncing = true;

				if (LevelManager.movable) {
					Move ();

					if (Input.GetKey (shootkey) && _turnoverwait <= 0) {

						ball.GetComponent<BallMovement>().isbouncing = false;

						Vector3 hooppos = hoop.transform.position;
						hooppos.y = 0f;
						if (Vector3.Distance (hooppos, transform.position) < 4) {
							popuptext.GetComponent<PopUpText> ().SetAnimation ("Can't shoot here!", 0.5f, 0.5f, "black");
						} else {
							if (gameObject.tag == "Player1") {
								pointstoadd = ThreePointCollision.player1range + 0;
							} else {
								pointstoadd = ThreePointCollision.player2range + 0;
							}

							shooting = true;
							Vector3 tohoop = hoop.transform.position - transform.position;
							tohoop.y = 0;
							transform.rotation = Quaternion.LookRotation (tohoop);

							if (gameObject.tag == "Player1") {
								LevelManager.lastshot = 1;
							} else {
								LevelManager.lastshot = 2;
							}
							shotmeter.GetComponent<ShotMeter> ().Shoot (shootkey);
						}
					}
				}
			} else {
				if (blocking) {
					_jumpvel -= jumpaccel * Time.deltaTime;
					transform.Translate (new Vector3 (0f, _jumpvel * Time.deltaTime, 0f));

					if (transform.position.y <= 0) {
						transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
						blocking = false;
						_jumpvel = jumpvel;
					}
				}

				if (LevelManager.movable) {
					Move ();

					if (Input.GetKey (shootkey)) {
						if (!blocking) {
							blocking = true;
						}
					}
				}
			}
		}
	}

	public void StartDribbling (bool isturnover)
	{
		shooting = blocking = false;
		transform.position = new Vector3 (transform.position.x, 0f, transform.position.z);
		ballincontrol = true;
		_jumpvel = jumpvel;
		if (isturnover) {
			_turnoverwait = turnoverwait;
		}
	}

	void LateUpdate() {
		Vector3 x = transform.position;
		if(x.z > 33) {
			x.z = 33f;
		} else if(x.z < 0) {
			x.z = 0f;
		}

		if(x.x > 17) {
			x.x = 17f;
		} else if(x.x < -17) {
			x.x = -17f;
		}
		transform.position = x;
	}

	void Move() {
		if (Input.GetKey (upkey)) {
			if (Input.GetKey (leftkey)) {
				gotorot = new Vector3 (-1f, 0f, 1f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			} else if (Input.GetKey (rightkey)) {
				gotorot = new Vector3 (1f, 0f, 1f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			} else {
				gotorot = new Vector3 (0f, 0f, 1f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			}
		} else if (Input.GetKey (downkey)) {
			if (Input.GetKey (leftkey)) {
				gotorot = new Vector3 (-1f, 0f, -1f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			} else if (Input.GetKey (rightkey)) {
				gotorot = new Vector3 (1f, 0f, -1f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			} else {
				gotorot = new Vector3 (0f, 0f, -1f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			}
		} else {
			if (Input.GetKey (leftkey)) {
				gotorot = new Vector3 (-1f, 0f, 0f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			} else if (Input.GetKey (rightkey)) {
				gotorot = new Vector3 (1f, 0f, 0f);
				transform.Translate (Vector3.forward * movementSpeed * Time.deltaTime);
			}
		}

		transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.LookRotation (gotorot), 6f * Time.deltaTime);
	}
}
