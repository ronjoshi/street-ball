using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

	[SerializeField]
	GameObject hoop;
	GameObject popuptext;

	public bool isbouncing;
	float bouncevel;
	float maxvel;

	[SerializeField]
	float bouncepoint;
	[SerializeField]
	float bounceaccel;

	bool isshooting;
	bool isdoneshooting;
	public bool reattachable;

	float _reattachtime;

	[SerializeField]
	float turnovertime;
	float _turnovertime;

	// Use this for initialization
	void Start () {
		popuptext = GameObject.FindGameObjectWithTag("PopUpText");
		isbouncing = true;
		isshooting = reattachable = false;
		bouncevel = maxvel = _turnovertime = 0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(_turnovertime > 0) {
			_turnovertime -= Time.deltaTime;
		}

		if (isshooting) {
			if(_reattachtime <= 0) {
				reattachable = true;
			} else {
				_reattachtime -= Time.deltaTime;
			}
		} else {
			if (isbouncing) {
				if (transform.position.y >= bouncepoint) {
				} else {
					if (bouncevel < 0) {
						if (maxvel > 0) {
							bouncevel = maxvel + 0;
						} else {
							bouncevel *= -1;
							maxvel = bouncevel + 0;
						}
					}
				}
				bouncevel -= bounceaccel * Time.deltaTime;
				transform.Translate (new Vector3 (0f, bouncevel * Time.deltaTime, 0f));
			} else {
				transform.localPosition = new Vector3 (0f, 3f, 1.6f);
			}
		}
	}

	void LateUpdate ()
	{
		if (transform.parent == null) {
			Vector3 x = transform.position;
			if (x.z > 33) {
				x.z = 33f;
			} else if (x.z < 0) {
				x.z = 0f;
			}

			if(x.y < bouncepoint) {
				x.y = bouncepoint;
			}

			if (x.x > 17) {
				x.x = 17f;
			} else if (x.x < -17) {
				x.x = -17f;
			}
			transform.position = x;
		}
	}

	void OnCollisionEnter (Collision col)
	{
		if (reattachable) {
			if (col.gameObject.tag == "Player1") {
				Reattach (1, false);
			} else if (col.gameObject.tag == "Player2") {
				Reattach (2, false);
			}
		} else {
			if (_turnovertime <= 0) {
				if (col.gameObject.tag == "Player1") {
					if (!GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerMovement> ().ballincontrol) {
						_turnovertime = turnovertime;
						Disattach ();
						reattachable = true;

						Reattach (1, true);

						popuptext.GetComponent<PopUpText> ().SetAnimation ("Turnover!", 0.5f, 0.5f, "blue");
					}
				} else if (col.gameObject.tag == "Player2") {
					if (!GameObject.FindGameObjectWithTag ("Player2").GetComponent<PlayerMovement> ().ballincontrol) {
						_turnovertime = turnovertime;
						Disattach ();
						reattachable = true;

						Reattach (2, true);

						popuptext.GetComponent<PopUpText> ().SetAnimation ("Turnover!", 0.5f, 0.5f, "red");
					}
				}
			}
		}
	}

	public void Shoot (float shotmeter)
	{
		//Brings ball to a shooting height and disattaches from player's gameobject.
			transform.localPosition = new Vector3 (0f, 3f, 1.6f);
			Disattach ();

		/*A vector3 is a 3 dimensional coordinate point/vector direction. The variable
		gotopos is the position of the center of the hoop that it needs to go to.

		transform.position is the vector3 position of the ball.
		*/
			Vector3 gotopos = hoop.transform.position;

		/*
		Horizvector is the vector the ball needs to travel ONLY on the x and z axes.
		See that the y component (vertical) is set to 0. horizdistance is simply distance
		of the horizontal vector (or the horizontal distance it needs to travel).
		*/
			Vector3 horizvector = gotopos - transform.position;
			horizvector.y = 0f;
			float horizdistance = horizvector.magnitude;

		/*
		This manipulates the horizontal distance the ball will travel based on how accurate
		the shot is. If shotmeter is above the center of the green line, it overshoots; if it's
		below, then it undershoots.
		*/
			if (GameObject.FindGameObjectWithTag ("Player1").GetComponent<PlayerMovement> ().ballincontrol) {
				horizdistance += horizdistance / 20f * LevelManager.player1difficulty * shotmeter / 100f;
			} else {
				horizdistance += horizdistance / 20f * LevelManager.player2difficulty * shotmeter / 100f;
			}

		/*
		t is the time it will take to travel. Minimum time is 0.8 seconds, and the time in seconds
		will increase by 0.1 every unit change in horizontal distance.
		*/
			float t = 0.8f + 0.1f * horizdistance;

		//This simply makes the object look toward the bottom of the hoop (disregarding height disparity)
			transform.rotation = Quaternion.LookRotation(horizvector);

		//thisy is the y position of the ball; gotoy is the y position of the hoop.
			float thisy = transform.position.y;
			float gotoy = hoop.transform.position.y;

		/*
		vy is the starting velocity in the y direction; vx is the starting velocity in x direction

		KINEMATIC FORMULA:
			delta y = 0.5 * a * t^2 + vy * t
			delta y - 0.5 * a * t^2 = vy * t
			vy = (delta y - 0.5 * a * t^2) / t
			vy = ((gotoy - thisy) - 0.5 * -9.81 * t^2) / t
		is how you calculate velocity in y direction.

		Since you have time it will take to travel:

		KINEMATIC FORMULA 2:
			delta x = vx * t
			vx = delta x / t
			vx = horizdistance / t
		*/
			float vy = ((gotoy - thisy) + 0.5f * 9.81f * Mathf.Pow(t, 2f)) / t;
			float vx = (horizdistance / t);

		/*
		this is the amount of time it will take for the player to be able to pick up the ball again.
		this happens at the top of the ball's arc, or when y velocity is 0.
			0 = vy (vy initial) + a * t
			0 = vy + -9.81 * t
			-vy = -9.81 * t
			t = vy / 9.81
		Here, t is the time at which the ball is at its peak (vertical velocity is 0).
		*/
			_reattachtime = vy / 9.81f;

		//this combines the vectors in the horizontal and vertical directions into one - called vvector (velocity vector).
			Vector3 vvector = new Vector3(0f, vy, vx);

		/* this is an in-built function that adds an initial velocity in a certain direction - just for this frame.
		it adds the required velocity in the direction relative to itself, so it points perfectly where
		a -9.81 gravity acceleration constant will drop it into the hoop.
		*/
			this.GetComponent<Rigidbody>().AddRelativeForce(vvector, ForceMode.VelocityChange);

		//this just gives the ball a little back spin while traveling in the air. It adds a torque just for one frame.
			this.GetComponent<Rigidbody>().AddTorque(transform.right * -200f * horizdistance);
	}

	public void Disattach() {
		gameObject.GetComponent<Rigidbody>().isKinematic = false;
		gameObject.GetComponent<Rigidbody>().useGravity = true;
		isshooting = true;
		isbouncing = false;
		transform.parent = null;
	}

	public void Reattach(int p, bool isturnover) {
		gameObject.GetComponent<Rigidbody> ().isKinematic = true;
		gameObject.GetComponent<Rigidbody> ().useGravity = false;
		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		isbouncing = true;
		bouncevel = maxvel = 0f;
		isshooting = reattachable = false;

		if(p == 1) {
			GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>().StartDribbling(isturnover);
			transform.parent = GameObject.FindGameObjectWithTag("Player1").GetComponent<Transform>();
			GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().ballincontrol = false;
		} else {
			GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerMovement>().StartDribbling(isturnover);
			Debug.Log("2 started dribbling");
			transform.parent = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
			GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerMovement>().ballincontrol = false;
		}

		transform.localPosition = new Vector3(0f, 3f, 1.6f);
	}

	public float GetDistance() {
		Vector3 horizvector = (hoop.transform.position - transform.position);
		horizvector.y = 0f;
		return horizvector.magnitude;
	}
}
