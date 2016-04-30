using UnityEngine;
using System.Collections;

public class PlayerAudioAI : MonoBehaviour {

	private NavMeshAgent navAgent;
	private AudioSource audioSource;
	private Vector3 previousPosition;
	private float lastSoundPlayedTime;

	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 curMove = transform.position - previousPosition;
		float curSpeed = curMove.magnitude / Time.deltaTime;
		previousPosition = transform.position;
		float delta = Time.time - lastSoundPlayedTime;
		if (curSpeed > 0.0f && !audioSource.isPlaying && delta > 0.1f) {
			audioSource.Play ();
			lastSoundPlayedTime = Time.time;
		} else {
			audioSource.Stop ();
		}
	}
}
