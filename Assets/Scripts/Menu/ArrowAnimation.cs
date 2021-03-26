using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowAnimation : MonoBehaviour {

	float currpos;
	float vel;

	// Use this for initialization
	void Start () {
		currpos = 80f;
		vel = 100f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 x = this.GetComponent<RectTransform>().anchoredPosition;
		if(currpos > 180) {
			vel*= -1;
		} else if(currpos < -20) {
			vel *= -1;
		}
		currpos += vel * Time.deltaTime;
		x.y = currpos;
		this.GetComponent<RectTransform>().anchoredPosition = x;
	}
}
