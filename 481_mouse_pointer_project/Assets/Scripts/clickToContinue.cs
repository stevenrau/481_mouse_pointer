using UnityEngine;
using System.Collections;

public class clickToContinue : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {

		//destroy the preview mouse
		Destroy (GameObject.Find ("previewMouse"));
		Destroy (GameObject.Find ("target"));

		if (GameObject.Find ("Trial Controller")) {
			GameObject.Find ("Trial Controller").GetComponent<trialController> ().continueTrial ();
		} else if (GameObject.Find ("Practice Controller")) {
			GameObject.Find ("Practice Controller").GetComponent<practiceController> ().continueTrial ();
		}

		//Destroy (gameObject);

		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;

	}

	public void show() {

		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;

	}


}
