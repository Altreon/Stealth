using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertModeSpriteModifier : MonoBehaviour {
	[SerializeField]
	Sprite spriteToSwitchTo;
	[SerializeField]
	Color colorToSwitchTo;
	[SerializeField]
	[Range(0, 1)]
	float colorBlend;

	SpriteRenderer[] spriteRenderers;
	Sprite[] originalSprites;
	Color[] originalColors;

	void Awake () {
		spriteRenderers = GetComponentsInChildren<SpriteRenderer> ();
		originalSprites = new Sprite[spriteRenderers.Length];
		originalColors = new Color[spriteRenderers.Length];
		for(int i = 0; i < spriteRenderers.Length; ++i){
			originalSprites [i] = spriteRenderers [i].sprite;
			originalColors [i] = spriteRenderers [i].color;
		}

		AlertModeManager.alertModeStatusChangeDelegate += ChangeSprite;
	}

	void OnDestroy () {
		AlertModeManager.alertModeStatusChangeDelegate -= ChangeSprite;
	}

	void ChangeSprite (bool alert) {
		for (int i = 0; i < spriteRenderers.Length; ++i) {
			spriteRenderers [i].sprite = alert ? spriteToSwitchTo : originalSprites [i];
			spriteRenderers [i].color = alert ? originalColors[i] * 0.3f + colorToSwitchTo * 0.7f : originalColors[i];
		}
	}
}
