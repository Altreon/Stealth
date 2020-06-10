using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LightCone))]
public class PlayerInteractable_LightCone : PlayerInteractable {
	LightCone lightCone;

	void Start () {
		lightCone = GetComponent<LightCone> ();
	}

	void LateUpdate () {
		foreach (RaycastHit hit in lightCone.GetRaycastHits()){
			if (hit.transform && hit.transform.tag == "Player") {
				playerWithinTrigger = true;

				return;
			}
		}

		playerWithinTrigger = false;
	}
}
