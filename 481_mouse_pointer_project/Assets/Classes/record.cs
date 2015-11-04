using UnityEngine;
using System.Collections;

public class record {

	private float timeTaken;
	private int method;
	private int background;

	public record(float t, int m, int b) {
		setTime (t);
		setMethod (m);
		setBackground (b);
	}

	public float getTime() {
		return timeTaken;
	}

	public int getMethod() {
		return method;
	}

	public int getBackground() {
		return background;
	}

	public void setTime(float t) {
		timeTaken = t;
	}

	public void setMethod(int m) {
		method = m;
	}

	public void setBackground(int b) {
		background = b;
	}
}
