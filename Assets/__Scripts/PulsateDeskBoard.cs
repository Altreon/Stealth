using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsateDeskBoard : MonoBehaviour {
	[SerializeField]
	Renderer rend;
	[SerializeField]
	float fadeTime;
	[SerializeField]
	float boardSize;

	Material material;
	bool playerOnDesk = false;

	void Start () {
		material = rend.material;

		material.SetFloat ("_Border", 0);
	}

	void Update () {
		if (!playerOnDesk) {
			return;
		}

		float fadeValue = Mathf.Abs (Mathf.Sin ((Time.time / fadeTime) * Mathf.PI)) * boardSize;
		material.SetFloat ("_Border", fadeValue);
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag != "Player") {
			return;
		}

		playerOnDesk = true;
	}

	void OnTriggerExit(Collider collider)
	{
		if (collider.tag != "Player") {
			return;
		}

		playerOnDesk = false;

		material.SetFloat ("_Border", 0);
	}

	void OnDisable () {
		material.SetFloat ("_Border", 0);
	}
}
