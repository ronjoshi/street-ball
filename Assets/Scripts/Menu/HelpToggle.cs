using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpToggle : MonoBehaviour {

	[SerializeField]
	GameObject maincanvas;
	[SerializeField]
	GameObject helpcanvas;
	bool isonmain;

	// Use this for initialization
	void Start () {
		isonmain = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Switch() {
		if(isonmain) {
			maincanvas.GetComponent<Canvas>().enabled = false;
			helpcanvas.GetComponent<Canvas>().enabled = true;
		} else {
			helpcanvas.GetComponent<Canvas>().enabled = false;
			maincanvas.GetComponent<Canvas>().enabled = true;
		}
		isonmain = !isonmain;
	}
}
