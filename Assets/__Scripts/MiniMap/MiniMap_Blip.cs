using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_Blip : MonoBehaviour {
	[HideInInspector]
	public Transform followed;
	[HideInInspector]
	public float height;
	
	void Update () {
		transform.position = followed.position + Vector3.up * height;
	}
}
