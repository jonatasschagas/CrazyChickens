using UnityEngine;
using System.Collections;

public enum EVENT_TYPE {
	BOMB_DEPLOY_COMMAND,
	BOMB_DEPLOYED,
	BOMB_EXPLODED,
	PLAYER_TURN_LEFT,
	PLAYER_TURN_RIGHT,
	PLAYER_TURN_UP,
	PLAYER_TURN_DOWN,
	PLAYER_DIED,
	PLAYER_RESPAWN
};

public interface IListener {

	void OnEvent(EVENT_TYPE EventType, Component sender, System.Object param = null);

}
