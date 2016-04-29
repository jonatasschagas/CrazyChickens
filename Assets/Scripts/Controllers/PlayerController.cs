using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Reads the controls from the Player
/// </summary>
public class PlayerController : MonoBehaviour {

	private bool tapping;
	private float lastTap;
	private float tapTime = 0.5f;

	/// <summary>
	/// Detects if the player has tapped on the screen
	/// </summary>
	void TapDetection (){
		if(Input.GetKeyDown("f") || Input.GetMouseButtonDown(0)){
			if(!tapping){
				tapping = true;
				StartCoroutine(SingleTap());
			}
			if((Time.time - lastTap) < tapTime){
				EventManager.Instance.PostNotification (EVENT_TYPE.BOMB_DEPLOY_COMMAND, this, gameObject);
				tapping = false;
			}
			lastTap = Time.time;
		}
	}

	IEnumerator SingleTap() {
		yield return new WaitForSeconds(tapTime);
		if(tapping){
			tapping = false;
		}
	}

	void Start() {
		gameObject.GetComponent<SwipeControl> () 
			.SetMethodToCall (ReadCommands);
	}

	void Update() {
		TapDetection ();
	}

	void ReadCommands(SwipeControl.SWIPE_DIRECTION iDirection) {
		switch ( iDirection ) {
		case SwipeControl.SWIPE_DIRECTION.SD_UP:
			EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_UP, this, gameObject);
			break;
		case SwipeControl.SWIPE_DIRECTION.SD_DOWN:
			EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_DOWN, this, gameObject);
			break;
		case SwipeControl.SWIPE_DIRECTION.SD_LEFT:
			EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_LEFT, this, gameObject);
			break;
		case SwipeControl.SWIPE_DIRECTION.SD_RIGHT:
			EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_TURN_RIGHT, this, gameObject);
			break;
		}
	}

}
