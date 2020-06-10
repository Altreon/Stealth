using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeAmbiantLightModifier : MonoBehaviour {
	[SerializeField]
	Color colorToSwitchTo;

	Color originalColor;

	void Awake () {
		originalColor = RenderSettings.ambientLight;

		AlertModeManager.alertModeStatusChangeDelegate += ChangeAmbiantLight;
	}

	void OnDestroy () {
		AlertModeManager.alertModeStatusChangeDelegate -= ChangeAmbiantLight;
	}

	void ChangeAmbiantLight (bool alert) {
		if (alert) {
			RenderSettings.ambientLight = colorToSwitchTo;
		} else {
			RenderSettings.ambientLight = originalColor;
		}
	}
}
