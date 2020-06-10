using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeLineConeModifier : MonoBehaviour {
	[SerializeField]
	Color colorToSwitchTo;

	Color originalColor;

	LightCone lightCone;

	void Awake () {
		lightCone = GetComponent<LightCone> ();
		originalColor = lightCone.color;

		AlertModeManager.alertModeStatusChangeDelegate += ChangeColor;
	}

	void OnDestroy () {
		AlertModeManager.alertModeStatusChangeDelegate -= ChangeColor;
	}

	void ChangeColor (bool alert) {
		if (alert) {
			lightCone.color = colorToSwitchTo;
		} else {
			lightCone.color = originalColor;
		}
	}
}
