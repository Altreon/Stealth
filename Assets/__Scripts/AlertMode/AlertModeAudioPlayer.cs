using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeAudioPlayer : MonoBehaviour {
	AudioSource audioSource;

	void Awake () {
		audioSource = GetComponent<AudioSource> ();

		AlertModeManager.alertModeStatusChangeDelegate += Alarm;
	}

	void OnDestroy () {
		AlertModeManager.alertModeStatusChangeDelegate -= Alarm;
	}

	void Alarm (bool alert) {
		if (alert) {
			audioSource.Play ();
		} else {
			audioSource.Stop ();
		}
	}
}
