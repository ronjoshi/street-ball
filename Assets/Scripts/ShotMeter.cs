using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotMeter : MonoBehaviour {

	KeyCode shootkey;

	[SerializeField]
	GameObject bar;
	[SerializeField]
	GameObject arrow;
	[SerializeField]
	GameObject ball;
	[SerializeField]
	GameObject text;

	[SerializeField]
	float textfloattime;
	float _textfloattime;

	float arrowheight;
	bool shooting;
	bool overshot;
	float dhdt;

	void Start() {
		shooting = false;
		_textfloattime = 0f;
	}

	void Update ()
	{
		if (_textfloattime > 0) {
			Color c = text.GetComponent<Text> ().color;
			c.a = (_textfloattime / textfloattime);
			text.GetComponent<Text> ().color = c;
			Vector3 tpos = text.GetComponent<RectTransform> ().anchoredPosition;
			tpos.x += 10f * (1 - (_textfloattime / textfloattime));
			text.GetComponent<RectTransform> ().anchoredPosition = tpos;
			_textfloattime -= Time.deltaTime;

			if (_textfloattime <= 0) {
				Color ar1 = arrow.transform.GetChild (0).GetComponent<Image> ().color;
				Color ar2 = arrow.transform.GetChild (1).GetComponent<Image> ().color;
				ar1.a = 0f;
				ar2.a = 0f;
				arrow.transform.GetChild (0).GetComponent<Image> ().color = ar1;
				arrow.transform.GetChild (1).GetComponent<Image> ().color = ar2;

				Color br = bar.GetComponent<Image> ().color;
				br.a = 0f;
				bar.GetComponent<Image> ().color = br;
			}
		} else {
			if (shooting) {
				if (Input.GetKey (shootkey) && !overshot) {
					Vector3 apos = arrow.GetComponent<RectTransform> ().anchoredPosition;
					arrowheight += dhdt * Time.deltaTime;
					apos.y = arrowheight;
					if (arrowheight > 100) {
						apos.y = arrowheight = 100;
						overshot = true;
					} else {
						arrow.GetComponent<RectTransform> ().anchoredPosition = apos;
					}
				} else {
					shooting = false;
					overshot = false;

					ball.GetComponent<BallMovement> ().Shoot (arrow.GetComponent<RectTransform> ().anchoredPosition.y);

					Vector3 tpos = text.GetComponent<RectTransform> ().anchoredPosition;
					tpos.x = -306f;
					tpos.y = arrow.GetComponent<RectTransform> ().anchoredPosition.y;
					text.GetComponent<RectTransform> ().anchoredPosition = tpos;
					text.GetComponent<Text> ().text = (Mathf.FloorToInt (100 - Mathf.Abs (arrowheight))).ToString () + "%";
					_textfloattime = textfloattime;
					Color c = text.GetComponent<Text> ().color;
					c.a = 1f;
					text.GetComponent<Text> ().color = c;
				}
			}
		}
	}

	public void Shoot(KeyCode sk) {
		shootkey = sk;
		shooting = true;
		arrowheight = -100f;
		dhdt = 200 + ball.GetComponent<BallMovement>().GetDistance() * 15;

		Color ar1 = arrow.transform.GetChild(0).GetComponent<Image>().color;
		Color ar2 = arrow.transform.GetChild(1).GetComponent<Image>().color;
		ar1.a = 1f;
		ar2.a = 1f;
		arrow.transform.GetChild(0).GetComponent<Image>().color = ar1;
		arrow.transform.GetChild(1).GetComponent<Image>().color = ar2;

		Color br = bar.GetComponent<Image>().color;
		br.a = 1f;
		bar.GetComponent<Image>().color = br;
	}
}
