using UnityEngine;
using System.Collections;

public class PlayerCommandFactory {


	/// <summary>
	/// Creates the player move command and returns the GameCommand
	/// </summary>
	/// <returns>The player move command.</returns>
	/// <param name="eventType">Event type.</param>
	/// <param name="playerGameObject">Player game object.</param>
	private GameCommand CreatePlayerMoveCommand(EVENT_TYPE eventType, GameObject playerGameObject) {
		
		switch ( eventType ) {
		case EVENT_TYPE.PLAYER_TURN_UP:
			
			break;
		case EVENT_TYPE.PLAYER_TURN_DOWN:
			
			break;
		case EVENT_TYPE.PLAYER_TURN_LEFT:
			
			break;
		case EVENT_TYPE.PLAYER_TURN_RIGHT:
			
			break;
		}

		return null;
	}

}
