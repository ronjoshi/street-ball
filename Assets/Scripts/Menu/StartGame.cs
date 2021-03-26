using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

	[SerializeField]
	GameObject p1slider;
	[SerializeField]
	GameObject p2slider;
	[SerializeField]
	GameObject playto;
	[SerializeField]
	GameObject miti;

	public static int v1;
	public static int v2;
	public static int v3;
	public static bool v4;

	void Start() {
		DontDestroyOnLoad(gameObject);
	}

	public void OnClick ()
	{
		v1 = (int)p1slider.GetComponent<Slider> ().value;
		v2 = (int)p2slider.GetComponent<Slider> ().value;
		v3 = (int)playto.GetComponent<Slider>().value;
		v4 = miti.GetComponent<Toggle>().isOn;

		SceneManager.LoadScene("Main");
	}
}
