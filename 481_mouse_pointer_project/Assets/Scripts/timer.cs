using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

	public float seconds = 0;
	private bool pause = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!pause) {
			seconds = seconds + Time.deltaTime;
		}
	}

	public float getTime() {
		return seconds;
	}

	public void resetTime() {
		seconds = 0;
	}

	public void pauseTime() {
		pause = true;
	}

	public void unpauseTime() {
		pause = false;
	}
}
