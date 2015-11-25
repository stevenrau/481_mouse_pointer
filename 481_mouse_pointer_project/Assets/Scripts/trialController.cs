using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;

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

	private string methodName = "";
	public string backgroundName = "";

	private int trialCount = 0;
	private int skipCount = 0;

	
	void Start () {

		reshuffle (cursor);
		reshuffle (desktop);

		totalTrials = (desktop.Length * cursor.Length) * subTrialCount;
		times = new recordSet (totalTrials + 2000);


		gameObject.GetComponent<timer>().unpauseTime();
		changeDesktop ();
		spawnMouse ();



	}
	

	void Update () {
		if (Input.GetKeyDown ("space") && !gameObject.GetComponent<timer>().isPaused ()) {
			skipStep();
			Destroy(GameObject.FindGameObjectWithTag("cursor"));
		}
	}

	void reshuffle(GameObject[] methods)
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < methods.Length; t++ )
		{
			GameObject tmp = methods[t];
			int r = Random.Range(t, methods.Length);
			methods[t] = methods[r];
			methods[r] = tmp;
		}
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

		methodName = GameObject.FindGameObjectWithTag ("cursor").transform.name;


	}

	void printData() {

		//The csv file will be named YEAR-MONTH-DAY_HOUR-MINUTE
		string curDateTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".csv";

		TextWriter write = new StreamWriter(curDateTime);

		write.WriteLine ("trial number,time,method,background,skipped?");

		string isSkip;

		for (int i=0;i<totalTrials+skipCount;i++) {
			record r = times.getItem (i);

			if (r.getSkip ()) {
				isSkip = "skipped";
			} else {
				isSkip = " ";
			}

			string mName = r.getMethodName().Replace("(clone)", "");
			string bName = r.getBackgroundName().Replace("(clone)", "");

			write.WriteLine (r.getNumber() + "," + r.getTime() + "," + mName + "," + bName + ", " + isSkip);
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

		trialCount ++;

		backgroundName = GameObject.FindGameObjectWithTag ("desktop").transform.name;
		times.addRecord (tempTime, methodName, backgroundName, trialCount, false);
		Debug.Log (trialCount + ", " + methodName + ", " + backgroundName);
		gameObject.GetComponent<timer> ().resetTime ();

		//If the last trial of the current type is finished, move to the next cursor desktop. If there is no next desktop, move to the next cursor method
		counter++;
		if (counter == subTrialCount) {
			counter = 0;
			desktopCount++;

			if (desktopCount == (desktop.Length)) {
				desktopCount = 0;
				reshuffle (desktop);
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


	public void skipStep () {
		
		//Add the time to the record
		gameObject.GetComponent<timer>().pauseTime();
		float tempTime = gameObject.GetComponent<timer> ().getTime ();
		
		trialCount ++;
		skipCount ++;

		reshuffle (desktop);
		
		backgroundName = GameObject.FindGameObjectWithTag ("desktop").transform.name;
		times.addRecord (tempTime, methodName, backgroundName, trialCount, true);
		Debug.Log (trialCount + ", " + methodName + ", " + backgroundName);
		gameObject.GetComponent<timer> ().resetTime ();
		
		//If the last trial of the current type is finished, move to the next cursor desktop. If there is no next desktop, move to the next cursor method
//		counter++;
//		if (counter == subTrialCount) {
//			counter = 0;
//			//desktopCount++;
//			
//			if (desktopCount == (desktop.Length)) {
//				desktopCount = 0;
//				reshuffle (desktop);
//				//cursorCount++;
//				
//				if (cursorCount == (cursor.Length)) {
//					cursorCount = 0;
//					printData ();
//					Application.LoadLevel("testingCompleted");
//					return;
//					
//				}
//			}
//			
//		}
		
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
