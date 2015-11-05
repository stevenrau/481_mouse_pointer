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
		GameObject.Find ("Trial Controller").GetComponent<trialController> ().continueTrial ();
		//Destroy (gameObject);

		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.GetComponent<BoxCollider2D> ().enabled = false;

	}

	public void show() {

		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;

	}


}
