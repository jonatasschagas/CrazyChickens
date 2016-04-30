using UnityEngine;
using System.Collections;

public class PlayerAudioAI : MonoBehaviour, IListener {

	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_DIED, this);
	}
	
	/// <summary>
	/// Handles the events
	/// </summary>
	public void OnEvent(EVENT_TYPE eventType, Component sender, System.Object param = null) {
		GameObject player = null;
		switch (eventType) {
		case EVENT_TYPE.PLAYER_DIED:
			player = (GameObject)param;
			if (player != null && player.GetInstanceID () == gameObject.GetInstanceID ()) {
				audioSource.Play ();
			}
			break;
		}

	}

}
