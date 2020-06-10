using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour {
	[SerializeField]
	float pauseTime;

	public Vector3 Position {
		get {
			return transform.position;
		}
	}

	public Vector3 Forward {
		get {
			return transform.forward;
		}
	}

	public Quaternion Rotation {
		get {
			return transform.rotation;
		}
	}

	public float PauseTime {
		get {
			return pauseTime;
		}
	}
}
