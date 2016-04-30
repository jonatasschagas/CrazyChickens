using UnityEngine;
using System.Collections;

public class TileAI : MonoBehaviour, IListener {

	public GameObject tileBombRangePrefab;

	private MapGenerator map;
	private GameManager gameManager;

	void Start () {
		EventManager.Instance.AddListener (EVENT_TYPE.BOMB_DEPLOYED, this);
		gameManager = GameObject.FindObjectOfType<GameManager> () as GameManager;
		map = gameManager.GetMapGenerator ();
	}
	
	void Update () {
		
	}

	/// <summary>
	/// Handles the events
	/// </summary>
	public void OnEvent(EVENT_TYPE eventType, Component sender, System.Object param = null) {
		switch (eventType) {
		case EVENT_TYPE.BOMB_DEPLOYED:
			// verifies if player was hit by the bomb
			GameObject bomb = (GameObject)param;
			float timeToExplode = bomb.GetComponent<BombAI> ().timeToExplode;
			bool isWithinBombRange = BombAI.IsWithinExplosionRange (bomb.transform.position, transform.position, map);
			// verifying if player is in the tiles affected by the explosion
			if (isWithinBombRange) {
				Vector3 tilePos = transform.position;
				tilePos.y += 0.1f;
				// highlight tile that will be affected by the explosion
				StartCoroutine(ReturnTileToNormal(timeToExplode + 0.5f, Instantiate(tileBombRangePrefab, tilePos, transform.rotation) as GameObject));
			}
			break;
		}

	}

	IEnumerator ReturnTileToNormal(float waitTime, GameObject tileBombRange) {
		tileBombRange.transform.localScale = Vector3.one * map.tileSize;
		yield return new WaitForSeconds(waitTime);
		Destroy (tileBombRange);
	}

}
