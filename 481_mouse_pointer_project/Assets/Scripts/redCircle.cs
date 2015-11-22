using UnityEngine;
using System.Collections;

public class redCircle : MonoBehaviour {

	public Texture circleImage; // the image to use as a new mouse pointer
	float diameter = 40.0f;
	float radius;

	float xCorrection = 3.0f;

	// Use this for initialization
	void Start () {
		radius = diameter / 2.0f;
		Cursor.visible = false;
	}


	public void OnGUI()
	{
		Vector3 mousePos = Input.mousePosition;

		Rect pos = new Rect(mousePos.x - radius + xCorrection, (Screen.height - mousePos.y) - radius, diameter, diameter);

		GUI.Label(pos, circleImage);
	}

	// Update is called once per frame
	//void Update () {
	//}
}
