using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{

		private float time = 0.0f;
		private int seconds = 0;
		public GUISkin skin;
		private float width = Screen.width / 2 - 124;
		private float height = Screen.height / 2 + 40;


	
		void Update ()
		{
				time -= Time.deltaTime;
				seconds = Mathf.FloorToInt (time);

				if (Input.GetKey (KeyCode.Return)) {
						Application.LoadLevel (1);
				}
		}

		void OnGUI ()
		{
				GUI.skin = skin;
				GUIStyle splash1 = skin.GetStyle ("splash1");
				GUIStyle splash2 = skin.GetStyle ("splash2");
				if (seconds % 2 == 0) {
						GUI.Box (new Rect (width, height, 248, 64), "", splash1);
				} else {
						GUI.Box (new Rect (width, height, 248, 64), "", splash2);
				}
				
		}
}
