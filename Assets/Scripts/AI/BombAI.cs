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
	public GameObject fireAnimationPrefab;
	public float tileSize;
	public static int numberOfExplosions = 6;

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
		}
	}

	void CreateExplosions() {
		Vector3 bombPosition = transform.position;
		explosions.Add (Instantiate (explosionAnimationPrefab, transform.position , explosionAnimationPrefab.transform.rotation) as GameObject);
		bool betweenWallsHorizontal = map.IsBetweenWallsHorizontal (bombPosition);
		bool betweenWallsVertical = map.IsBetweenWallsVertical (bombPosition);

		// creating fire in the horizontal
		if (!betweenWallsHorizontal) {
			Vector3 tileLeft = map.GetPositionNeightborTile (bombPosition, MapGenerator.TileDirection.TILE_LEFT);
			Vector3 tileRight = map.GetPositionNeightborTile (bombPosition, MapGenerator.TileDirection.TILE_RIGHT);
			if (tileLeft != Vector3.zero) {
				GameObject fireTileLeft = Instantiate (fireAnimationPrefab, tileLeft, fireAnimationPrefab.transform.rotation) as GameObject;
				fireTileLeft.transform.Rotate (0,-90,0);
				explosions.Add (fireTileLeft);
			}
			if (tileRight != Vector3.zero) {
				GameObject fireTileRight = Instantiate (fireAnimationPrefab, tileRight, fireAnimationPrefab.transform.rotation) as GameObject;
				fireTileRight.transform.Rotate (0,90,0);
				explosions.Add (fireTileRight);
			}
		}

		// creating fire in the vertical
		if (!betweenWallsVertical) {
			Vector3 tileTop = map.GetPositionNeightborTile (bombPosition, MapGenerator.TileDirection.TILE_UP);
			Vector3 tileDown = map.GetPositionNeightborTile (bombPosition, MapGenerator.TileDirection.TILE_DOWN);
			if (tileTop != Vector3.zero) {
				GameObject fireTileTop = Instantiate (fireAnimationPrefab, tileTop, fireAnimationPrefab.transform.rotation) as GameObject;
				explosions.Add (fireTileTop);
			}
			if (tileDown != Vector3.zero) {
				GameObject fireTileDown = Instantiate (fireAnimationPrefab, tileDown, fireAnimationPrefab.transform.rotation) as GameObject;
				fireTileDown.transform.Rotate (0,180,0);
				explosions.Add (fireTileDown);
			}
		}
		EventManager.Instance.PostNotification (EVENT_TYPE.BOMB_EXPLODED, this, numberOfExplosions);
		detonated = true;
		timeCreation = Time.time;
		// hiding bomb
		MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
		render.enabled = false;
		StartCoroutine(DestroyExplosionsAndBomb(timeToExplode));
	}

	IEnumerator DestroyExplosionsAndBomb(float timeExplosion) {
		yield return new WaitForSeconds(timeExplosion);
		for (int i = 0; i < explosions.Count; i++) {
			Destroy (explosions[i]);
		}
		Destroy (gameObject);
	}

	/// <summary>
	/// Determines whether the given position is within the range of explosion of the bomb
	/// </summary>
	/// <returns><c>true</c> if this instance is within range the specified tilePos; otherwise, <c>false</c>.</returns>
	/// <param name="tilePos">Tile position.</param>
	public static bool IsWithinExplosionRange(Vector3 bombPos, Vector3 tilePos, MapGenerator map) {
		Coord bombCoord = map.PositionToCoord (bombPos);
		Coord tileCoord = map.PositionToCoord (tilePos);
		bool betweenWallsHorizontal = map.IsBetweenWallsHorizontal (bombPos);
		bool betweenWallsVertical = map.IsBetweenWallsVertical (bombPos);
		int numberOfTilesInEachSide = numberOfExplosions / 2;
		return tileCoord.tileType == TILE_TYPE.GROUND && 
			((tileCoord.x >= (bombCoord.x - numberOfTilesInEachSide) && tileCoord.x <= (bombCoord.x + numberOfTilesInEachSide) && bombCoord.y == tileCoord.y && !betweenWallsHorizontal)
				|| (tileCoord.y >= (bombCoord.y - numberOfTilesInEachSide) && tileCoord.y <= (bombCoord.y + numberOfTilesInEachSide) && bombCoord.x == tileCoord.x && !betweenWallsVertical));
	}

}
