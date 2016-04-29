using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the Opponent's behaviour
/// </summary>
public class OpponentAI : MonoBehaviour, IListener {

	public float timeToChangeDirection = 2.0f;
	public float timeToDeployBomb = 1.0f;

	private bool dead = false;
	private float lastTimeChangedDirection = 0.0f;
	private float lastTimeDeployedBomb = 0.0f;

	void Start() {
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_DIED, this);
	}

	void Update() {

		if (dead) {
			return;
		}

		float deltaChangeDirection = Time.time - lastTimeChangedDirection;
		float deltaDeployBomb = Time.time - lastTimeDeployedBomb;

		if (deltaChangeDirection > timeToChangeDirection) {
			// generate random direction
			int random = Random.Range (0,3);
			switch ( random ) {
			case 0:
				EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_UP, this, gameObject);
				break;
			case 1:
				EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_DOWN, this, gameObject);
				break;
			case 2:
				EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_LEFT, this, gameObject);
				break;
			case 3:
				EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_RIGHT, this, gameObject);
				break;
			}
			lastTimeChangedDirection = Time.time;
		} else if(deltaDeployBomb > timeToDeployBomb) {
			// deploys the bomb
			EventManager.Instance.PostNotification (EVENT_TYPE.BOMB_DEPLOY_COMMAND, this, gameObject);
			lastTimeDeployedBomb = Time.time;
		}

	}

	/// <summary>
	/// Handles the events
	/// </summary>
	public void OnEvent(EVENT_TYPE eventType, Component sender, System.Object param = null) {
		GameObject playerWhoDied = null;
		switch (eventType) {
		case EVENT_TYPE.PLAYER_DIED:
			playerWhoDied = (GameObject)param;
			if(playerWhoDied != null && playerWhoDied.GetInstanceID () == gameObject.GetInstanceID ()) {
				dead = true;
			}
			break;
		}
	}

}
