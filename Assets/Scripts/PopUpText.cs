using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour {

	float fadeintime;
	float _fadeintime;
	float fadeouttime;
	float _fadeouttime;

	// Use this for initialization
	void Start () {
		_fadeintime = _fadeouttime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if(_fadeouttime > 0) {
			this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 200f, 0f), 3f * Time.deltaTime);
			Color c = this.GetComponent<Text>().color;
			c.a = Mathf.Lerp(c.a, 0f, 3f * Time.deltaTime);
			this.GetComponent<Text>().color = c;
			_fadeouttime -= Time.deltaTime;
			if(_fadeouttime < 0) {
				this.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 300f, 0f);
			}
		} else if(_fadeintime > 0) {
			this.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(this.GetComponent<RectTransform>().anchoredPosition, new Vector3(0f, 180f, 0f), 3f * Time.deltaTime);
			Color c = this.GetComponent<Text>().color;
			c.a = Mathf.Lerp(c.a, 1f, 3f * Time.deltaTime);
			this.GetComponent<Text>().color = c;
			_fadeintime -= Time.deltaTime;
			if(_fadeintime < 0) {
				_fadeouttime = fadeouttime;
			}
		}
	}

	public void SetAnimation(string s, float fit, float fot, string c) {
		Color col = new Color(0f, 0f, 0f, 0f);
		if(c == "white") {
			col = new Color(1f, 1f, 1f, 0f);
		} else if(c == "black") {
			col = new Color(0f, 0f, 0f, 0f);
		} else if(c == "blue") {
			col = new Color(0f, 0f, 1f, 0f);
		} else if(c == "red") {
			col = new Color(1f, 0f, 0f, 0f);
		}
		fadeintime = fit;
		fadeouttime = fot;
		gameObject.GetComponent<Text>().text = s;
		gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 50f, 0f);
		_fadeintime = fadeintime;
		gameObject.GetComponent<Text>().color = col;
	}
}
