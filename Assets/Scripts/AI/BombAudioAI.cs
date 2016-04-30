using UnityEngine;
using System.Collections;

public class BombAudioAI : MonoBehaviour, IListener {

	public AudioClip fuseSound;
	public AudioClip explosionSound;

	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_DEPLOYED, this);
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_EXPLODED, this);
	}
	
	/// <summary>
	/// Handles the events
	/// </summary>
	public void OnEvent(EVENT_TYPE eventType, Component sender, System.Object param = null) {
		switch (eventType) {
		case EVENT_TYPE.BOMB_EXPLODED:
			sender.gameObject.GetComponent<BombAudioAI> ().PlayExplosion();
			break;
		case EVENT_TYPE.BOMB_DEPLOYED:
			GameObject bomb = (GameObject)param;
			if (bomb != null) {
				bomb.GetComponent<BombAudioAI> ().PlayFuseBurning ();
			}
			break;
		}

	}

	public void PlayExplosion() {
		audioSource.Stop ();
		audioSource.clip = explosionSound;
		audioSource.Play ();
	}

	public void PlayFuseBurning() {
		if (audioSource == null) {
			audioSource = GetComponent<AudioSource> ();
		}
		audioSource.Stop ();
		audioSource.clip = fuseSound;
		audioSource.Play ();
	}

}
