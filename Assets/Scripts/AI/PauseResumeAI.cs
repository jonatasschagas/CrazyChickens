using UnityEngine;
using System.Collections;

/// <summary>
/// Reacts to the PAUSE - RESUME game events
/// </summary>
public class PauseResumeAI : MonoBehaviour, IListener {

	public static PauseResumeAI Instance {
		get { return _instance; }
		set { }
	}

	private bool pause = true;
	private static PauseResumeAI _instance = null;

	void Awake () {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (this);
		}
	}

	void Start() {
		EventManager.Instance.AddListener (EVENT_TYPE.GAME_PAUSED, this);
		EventManager.Instance.AddListener (EVENT_TYPE.GAME_STARTED, this);
	}

	public bool IsGamePaused() {
		return pause;
	}

	public void OnEvent(EVENT_TYPE EventType, Component sender, System.Object param = null) {
		switch (EventType) {
		case EVENT_TYPE.GAME_PAUSED:
			pause = true;
			break;
		case EVENT_TYPE.GAME_STARTED:
			pause = false;
			break;
		}
	}

}
