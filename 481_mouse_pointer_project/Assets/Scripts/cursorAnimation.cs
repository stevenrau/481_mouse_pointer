using UnityEngine;
using System.Collections;

public class cursorAnimation : MonoBehaviour {

	public float speed;
	public int startScale;

	private bool animating = false;
	private float timer = 0.0f;
	private bool timing = false;

	// Use this for initialization
	void Start () {
		initializeSize();
		startAnimating ();
	}
	
	// Update is called once per frame
	void Update () {

		if (timing) {
			timer += Time.deltaTime;
		}

		if (timing && timer > 1.0f) {
			timing = false;
			timer = 0.0f;
			initializeSize ();
			startAnimating ();
		}


		if (animating) {
			Vector3 tempScale = GetComponent<Transform>().localScale;
			tempScale = new Vector3(tempScale.x -speed, tempScale.y -speed, tempScale.z);
			GetComponent<Transform>().localScale = tempScale;

			if (tempScale.x <= 2) {
				animating = false;
				GetComponent<SpriteRenderer>().enabled = false;
				timing = true;
			}
		}

		//if (Input.GetKeyDown ("a")) {
		//	startAnimating ();
		//}
	}

	void startAnimating () {
		initializeSize ();
		animating = true;
	}

	void initializeSize() {
		Vector3 tempScale = GetComponent<Transform>().localScale;
		tempScale = new Vector3(startScale, startScale, startScale);
		GetComponent<Transform>().localScale = tempScale;
		GetComponent<SpriteRenderer>().enabled = true;

	}


}

