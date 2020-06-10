using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameManagerState {
	idle,
	preLevel,
	level,
	postLevel
}

public class GameManager : MonoBehaviour {
	static GameManager instance;

	[SerializeField]
	string[] levelNames;

	[SerializeField]
	UnityEvent levelStartEvent;
	[SerializeField]
	UnityEvent levelEndEvent;

	int currentLevel;
	public GameManagerState state = GameManagerState.postLevel;

	public static GameManager Instance {
		get {
			return instance;
		}
		set {
			if (instance != null) {
				return;
			}
			instance = value;
		}
	}

	public int CurrentLevel {
		get {
			return currentLevel;
		}
	}

	void Start () {
		Instance = this;
		currentLevel = 0;

		LevelAdvancePanel.FadeInToEndLevel (LoadLevel);
	}
	
	void LoadLevel () {
		LoadLevel (-1);
	}

	void LoadLevel (int num) {
		if (num < 0) {
			num = currentLevel;
		}

		currentLevel = num;

		StartCoroutine (LoadScene (currentLevel));
	}

	IEnumerator LoadScene (int scene){
		if (SceneManager.sceneCount > 1) {
			yield return SceneManager.UnloadSceneAsync (SceneManager.GetActiveScene ().buildIndex);
		}

		InteractingPlayer.SetPosition (new Vector3 (10000, 10000, 10000), Quaternion.identity);

		yield return SceneManager.LoadSceneAsync (levelNames[scene], LoadSceneMode.Additive);

		SceneManager.SetActiveScene(SceneManager.GetSceneAt(SceneManager.sceneCount - 1));

		state = GameManagerState.preLevel;
		LevelAdvancePanel.FadeOutToBeginLevel (StartLevel);
	}

	void StartLevel () {
		GameObject startPos = GameObject.Find("StartPosition");
		if (startPos == null) {
			return;
		}

		InteractingPlayer.SetPosition (startPos.transform.position, startPos.transform.rotation);
		StealthPlayerCamera.ResetToFarPosition ();

		state = GameManagerState.level;

		levelStartEvent.Invoke ();
	}

	public void ReloadLevel () {
		if(state == GameManagerState.postLevel) return;

		--currentLevel;
		EndLevel ();
	}

	public void RestartFromFirstLevel () {
		if(state == GameManagerState.postLevel) return;

		currentLevel = -1;
		EndLevel ();
	}

	public void EndLevel () {
		if(state == GameManagerState.postLevel) return;

		AlertModeManager.SwitchToAlertMode (false);

		state = GameManagerState.postLevel;
		++currentLevel;

		LevelAdvancePanel.FadeInToEndLevel (LoadLevel);

		levelEndEvent.Invoke ();
	}
}
