using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	[Header("References")]
	[SerializeField]
	GameObject bluescore;
	[SerializeField]
	GameObject redscore;
	[SerializeField]
	GameObject playtotext;
	GameObject popuptext;

	GameObject player1;
	GameObject player2;

	int redpoints;
	int bluepoints;

	public static float player1difficulty;
	public static float player2difficulty;
	[SerializeField]
	int playto;
	[SerializeField]
	bool makeittakeit;

	public static bool buckets1;
	public static bool buckets2;
	public static bool movable;

	public static int lastshot;

	float bucketscountdown = 1f;
	float _bucketscountdown;

	float _gamestartime = 3f;

	float resetwait = 2f;
	float _resetwait;
	float resettime = 1f;
	float _resettime;
	float postresettime = 1f;
	float _postresettime;
	float wontime = 2f;
	float _wontime;
	bool resettoblue;

	public static int gameover;

	// Use this for initialization
	void Start () {
		player1difficulty = 4 + 2*StartGame.v1;
		player2difficulty = 4 + 2*StartGame.v2;
		playto = StartGame.v3;
		makeittakeit = StartGame.v4;

		player1 = GameObject.FindGameObjectWithTag("Player1");
		player2 = GameObject.FindGameObjectWithTag("Player2");

		buckets1 = buckets2 = false;
		_bucketscountdown = _resetwait = _resettime = _postresettime = _wontime = 0f;
		redpoints = bluepoints = 0;
		popuptext = GameObject.FindGameObjectWithTag("PopUpText");

		movable = false;

		gameover = 0;

		playtotext.GetComponent<Text>().text = "Playing to " + playto.ToString();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_gamestartime > 0) {
			_gamestartime -= Time.deltaTime;
			if(_gamestartime <= 0) {
				movable = true;
				popuptext.GetComponent<PopUpText>().SetAnimation("Go!", 1f, 1f, "black");
			}
		}

		if (_bucketscountdown <= 0) {
			if (buckets1 || buckets2) {
				_bucketscountdown = bucketscountdown;
			} else {
				buckets1 = buckets2 = false;
			}
		} else {
			if(buckets1 && buckets2) {
				Splash();
				buckets1 = false;
				buckets2 = false;
				_bucketscountdown = 0;
			}
			_bucketscountdown -= Time.deltaTime;
			if(_bucketscountdown <= 0) {
				buckets1 = buckets2 = false;
			}
		}


		if(_postresettime > 0) {
			_postresettime -= Time.deltaTime;

			if(_postresettime <= 0) {
				movable = true;
				popuptext.GetComponent<PopUpText>().SetAnimation("Go!", 1f, 1f, "black");
				_postresettime = _resettime = _resetwait = 0f;
			}
		} else if(_resettime > 0) {
			_resettime -= Time.deltaTime;
			if(resettoblue) {
				player1.transform.position = Vector3.Lerp(player1.transform.position, new Vector3(0f, 0f, 13f), 6f * Time.deltaTime);
				player2.transform.position = Vector3.Lerp(player2.transform.position, new Vector3(0f, 0f, 17f), 6f * Time.deltaTime);
			} else {
				player1.transform.position = Vector3.Lerp(player1.transform.position, new Vector3(0f, 0f, 17f), 6f * Time.deltaTime);
				player2.transform.position = Vector3.Lerp(player2.transform.position, new Vector3(0f, 0f, 13f), 6f * Time.deltaTime);
			}

			if(_resettime <= 0) {
				if(resettoblue) {
					player1.transform.position = new Vector3(0f, 0f, 13f);
					player2.transform.position = new Vector3(0f, 0f, 17f);
				} else {
					player2.transform.position = new Vector3(0f, 0f, 13f);
					player1.transform.position = new Vector3(0f, 0f, 17f);
				}

				_postresettime = postresettime;
			}
		} else if(_resetwait > 0) {
			_resetwait -= Time.deltaTime;
			if(_resetwait < 0) {
				GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>().Disattach();
				GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>().reattachable = true;
				if(resettoblue) {
					GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>().Reattach(1, false);
					player1.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
					player2.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				} else {
					GameObject.FindGameObjectWithTag("Ball").GetComponent<BallMovement>().Reattach(2, false);
					player2.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
					player1.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
				}
				_resettime = resettime;
			}
		}

		if(_wontime > 0) {
			_wontime -= Time.deltaTime;

			if(_wontime <= 0) {
				Destroy(GameObject.FindGameObjectWithTag("Ball"));
				if(bluepoints > redpoints) {
					popuptext.GetComponent<PopUpText>().SetAnimation("Blue won!", 1f, 10f, "blue");
					gameover = 1;
					player1.transform.position = new Vector3(0f, 5f, 0f);
				} else {
					popuptext.GetComponent<PopUpText>().SetAnimation("Red won!", 1f, 10f, "red");
					gameover = 2;
					player2.transform.position = new Vector3(0f, 5f, 0f);
				}
			}
		}
	}

	void Splash ()
	{
		if (lastshot == 1) {
			bluepoints += GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerMovement> ().pointstoadd;
			bluescore.GetComponent<Text> ().text = "Blue Player: " + bluepoints.ToString ();
			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerMovement> ().pointstoadd == 3) {
				popuptext.GetComponent<PopUpText> ().SetAnimation ("3 pointer!", 1f, 0.5f, "blue");
			} else {
				popuptext.GetComponent<PopUpText> ().SetAnimation ("2 points!", 0.5f, 0.5f, "blue");
			}

			if (bluepoints < playto) {
				if (makeittakeit) {
					movable = false;
					_resetwait = resetwait;
					resettoblue = true;
				} else {
					movable = false;
					_resetwait = resetwait;
					resettoblue = false;
				}
			}

		} else {
			redpoints += GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerMovement> ().pointstoadd;
			redscore.GetComponent<Text> ().text = "Red Player: " + redpoints.ToString ();
			if (GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerMovement> ().pointstoadd == 3) {
				popuptext.GetComponent<PopUpText> ().SetAnimation ("3 pointer!", 1f, 0.5f, "red");
			} else {
				popuptext.GetComponent<PopUpText> ().SetAnimation ("2 points!", 0.5f, 0.5f, "red");
			}

			if (redpoints < playto) {
				if (makeittakeit) {
					movable = false;
					_resetwait = resetwait;
					resettoblue = false;
				} else {
					movable = false;
					_resetwait = resetwait;
					resettoblue = true;
				}
			}
		}

		if(bluepoints >= playto || redpoints >= playto) {
			if(bluepoints >= playto) {
				Debug.Log("BLUEWON");
			} else {
				Debug.Log("REDWON");
			}
			movable = false;
			_wontime = wontime;
		}
	}
}
