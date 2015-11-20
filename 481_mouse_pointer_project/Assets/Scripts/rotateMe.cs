using UnityEngine;
using System.Collections;

public class rotateMe : MonoBehaviour {
	public int speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<Rigidbody2D>().MoveRotation (GetComponent<Rigidbody2D>().rotation + speed * Time.deltaTime);
	}
}
