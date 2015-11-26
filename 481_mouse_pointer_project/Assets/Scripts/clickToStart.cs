using UnityEngine;
using System.Collections;

public class clickToStart : MonoBehaviour {

	private float timer = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;

		if (timer > 3.0f) {
			GetComponent<SpriteRenderer>().enabled = true;
		}
	}

	void OnMouseDown() {
		if (timer > 3.0f) {
			Application.LoadLevel ("lowFidelityPrototype");
		}
	}
}
