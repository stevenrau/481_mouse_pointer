using UnityEngine;
using System.Collections;
using System.IO;

public class trialController : MonoBehaviour {

	public GameObject[] desktop;
	public GameObject[] regionSet;
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

		GameObject newRegionSet = (GameObject)Instantiate (regionSet[desktopCount], transform.position, transform.rotation);

		GameObject[] regions = GameObject.FindGameObjectsWithTag ("region");
		
		for(var i = 0 ; i < regions.Length ; i ++)
		{
			regions[i].GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	void destroyDesktop () {
		
		GameObject[] gameObjects;
		
		gameObjects = GameObject.FindGameObjectsWithTag ("desktop");
		
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}

		gameObjects = GameObject.FindGameObjectsWithTag ("regionSet");
		
		for(var i = 0 ; i < gameObjects.Length ; i ++)
		{
			Destroy(gameObjects[i]);
		}
	}
	
	void spawnMouse () {


//		int rand = Random.Range (0, (mouseSpawn.Length - 1));
//		while (rand == lastRand) {
//			rand = Random.Range (0, (mouseSpawn.Length - 1));
//		}
//		lastRand = rand;
//
//
//		GameObject newMouse = (GameObject)Instantiate (cursor[cursorCount], mouseSpawn[rand].position, mouseSpawn[rand].rotation);

		GameObject[] regions;

		regions = GameObject.FindGameObjectsWithTag ("region");

		int rand = Random.Range (0, regions.Length);

		Transform regionCenter = regions [rand].GetComponent<Transform> ();

		float randX = Random.Range (-(regionCenter.localScale.x / 2),(regionCenter.localScale.x / 2));
		float randY = Random.Range (-(regionCenter.localScale.y / 2),(regionCenter.localScale.y / 2));

		Transform cursorDestination = regionCenter;

		cursorDestination.transform.position = new Vector3 (regionCenter.position.x + randX, regionCenter.position.y + randY, -5);

		GameObject newMouse = (GameObject)Instantiate (cursor[cursorCount], cursorDestination.position, cursorDestination.rotation);

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

	public void continueTrial () {

		changeDesktop ();
		spawnMouse ();
		gameObject.GetComponent<timer>().unpauseTime();

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
			desktopCount++;

			if (desktopCount == (desktop.Length)) {
				desktopCount = 0;
				cursorCount++;

				if (cursorCount == (cursor.Length)) {
					cursorCount = 0;
					printData ();
					Application.LoadLevel("testingCompleted");
					return;

				}
			}
		
		}

		GameObject.Find ("clicktocontinue").GetComponent<clickToContinue> ().show ();


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
		case 2:
			temp = "colour";
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
