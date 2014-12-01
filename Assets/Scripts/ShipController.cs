using UnityEngine;
using System.Collections;

public class ShipController : MonoBehaviour
{
	
		private Vector3 forward = new Vector3 (0.0f, 1.0f, 0.0f);
		private Vector3 back = new Vector3 (0.0f, -1.0f, 0.0f);
		private float speed = 15.0f;
		private bool isMission = true;
		private bool onMission = false;
		private bool missionFailed = false;
		private bool isGameOver = false;
		private bool play = true;
		private int seconds;
		private float time = 60.0f;
		private float interval = 60.0f;
		private string[] mission1 = new string[15];
		private string[] planetNames = new string[6];
		private Vector2[] planetCoordinates = new Vector2[6];
		private int nextIdx = 0;
		public GUISkin skin;
		private float minX = -5000.0f;
		private float maxX = 5000.0f;
		private float minY = -3100.0f;
		private float  maxY = 2900.0f;
		private float score = 0.0f;
		private bool isImperialCuntFace = false;
		private string planetName;
		private int planetIdx = 0;
		public GUIText gameOverText;
		public bool discoverable = false;
		private bool isNext = false;
		private bool isWin = false;
		private string winText;
		private int imperialTimer = 3;
		private int planetTimer = 3;
		private int delay = 3;
		public AudioClip[] audioClip;
		public AudioClip music;
		private string[] barks = new string[10];
		private bool isBark = false;
		private int timer = 3;
		private string currentBark;
		private bool isFirst = true;
		public float charsPerSecond = 1; // speed of typewriter
		private int missionTimeIndex = 0;
		private int missionTimer = 0;
		private float missionTime = 0f;
	
		void Update ()
		{
		
				if (!isGameOver) {
						if (!isMission) {
								if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
										Vector3 movement = forward * speed;
										gameObject.GetComponent<Animator> ().SetTrigger ("move");
										movement *= Time.deltaTime;
										transform.Translate (movement);
								}
				
								if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
										Vector3 movement = back * speed;
										movement *= Time.deltaTime;
										gameObject.GetComponent<Animator> ().SetTrigger ("move");
										transform.Translate (movement);
								}
				
								if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
										transform.Rotate (new Vector3 (0.0f, 0.0f, 1.0f));				
								}
				
								if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
										transform.Rotate (new Vector3 (0.0f, 0.0f, -1.0f));	
								}
				
								if (Input.GetKey (KeyCode.Space)) {
										gameObject.GetComponent<Animator> ().SetTrigger ("Blip");
										if (discoverable) {
												GameObject planet = GameObject.FindGameObjectWithTag (getPlanetString ());
												planet.GetComponent<SpriteRenderer> ().enabled = true;
												planet.GetComponent<CircleCollider2D> ().enabled = true;
												isMission = true;
												resetTime ();
												play = true;
												onMission = false;
												planetIdx++;
												score = seconds;
												isNext = true;
												time = interval;
												discoverable = false;
										}
								}
						}
			
						if (seconds == imperialTimer) {
								isImperialCuntFace = false;
						}
			
						if (seconds == timer) {
								isBark = false;
								int index = Random.Range (0, barks.Length);
								currentBark = barks [index];
						}
			
						if (seconds == planetTimer) {
								discoverable = false;
						}
			
			
						if (isMission && Input.GetKey (KeyCode.Return)) {
								isMission = false;
								onMission = true;
								nextIdx++;
								isFirst = false;
								missionTime = 0.0f;
						}
			
						if (Input.anyKey == false) {
								gameObject.GetComponent<Animator> ().SetTrigger ("idle");
						}
			
						if (onMission) {
								if (isNext) {
										nextIdx ++;
										isNext = false;
								}
								time -= Time.deltaTime;
								seconds = Mathf.FloorToInt (time);
								if (seconds < 1) {
										missionFailed = true;
										onMission = false;
										isMission = false;
										play = true;
										nextIdx++;
								}
						}

						if (isGenerateText ()) {
								missionTime += Time.deltaTime;
								missionTimer = Mathf.FloorToInt (missionTime * 50.0f);
								missionTimeIndex = Mathf.FloorToInt (missionTime);
						}
				}
		
				if (nextIdx == 10 || nextIdx == 11) {
						isGameOver = true;
						isWin = true;
						onMission = false;
						isMission = true;
						play = true;
						winText = "Congratulations! You managed to score : " + score.ToString ();
				}
		
		}

		bool isGenerateText ()
		{
				return isMission || missionFailed || isImperialCuntFace || isBark;
		}

		void resetTime ()
		{
				time = 0;
		}
	
	
		void OnGUI ()
		{
		
				GUI.skin = skin;
				GUIStyle ambassador = skin.GetStyle (getPlanetString ());
				GUIStyle textBoxLong = skin.GetStyle ("TextBoxLong");
				GUIStyle textBox = skin.GetStyle ("TextBox");
				GUI.Box (new Rect (Screen.width - 250, 0, 250, 30), getCoordinateString (), textBox);
				GUI.Box (new Rect (0, 0, 250, 30), "Score: " + score.ToString (), textBox);
				if (isMission) {
						GUI.Box (new Rect (0, Screen.height - 128, 128, 128), "", ambassador);
						if (play) {
								int index = Random.Range (0, audioClip.Length);
								gameObject.audio.PlayOneShot (audioClip [index]);
								play = false;
						}	
						if (isFirst && missionTimeIndex > 5) {
								string reminderText = getReminderText ();
								if (missionTimer / 300 < 2 && missionTimer % 300 < reminderText.Length) {
										GUI.Box (new Rect (128, Screen.height - 64, Screen.width - 128, 64), reminderText.Substring (0, missionTimer % 300), textBoxLong);
								} else {
										GUI.Box (new Rect (128, Screen.height - 64, Screen.width - 128, 64), reminderText, textBoxLong);
								}
						} else {
								string missionString = getMissionString ();
								getTypewrittenText (textBoxLong, missionString);
						}
			
				}
		
				if (onMission) {
						GUI.Box (new Rect (0, 35, 250, 30), getSecondsString (), textBox);
						GUI.Box (new Rect (Screen.width - 250, 35, 250, 30), getPlanetString () + " : " + getPlanetCoordinate (), textBox);
				}
				if (missionFailed) {
						string missionString = getMissionString ();
						getTypewrittenText (textBoxLong, missionString);
						if (play) {
								int index = Random.Range (0, audioClip.Length);
								gameObject.audio.PlayOneShot (audioClip [index]);
								play = false;
						}	
						isGameOver = true;
				}
				if (isImperialCuntFace && !isMission) {
						GUIStyle imperial = skin.GetStyle ("Imperial");
						GUI.Box (new Rect (0, Screen.height - 128, 128, 128), "", imperial);
						getTypewrittenText (textBoxLong, getImperialText ());
				}
		
				if (isBark && !isMission) {
						GUIStyle bark = skin.GetStyle ("Bark");
						GUI.Box (new Rect (0, Screen.height - 128, 128, 128), "", bark);
						string missionString = getBarkText ();
						getTypewrittenText (textBoxLong, missionString);
				}
		
				if (isGameOver) {
						if (isWin) {
								GameObject win = GameObject.FindGameObjectWithTag ("win");
								win.guiText.enabled = true;
								win.guiText.text = winText;
						}
						GameObject guitext = GameObject.FindGameObjectWithTag ("gameOver");
						guitext.guiText.enabled = true;
						if (Input.GetKey (KeyCode.Y)) {
								Application.LoadLevel (Application.loadedLevel);	
						}
						if (Input.GetKey (KeyCode.N)) {
								Application.Quit ();
						}
				}
		}

		void getTypewrittenText (GUIStyle style, string missionString)
		{
				if (missionTimer < missionString.Length) {
						GUI.Box (new Rect (128, Screen.height - 64, Screen.width - 128, 64), missionString.Substring (0, missionTimer), style);
				} else {
						GUI.Box (new Rect (128, Screen.height - 64, Screen.width - 128, 64), missionString, style);
				}
		}

		string getCoordinateString ()
		{
				int currentCoordinateX = Mathf.FloorToInt (transform.position.x);
				int currentCoordinateY = Mathf.FloorToInt (transform.position.y);
				return "x : " + currentCoordinateX + " y: " + currentCoordinateY;
		}
	
		string getSecondsString ()
		{
				return "Time Remaining : " + seconds.ToString ();
		}
	
	
		void Start ()
		{
				setText ();
				setPlanetNames ();
				setPlanetCoordinates ();		
				setBarks ();
				gameObject.audio.clip = music;
				gameObject.audio.loop = true;
				gameObject.audio.Play ();
		}
	
		string getImperialText ()
		{
				return "You can't pass here, there's a trade embargo!";
		}
	
		string getBarkText ()
		{
				return currentBark;
		}
	
		void setText ()
		{
				mission1 [0] = "Okay newbie, your first job here at Star Couriers is to get this tank of undead piranhas to the StarPope on Gromulon. You need to get it there fast or else he’ll eat you too, newb...";
				mission1 [1] = "Great job kid! That’s us safe for another cycle. Here, do us a space solid and deliver these boxes of spam to Bambomm. They’re having a celebratory ‘no-dead-to-the-StarPope’ party and there ain’t no party like a boxed spam party.";
				mission1 [2] = "Thanks for nothing, Newbie. The StarPope went on a feeding rampage instead. Seems like if we need parcels delivered, we’d have been faster flushing them down our cosmic toilet.";
				mission1 [3] = "Excellent, the meat boxes are here. We can begin. Thanks kid. So, we’ve found a typo in the latest print run of the interspecies Kama Sutra. Turns out you should NOT do number 34 with a Gorgox. Could you bring the reprints to anyone not already dead on Spinklehill? Time is of the sexy essence...";
				mission1 [4] = "Too late, kid. You may as well gorge on those meat boxes yourself; we’ve no use for them now.";
				mission1 [5] = "Great timing! I was about to stick this in that. I should have known Jimi wasn’t crying for no reason. Could you take him to the hospiworld of Yumulon to get sewn back together?";
				mission1 [6] = "So... much... blood...";
				mission1 [7] = "We can put him back together, but he’s gonna be a few inches smaller... Here, takes his offcuts home for dinner. ";
				mission1 [8] = "Oh... That’s been off for too long to reattach... Soz bbz!";
				mission1 [9] = "Back just in time for tea; great work newbie!";
				mission1 [10] = "Poisoned! Why did you bring us back out of date meat for dinner, newbie?";
		}
	
		void setBarks ()
		{
				barks [0] = "Beat it posty. We do our own deliveries around here.";
				barks [1] = "Beep beep boop, etc.";
				barks [2] = "01000110011101010110001101101011001000000110111101100110011001100010000001000101011000010111001001110100011010000110110001101001011011100110011100100001";
				barks [3] = "Klaatu barada nikto.";
				barks [4] = "I’m too blown up to help, sorry.";
				barks [5] = "Biddie-biddie-biddie! What's up, Buck?";
				barks [6] = "Are you the keymaster?";
				barks [7] = "Roads? Where we're going, we don't need roads.";
				barks [8] = "The planetary government you are trying to reach is on another call. Please hold and we’ll try to connect you.";
				barks [9] = "‘Out to lunch.’";
		}
	
		void setPlanetNames ()
		{
				planetNames [0] = "Gromulon";
				planetNames [1] = "Bambomm";
				planetNames [2] = "Spinklehill";
				planetNames [3] = "Yumulon";
				planetNames [4] = "home";
				planetNames [5] = planetNames [0];
		
		}

		string getReminderText ()
		{
				string reminder = "Remember to press the Star Pinger And Comms Equipment button (or S.P.A.C.E.) to locate and talk with your destination planet once you get to their approximate coordinates.";
				return reminder;
		}
	
		void setPlanetCoordinates ()
		{
				planetCoordinates [0] = new Vector2 (-178f, -131f);
				planetCoordinates [1] = new Vector2 (29f, 12f);
				planetCoordinates [2] = new Vector2 (124f, -77f);
				planetCoordinates [3] = new Vector2 (-171f, 98f);
				planetCoordinates [4] = new Vector2 (-14f, 11f);
				planetCoordinates [5] = new Vector2 (0, 0);
		
		}

		void OnTriggerEnter2D (Collider2D other)
		{
				if (other.gameObject.tag == getPlanetString ()) {
						planetTimer = seconds - delay;
						discoverable = true;
				}
		
				if (other.gameObject.tag == "imperial") {
						imperialTimer = seconds - delay;
						isImperialCuntFace = true;
				}
		
				if (other.gameObject.tag == "bark") {
						timer = seconds - delay;
						isBark = true;
				}
		
		}
	
		public string getMissionString ()
		{
				if (nextIdx == mission1.Length) {
						nextIdx = 0;
				}
				return mission1 [nextIdx];
		}
	
		public string getPlanetString ()
		{
				if (planetIdx == planetNames.Length) {
						planetIdx = 0;
				}
				return planetNames [planetIdx];
		}
	
		public string getPlanetCoordinate ()
		{
				if (planetIdx == planetCoordinates.Length) {
						planetIdx = 0;
				}
				Vector2 coord = planetCoordinates [planetIdx];
				return coord.x.ToString () + " , " + coord.y.ToString ();
		
		}
	
		bool IsOutOfbounds ()
		{
				if (transform.position.x < minX || transform.position.x > maxX) {
						return true;
				}
		
				if (transform.position.y < minY || transform.position.y > maxY) {
						return true;
				}
				return false;
		
		}
	
}
