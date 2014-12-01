using UnityEngine;
using System.Collections;

public class OffsetMaterial : MonoBehaviour
{
		float scrollSpeed = 10.0f;

		void Update ()
		{
				float offset = Time.time * scrollSpeed;
				renderer.material.SetTextureOffset ("BIG_SPACE", new Vector2 (offset, 0));
		}
}
