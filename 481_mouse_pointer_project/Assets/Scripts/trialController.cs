using UnityEngine;
using System.Collections;
using System.IO;
using System.Globalization;

public class trialController : MonoBehaviour {

	private readonly string[] methodNames = { "cursor", "cursorBig", "cursorAnimated",
		                                     "cursorInverted", "cursorInvertedBig", "cursorInvertedAnimated",
	                                         "cursorBigAnimated", "cursorInvertedBigAnimated"};

	private readonly int numBackgrounds = 3;

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

	public GameObject targetObject;

	
	void Start () {

		reshuffle (cursor);
		reshuffle (desktop);

		totalTrials = (desktop.Length * cursor.Length) * subTrialCount;
		times = new recordSet (totalTrials + 2000);


		//added to show a previw of the mouse type before each trial
		GameObject previewMouse = (GameObject)Instantiate (cursor[cursorCount]);
		previewMouse.transform.position = new Vector3 (0f, -3.0f, -4.0f);
		previewMouse.name = "previewMouse";
		previewMouse.GetComponent<SpriteRenderer> ().sortingOrder = 150;
		previewMouse.GetComponent<Collider2D> ().enabled = false;
		previewMouse.GetComponent<cursorClicked> ().enabled = false;
		
		//similarily, show the word target beside the preview mouse pointer
		GameObject target = (GameObject)Instantiate (targetObject);
		target.transform.position = new Vector3 (0f, -2.07f, -0.0f);
		target.name = "target";
		target.GetComponent<SpriteRenderer> ().sortingOrder = 150;


		//gameObject.GetComponent<timer>().unpauseTime();
		//changeDesktop ();
		//spawnMouse ();



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

		float randX = Random.Range (-8.72f,8.72f);
		float randY = Random.Range (-4.5f,4.5f);
		
		Transform cursorDestination = regionCenter;
		
		cursorDestination.transform.position = new Vector3 (randX, randY, -5);

		GameObject newMouse = (GameObject)Instantiate (cursor[cursorCount], cursorDestination.position, cursorDestination.rotation);

		//methodName = GameObject.FindGameObjectWithTag ("cursor").transform.name;


	}

	public void setMethodName (string s) {
		methodName = s;
	}

	void printData() {

		//The csv file will be named YEAR-MONTH-DAY_HOUR-MINUTE
		string curDateTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm") + ".csv";

		TextWriter write = new StreamWriter(curDateTime);

		write.WriteLine ("trial number,time,method,background,skipped?,average,standard deviation");

		record r = null;
		string mName = "";
		string bName = "";
		string isSkip = "";
		float curTime = 0.0f;
		float curTotal = 0.0f;
		int numTrials = 0;
		float[] methodTrials = new float[subTrialCount * numBackgrounds];

		for (int i=0;i<totalTrials+skipCount;i++) {

			r = times.getItem (i);
			curTime = r.getTime();

			mName = r.getMethodName().Replace("(Clone)", "");
			bName = r.getBackgroundName().Replace("(Clone)", "");

			if (r.getSkip ()) {
				isSkip = "skipped";
			} else {
				isSkip = " ";
			}

			// If this trial wasn't skipped, add the time to the total and count it as a trial
			if (!r.getSkip())
			{
				methodTrials[numTrials] = curTime;
				curTotal += curTime;
				numTrials++;
			}

			// Write the trial to the file (last two spots are the avg and std dev vals that get printed at the end of each method)
			write.WriteLine (r.getNumber() + "," + r.getTime() + "," + mName + "," + bName + ", " + isSkip, " , , "); 

			// If that was the last trial for that method, print out the avg and std. dev and reset counters
			if (numTrials == subTrialCount * numBackgrounds)
			{
				float stdDev = 0.0f;
				float sumDiffSquared = 0.0f;
				float mean = curTotal/numTrials;

				for (int j = 0; j < subTrialCount * numBackgrounds; j++)
				{
					// Get the difference of the current time and the mean and square it.
					sumDiffSquared += Mathf.Pow((methodTrials[j] - mean), 2);
				}

				//Complete the std dev. calculation with the sum of the squared differences
				stdDev = Mathf.Sqrt(sumDiffSquared/(subTrialCount * numBackgrounds));

				write.WriteLine (" , , , , ," + mean + "," + stdDev);

				// Reset the counters
				curTotal = 0.0f;
				numTrials = 0;
			}
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

		//added to show a previw of the mouse type before each trial
		GameObject previewMouse = (GameObject)Instantiate (cursor[cursorCount]);
		previewMouse.transform.position = new Vector3 (0f, -3.0f, -4.0f);
		previewMouse.name = "previewMouse";
		previewMouse.GetComponent<SpriteRenderer> ().sortingOrder = 150;
		previewMouse.GetComponent<Collider2D> ().enabled = false;
		previewMouse.GetComponent<cursorClicked> ().enabled = false;
		
		//similarily, show the word target beside the preview mouse pointer
		GameObject target = (GameObject)Instantiate (targetObject);
		target.transform.position = new Vector3 (0f, -2.07f, -0.0f);
		target.name = "target";
		target.GetComponent<SpriteRenderer> ().sortingOrder = 150;


	}


	public void skipStep () {
		
		//Add the time to the record
		gameObject.GetComponent<timer>().pauseTime();
		float tempTime = gameObject.GetComponent<timer> ().getTime ();
		
		trialCount ++;
		skipCount ++;

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
