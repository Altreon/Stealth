using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ACS17AnimationManager : MonoBehaviour
{
	EnemyNav enemyNav;
	Animator anim;
	NavMode lastState;

	void Start () {
		enemyNav = transform.parent.GetComponent<EnemyNav> ();
		anim = GetComponent<Animator> ();

		lastState = NavMode.idle;
	}

	void Update () {
		NavMode state = enemyNav.mode;
		if (lastState == state) {
			return;
		}

		SwitchAnimation (state);

		lastState = state;
	}

	void SwitchAnimation (NavMode state){
		switch (state) {
			case NavMode.idle:
			case NavMode.wait:
				anim.CrossFade ("ACS_Idle", 0.25f);
				break;
			case NavMode.preMoveRot:
			case NavMode.postMoveRot:
				if (enemyNav.turnDir == -1) {
					anim.CrossFade ("ACS_TurnLeft", 0.1f);
				} else {
					anim.CrossFade ("ACS_TurnRight", 0.1f);
				}
				break;
			case NavMode.move:
			case NavMode.chase:
			case NavMode.stopChase:
				anim.CrossFade ("ACS_Walk", 0.25f);
				break;
		}
	}

}
