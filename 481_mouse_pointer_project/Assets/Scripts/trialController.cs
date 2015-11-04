using UnityEngine;
using System.Collections;
using System.IO;

public class trialController : MonoBehaviour {

	public GameObject[] desktop;
	public Transform[] mouseSpawn;

	public int subTrialCount;
	public GameObject[] cursor;

	private int desktopCount = 0;
	private int cursorCount = 0;
	private int counter = 0;
	private recordSet times;
	private int lastRand=999;
	
	private int totalTrials;

	
	void Start () {

		totalTrials = (desktop.Length * cursor.Length) * subTrialCount;
		times = new recordSet (totalTrials);


		gameObject.GetComponent<timer>().unpauseTime();
		changeDesktop ();
		spawnMouse ();

	}
	

	void Update () {
	
	}

	void changeDesktop () {

		destroyDesktop ();

		GameObject newDesktop = (GameObject)Instantiate (desktop[desktopCount], transform.position, transform.rotation);
	}

	void destroyDesktop () {
		
		GameObject[] gameObjects;
		
		gameObjects = GameObject.FindGameObjectsWithTag ("desktop");
		
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}
	}
	
	void spawnMouse () {


		int rand = Random.Range (0, (mouseSpawn.Length - 1));
		while (rand == lastRand) {
			rand = Random.Range (0, (mouseSpawn.Length - 1));
		}
		lastRand = rand;


		GameObject newMouse = (GameObject)Instantiate (cursor[cursorCount], mouseSpawn[rand].position, mouseSpawn[rand].rotation);

	}

	void printData() {
		TextWriter write = new StreamWriter("data.csv");

		write.WriteLine ("time,method,background");

		for (int i=0;i<totalTrials;i++) {
			record r = times.getItem (i);
			write.WriteLine (r.getTime() + "," + descriptiveMethod(r.getMethod()) + "," + descriptiveBackground(r.getBackground()));
		}

		write.Close ();
	}

	public void nextStep () {

		//Add the time to the record
		gameObject.GetComponent<timer>().pauseTime();
		float tempTime = gameObject.GetComponent<timer> ().getTime ();
		times.addRecord (tempTime, cursorCount, desktopCount);
		gameObject.GetComponent<timer> ().resetTime ();

		//If the last trial of the current type is finished, move to the next cursor desktop. If there is no next desktop, move to the next cursor method
		counter++;
		if (counter == subTrialCount) {
			counter = 0;
			cursorCount++;

			if (cursorCount == (cursor.Length)) {
				cursorCount = 0;
				desktopCount++;

				if (desktopCount == (desktop.Length)) {
					desktopCount = 0;
					printData ();
					Application.LoadLevel("testingCompleted");

				}
			}
		
		}
		changeDesktop ();
		spawnMouse ();
		gameObject.GetComponent<timer>().unpauseTime();
	}

	string descriptiveMethod(int i){
		string temp = "none";
		switch (i) {
		case 0:
			temp = "normal";
			break;
		case 1:
			temp = "size";
			break;
		}
		return temp;
	}

	string descriptiveBackground(int i){
		string temp = "none";
		switch (i) {
		case 0:
			temp = "busy";
			break;
		case 1:
			temp = "clean";
			break;
		}
		return temp;
	}
}
