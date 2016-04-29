using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Bomb's AI. Controls how the bomb behaves
/// </summary>
public class BombAI : MonoBehaviour {

	public float timeToExplode =  2.0f;
	public float timeExplosion =  3.0f;
	public GameObject explosionAnimationPrefab;
	public float tileSize;
	public int numberOfExplosions = 6;

	private MapGenerator map;
	private float timeCreation;
	private List<GameObject> explosions = new List<GameObject>();
	private bool detonated;

	void Start () {
		GameManager gm = GameObject.FindObjectOfType<GameManager> () as GameManager;
		map = gm.GetMapGenerator ();
		timeCreation = Time.time;
		detonated = false;
	}
	
	void Update () {
		if (!detonated) {
			float delta = Time.time - timeCreation;
			if (delta >= timeToExplode) {
				CreateExplosions ();
			}
		} else {
			DestroyExplosionsAndBomb ();
		}
	}

	void CreateExplosions() {
		Vector3 bombPosition = transform.position;
		bool betweenWallsHorizontal = map.IsBetweenWallsHorizontal (bombPosition);
		bool betweenWallsVertical = map.IsBetweenWallsVertical (bombPosition);

		float offset = tileSize * (numberOfExplosions / 2.0f);
		// creating explosions in the horizontal
		Vector3 groundZero = new Vector3(bombPosition.x - offset , bombPosition.y, bombPosition.z);
		if (!betweenWallsHorizontal) {
			for (int i = 0; i <= numberOfExplosions; i++) {
				explosions.Add (Instantiate (explosionAnimationPrefab, groundZero , explosionAnimationPrefab.transform.rotation) as GameObject);
				groundZero.x += tileSize;
				EventManager.Instance.PostNotification (EVENT_TYPE.BOMB_EXPLODED, this, groundZero);
			}
		}

		if (!betweenWallsVertical) {
			// creating explosions in the vertical
			groundZero = new Vector3(bombPosition.x , bombPosition.y, bombPosition.z - offset);
			for (int i = 0; i <= numberOfExplosions; i++) {
				explosions.Add (Instantiate (explosionAnimationPrefab, groundZero , explosionAnimationPrefab.transform.rotation) as GameObject);
				groundZero.z += tileSize;
				EventManager.Instance.PostNotification (EVENT_TYPE.BOMB_EXPLODED, this, groundZero);
			}
		}
		detonated = true;
		timeCreation = Time.time;
		// hiding bomb
		MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
		render.enabled = false;
	}

	void DestroyExplosionsAndBomb() {
		float delta = Time.time - timeCreation;
		if (delta >= timeExplosion) {
			for (int i = 0; i < numberOfExplosions; i++) {
				Destroy (explosions[i]);
			}
			Destroy (gameObject);
		}
	}

}
