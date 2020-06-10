using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGroundSound : MonoBehaviour {
	[SerializeField]
	AudioFootstepSoundsScriptableObject footsStepSounds;

	public AudioClip GetSound () {
		return footsStepSounds.GetRandomClip ();
	}
}
