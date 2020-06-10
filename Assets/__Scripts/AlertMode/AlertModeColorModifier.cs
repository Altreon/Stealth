using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeColorModifier : MonoBehaviour {
	[SerializeField]
	int materialElementNumber;
	[SerializeField]
	string colorName;
	[SerializeField]
	Color colorToSwitchTo;

	Color originalColor;

	Material material;

	void Awake () {
		material = GetComponent<Renderer> ().materials [materialElementNumber];
		originalColor = material.GetColor (colorName);

		AlertModeManager.alertModeStatusChangeDelegate += ChangeColor;
	}

	void OnDestroy () {
		AlertModeManager.alertModeStatusChangeDelegate -= ChangeColor;
	}

	void ChangeColor (bool alert) {
		if (alert) {
			material.SetColor (colorName, colorToSwitchTo);
		} else {
			material.SetColor (colorName, originalColor);
		}
	}
}
