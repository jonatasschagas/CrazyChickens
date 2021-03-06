﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the player's artificial inteligence. It does the pathfinding, listens to the commands and
/// carries out the instructions
/// </summary>
public class PlayerAI : MonoBehaviour, IListener {

	public float bombCooldownTime = 2.0f;

	private MapGenerator map;
	private GameObject bombPrefab;

	private Vector3 prevDirection = Vector3.zero;
	private Vector3 moveDirection = Vector3.zero;
	private UnityEngine.AI.NavMeshAgent navAgent;
	private Animator animator;
	private List<Vector3> currentPath;
	private int currentPathIndex;
	private bool dead = false;
	private bool canDropBomb;
	private float lastTimeDroppedABomb;

	void Start() {
		GameManager gameManager = GameObject.FindObjectOfType<GameManager> ();
		map = gameManager.GetMapGenerator ();
		bombPrefab = gameManager.GetBombPrefab();
		// initializing vars
		moveDirection = Vector3.left;
		prevDirection = moveDirection;
		navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent> ();
		navAgent.SetDestination (transform.position);
		animator = gameObject.GetComponent<Animator> ();
		dead = false;
		canDropBomb = true;
		// registering for the events
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_EXPLODED, this);
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_DEPLOY_COMMAND, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_UP, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_DOWN, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_LEFT, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_RIGHT, this);
	}

	void Update() {

		if (PauseResumeAI.Instance.IsGamePaused ()) {
			Pause ();
			return;
		} else {
			Resume ();
		}

		if (dead) {
			return;
		}
		Move ();
		CheckBombCooldown ();
	}

	void Pause() {
		navAgent.velocity = Vector3.zero;
		animator.SetFloat ("Speed_f", 0.0f);
		navAgent.Stop ();
	}

	void Resume() {
			navAgent.Resume ();
	}

	void CheckBombCooldown() {
		if (!canDropBomb) {
			float delta = Time.time - lastTimeDroppedABomb;
			if (delta > bombCooldownTime) {
				canDropBomb = true;
				EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_CAN_DROP_BOMB, this, gameObject);
			}
		}
	}

	/// <summary>
	/// Moves the Player. If the target has been reached, it recalculates the default 
	/// path
	/// </summary>
	void Move() {
		if (navAgent.remainingDistance < 2.0f) {
			currentPathIndex++;
			if (currentPath != null && currentPathIndex < currentPath.Count) {
				// go to the next node from the path
				navAgent.SetDestination (currentPath [currentPathIndex]);
			} else {
				// find a new path
				prevDirection = moveDirection;
				if (moveDirection == Vector3.forward) {
					moveDirection = Vector3.right;
				} else if (moveDirection == Vector3.back) {
					moveDirection = Vector3.left;
				} else if (moveDirection == Vector3.left) {
					moveDirection = Vector3.forward;
				} else if (moveDirection == Vector3.right) {
					moveDirection = Vector3.back;
				}
				UpdateDirection ();
			}
		}
		animator.SetFloat ("Speed_f", navAgent.velocity.magnitude/10);
	}

	/// <summary>
	/// Generates the new path for player and starts the new path
	/// </summary>
	void UpdateDirection() {
		currentPath = map.GetNextPath (transform.position , prevDirection, moveDirection);
		currentPathIndex = 0;
		if (currentPath != null && currentPath.Count > 0) {
			navAgent.SetDestination (currentPath[currentPathIndex]);
		}
	}

	/// <summary>
	/// Creates the bomb and sends the command that a bomb was deployed
	/// </summary>
	public void DeployBomb() {
		Vector3 position = map.GetTileAlignedPosition (transform.position);
		GameObject bomb = Instantiate (bombPrefab, position, bombPrefab.transform.rotation) as GameObject;
		EventManager.Instance.PostNotification (EVENT_TYPE.BOMB_DEPLOYED, this, bomb); 
	}

	/// <summary>
	/// Draws in the scene view the path finding 
	/// </summary>
	void OnDrawGizmos() {
		if (currentPath != null && currentPath.Count > 0) {
			Gizmos.color = Color.yellow;
			foreach (Vector3 point in currentPath) {
				Gizmos.DrawCube (point, Vector3.one * 4);
			}
			Gizmos.color = Color.red;
			Gizmos.DrawCube (currentPath[currentPathIndex], Vector3.one * 4);
		}

	}

	/// <summary>
	/// Handles the events
	/// </summary>
	public void OnEvent(EVENT_TYPE eventType, Component sender, System.Object param = null) {
		GameObject player = null;
		switch (eventType) {
		case EVENT_TYPE.BOMB_EXPLODED:
			// verifying if player is in the tiles affected by the explosion
			if (BombAI.IsWithinExplosionRange(sender.gameObject.transform.position, transform.position, map)) {
				// if hit by the bomb the player dies
				animator.SetFloat ("Speed_f", 0);
				animator.SetBool ("Death_b", true);
				dead = true;
				EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_DIED, this, gameObject);
			}
			break;
		case EVENT_TYPE.PLAYER_TURN_UP:
			player = (GameObject)param;
			if (player != null && player.GetInstanceID () == gameObject.GetInstanceID ()) {
				prevDirection = moveDirection;
				moveDirection = Vector3.forward;
				UpdateDirection ();
			}
			break;
		case EVENT_TYPE.PLAYER_TURN_DOWN:
			player = (GameObject) param;
			if (player != null && player.GetInstanceID () == gameObject.GetInstanceID ()) {
				prevDirection = moveDirection;
				moveDirection = Vector3.back;
				UpdateDirection ();
			}
			break;
		case EVENT_TYPE.PLAYER_TURN_LEFT:
			player = (GameObject)param;
			if (player != null && player.GetInstanceID () == gameObject.GetInstanceID ()) {
				prevDirection = moveDirection;
				moveDirection = Vector3.left;
				UpdateDirection ();
			}
			break;
		case EVENT_TYPE.PLAYER_TURN_RIGHT:
			player = (GameObject)param;
			if (player != null && player.GetInstanceID () == gameObject.GetInstanceID ()) {
				prevDirection = moveDirection;
				moveDirection = Vector3.right;
				UpdateDirection ();
			}
			break;
		case EVENT_TYPE.BOMB_DEPLOY_COMMAND:
			player = (GameObject)param;
			if (player != null && player.GetInstanceID () == gameObject.GetInstanceID () && canDropBomb) {
				DeployBomb ();
				lastTimeDroppedABomb = Time.time;
				canDropBomb = false;
			}
			break;
		}

	}


}
