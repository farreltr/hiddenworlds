using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{

		private float time = 0.0f;
		private int seconds = 0;
		public GUISkin skin;
		private float x = 234f;
		private float y = 244f;


	
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
						GUI.Box (new Rect (x, y, 132 * 1.3f, 36 * 1.3f), "", splash1);
				} else {
						GUI.Box (new Rect (x, y, 132 * 1.3f, 36 * 1.3f), "", splash2);
				}
				
		}
}
