using UnityEngine;
using System.Collections;

public class Coordinates : MonoBehaviour
{
		public static int currentCoordinateX;
		public static int currentCoordinateY;
		public Vector3 truckPosition = Vector3.zero;

	
		void Update ()
		{
				ShipController truck = GameObject.FindObjectOfType<ShipController> ();
				Ray ray = Camera.main.ScreenPointToRay (truck.transform.position);
				RaycastHit hitInfo;
				if (collider.Raycast (ray, out hitInfo, Mathf.Infinity)) {
						truckPosition = hitInfo.point;
						currentCoordinateX = Mathf.FloorToInt (hitInfo.point.x);
						currentCoordinateY = Mathf.FloorToInt (hitInfo.point.y);
				}
	
		}
}
