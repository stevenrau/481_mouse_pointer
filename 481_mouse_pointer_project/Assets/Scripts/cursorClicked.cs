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

		if (GameObject.Find ("Trial Controller")) {
			GameObject.Find ("Trial Controller").GetComponent<trialController> ().setMethodName (this.GetComponent<Transform>().name);
			GameObject.Find ("Trial Controller").GetComponent<trialController> ().nextStep ();
		} else if (GameObject.Find ("Practice Controller")) {
			GameObject.Find ("Practice Controller").GetComponent<practiceController> ().nextStep ();
		}
		Destroy (gameObject);
	}
}
