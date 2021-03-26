using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayToSlider : MonoBehaviour {
	[SerializeField]
	GameObject t;

	public void UpdateText() {
		t.GetComponent<Text>().text = "Play to " + this.GetComponent<Slider>().value.ToString();
	}
}
