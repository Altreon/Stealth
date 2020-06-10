using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioFootstepSounds.asset", menuName = "ScriptableObjects/AudioFootstepSoundsScriptableObject", order = 0)]
[System.Serializable]
public class AudioFootstepSoundsScriptableObject : ScriptableObject {
	public AudioClip[] audioClips;

	int lastClip = -1;

	public AudioClip GetRandomClip () {
		if (audioClips == null || audioClips.Length == 0) {
			return null;
		}

		int num = -1;
		do {
			num = Random.Range (0, audioClips.Length);
		} while (num == lastClip);

		return audioClips [num];
	}
}
