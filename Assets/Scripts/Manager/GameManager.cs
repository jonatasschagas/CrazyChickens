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

	public GameObject spawnPosition1;
	public GameObject spawnPosition2;

	private int playerScore = 0;
	private int opponentScore = 0;
	private float timePlayerDied = 0.0f;
	private float timeOpponentDied = 0.0f;
	private bool playerDead = false;
	private bool opponentDead = false;
	private AudioSource audioSource;

	public static string SESSION_ID = System.Guid.NewGuid().ToString();

	void Start () {
		RegisterEventListeners ();
		InitializeAudio ();
		InitializeScore ();
	}
	
	void Update () {
		
		/*if (playerDead) {
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
		}*/

	}

	private void InitializeScore() {
		playerScore = 0;
		opponentScore = 0;
		playerScoreLabel.text = "You: " + playerScore + " | Enemy: " + opponentScore;
	}

	/// <summary>
	/// Initializes the audio.
	/// </summary>
	private void InitializeAudio() {
		audioSource = GetComponent<AudioSource> ();
		audioSource.Play ();
	}

	/// <summary>
	/// Registers the event listeners.
	/// </summary>
	private void RegisterEventListeners() {
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_CONNECTED, this);
		EventManager.Instance.AddListener (EVENT_TYPE.START_GAME, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_DIED, this);	
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_CAN_DROP_BOMB, this);	
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_DEPLOYED, this);	
	}

	/// <summary>
	/// Handles the events
	/// </summary>
	public void OnEvent(EVENT_TYPE eventType, Component sender, System.Object param = null) {
		switch (eventType) {
		case EVENT_TYPE.PLAYER_CONNECTED:
			string sessionId = (string)param;
			Debug.Log ("player " + sessionId + " is connected.");
			break;
		case EVENT_TYPE.START_GAME:
			JSONObject startGame = (JSONObject)param;

			string clientIdPos1 = startGame.GetField ("1").str;
			string clientIdPos2 = startGame.GetField ("2").str;

			if (clientIdPos1 == SESSION_ID) {

			} else {

			}

			break;
		case EVENT_TYPE.PLAYER_DIED:
			GameObject playerWhoDied = null;
			playerWhoDied = (GameObject)param;
			if (playerWhoDied != null && playerWhoDied.GetInstanceID () == player.GetInstanceID ()) {
				opponentScore++;
				playerDead = true;
				timePlayerDied = Time.time;
				playerScoreLabel.text = "You: " + playerScore + " | Enemy: " + opponentScore;
			} else if(playerWhoDied != null && playerWhoDied.GetInstanceID () == opponent.GetInstanceID ()) {
				playerScore++;
				opponentDead = true;
				timeOpponentDied = Time.time;
				playerScoreLabel.text = "You: " + playerScore + " | Enemy: " + opponentScore;
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

	private void CreatePlayer(Vector3 spawnPosition) {

	}

	private void CreateOpponent(Vector3 spawnPosition) {

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
