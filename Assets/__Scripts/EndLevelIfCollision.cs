using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelIfCollision : MonoBehaviour {
	[SerializeField]
	bool resetLevel = false;
	[SerializeField]
	bool startFirstLevel = false;

	void OnTriggerEnter (Collider collider){
		if (collider.tag == "Player") {
			if (resetLevel) {
				GameManager.Instance.ReloadLevel ();
			} else {
				if (startFirstLevel) {
					GameManager.Instance.RestartFromFirstLevel ();
				} else {
					GameManager.Instance.EndLevel ();
				}
			}
		}
	}
}