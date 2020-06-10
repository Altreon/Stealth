using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFootStepCollider : MonoBehaviour {
	AudioSource audioSource;

	void Awake () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.layer != LayerMask.NameToLayer("Ground")) {
			return;
		}

		AudioGroundSound groundSound = collider.GetComponent<AudioGroundSound> ();
		if (!groundSound) {
			return;
		}

		audioSource.PlayOneShot (groundSound.GetSound ());
	}
}
