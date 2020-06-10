using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction_DisableGameObject : PlayerAction {
	[SerializeField]
	GameObject gameObjectToDisable;
	

	public override void Action() {
		gameObjectToDisable.SetActive (false);
	}
}
