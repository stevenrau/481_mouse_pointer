using UnityEngine;
using System.Collections;

public class cursorClicked : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		GameObject.Find ("Trial Controller").GetComponent<trialController> ().nextStep ();
		Destroy (gameObject);
	}
}
