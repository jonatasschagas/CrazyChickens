using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Manages the scores, UI and the respawns
/// </summary>
public class GameManager : MonoBehaviour, IListener {

	public MapGenerator map;

	public Text playerScoreLabel;
	public Image bombIcon;
	public float playerRespawnTime = 5.0f;

	public GameObject pauseButton;
	public GameObject resumeButton;

	public GameObject bombPrefab;
	public GameObject playerPrefab;
	public GameObject opponentPrefab;

	public GameObject player;
	public GameObject opponent;

	private int playerScore = 0;
	private int opponentScore = 0;
	private float timePlayerDied = 0.0f;
	private float timeOpponentDied = 0.0f;
	private bool playerDead = false;
	private bool opponentDead = false;
	private Vector3 playerRespawnPosition;
	private Vector3 opponentRespawnPosition;
	private AudioSource audioSource;

	void Start () {
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_DIED, this);	
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_CAN_DROP_BOMB, this);	
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_DEPLOYED, this);	
		playerRespawnPosition = player.transform.position;
		opponentRespawnPosition = opponent.transform.position;
		audioSource = GetComponent<AudioSource> ();
		audioSource.Play ();
		playerScoreLabel.text = "You: " + playerScore + " | Opponent: " + opponentScore;
	}
	
	void Update () {
		
		if (playerDead) {
			float deltaPlayerDead = Time.time - timePlayerDied;
			if (deltaPlayerDead > playerRespawnTime) {
				Destroy (player);
				player = Instantiate (playerPrefab, playerRespawnPosition, Quaternion.identity) as GameObject;
				playerDead = false;
			}
		}

		if (opponentDead) {
			float deltaOpponentDead = Time.time - timeOpponentDied;
			if (deltaOpponentDead > playerRespawnTime) {
				Destroy (opponent);
				opponent = Instantiate (opponentPrefab, opponentRespawnPosition, Quaternion.identity) as GameObject;
				opponentDead = false;
			}
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
			if (playerWhoDied != null && playerWhoDied.GetInstanceID () == player.GetInstanceID ()) {
				opponentScore++;
				playerDead = true;
				timePlayerDied = Time.time;
				playerScoreLabel.text = "You: " + playerScore + " | Opponent: " + opponentScore;
			} else if(playerWhoDied != null && playerWhoDied.GetInstanceID () == opponent.GetInstanceID ()) {
				playerScore++;
				opponentDead = true;
				timeOpponentDied = Time.time;
				playerScoreLabel.text = "You: " + playerScore + " | Opponent: " + opponentScore;
			}
			break;
		case EVENT_TYPE.PLAYER_CAN_DROP_BOMB:
			bombIcon.enabled = true;
			break;
		case EVENT_TYPE.BOMB_DEPLOYED:
			bombIcon.enabled = false;
			break;
		}
	}

	public MapGenerator GetMapGenerator() {
		return map;
	}

	public GameObject GetBombPrefab() {
		return bombPrefab;
	}

	public void OnPauseBtnClicked() {
		EventManager.Instance.PostNotification (EVENT_TYPE.GAME_PAUSED, this, this);
		pauseButton.active = false;
		resumeButton.active = true;
	}

	public void OnResumeBtnClicked() {
		EventManager.Instance.PostNotification (EVENT_TYPE.GAME_STARTED, this, this);
		pauseButton.active = true;
		resumeButton.active = false;
	}

}
