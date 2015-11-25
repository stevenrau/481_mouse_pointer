using UnityEngine;
using System.Collections;

public class record {

	private float timeTaken;
	private string methodName;
	private string backgroundName;
	private int number;
	private bool skip;

	public record(float t, string mn, string bn, int n, bool skip) {
		setTime (t);
		setBackgroundName (bn);
		setMethodName (mn);
		setNumber (n);
		setSkip (skip);
	}

	public void setSkip (bool s) {
		skip = s;
	}

	public bool getSkip () {
		return skip;
	}

	public void setNumber(int n) {
		number = n;
	}

	public int getNumber() {
		return number;
	}

	public float getTime() {
		return timeTaken;
	}

	public void setTime(float t) {
		timeTaken = t;
	}

	public void setBackgroundName (string s) {
		backgroundName = s;
	}

	public string getBackgroundName () {
		return backgroundName;
	}

	public void setMethodName (string s) {
		methodName = s;
	}
	
	public string getMethodName () {
		return methodName;
	}
}
