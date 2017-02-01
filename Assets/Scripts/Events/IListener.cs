using UnityEngine;
using System.Collections;

public enum EVENT_TYPE {
	CONNECT,
	PLAYER_CONNECTED,
	START_GAME,
	GAME_STARTED,
	GAME_PAUSED,
	BOMB_DEPLOY_COMMAND,
	BOMB_DEPLOYED,
	BOMB_EXPLODED,
	PLAYER_TURN_LEFT,
	PLAYER_TURN_RIGHT,
	PLAYER_TURN_UP,
	PLAYER_TURN_DOWN,
	PLAYER_DIED,
	PLAYER_RESPAWN,
	PLAYER_CAN_DROP_BOMB
};

public interface IListener {

	void OnEvent(EVENT_TYPE EventType, Component sender, System.Object param = null);

}
