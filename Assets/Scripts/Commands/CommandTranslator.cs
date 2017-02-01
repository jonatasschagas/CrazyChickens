using UnityEngine;
using System.Collections;

/// <summary>
/// Command translator.
/// 
/// Translates server commands into Events (vice-versa)
/// 
/// </summary>
public class CommandTranslator {

	/// <summary>
	/// Translates the event to a game command.
	/// </summary>
	/// <returns>The event to game command.</returns>
	/// <param name="eventType">Event type.</param>
	/// <param name="sender">Sender.</param>
	/// <param name="param">Parameter.</param>
	public static GameCommand TranslateEventToGameCommand(EVENT_TYPE eventType, Component sender, object param = null) {
		GameCommand gameCommand = null;
		switch (eventType) {
		case EVENT_TYPE.CONNECT:
			gameCommand = ActionCommandFactory.CreateConnectionCommand ();
			break;
		};
		return gameCommand;
	}

	/// <summary>
	/// Translates the game command to an event and dispatches it to the game objects.
	/// </summary>
	/// <param name="gameCommand">Game command.</param>
	public static void TranslateGameCommandToEventAndDispatch(GameCommand gameCommand) {
		string commandType = gameCommand.CommandType;
		JSONObject commandJson = new JSONObject (gameCommand.CommandJSON);
		if (commandType == CommandNames.CONNECT) {
			string sessionId = commandJson.GetField ("sessionId").str;
			EventManager.Instance.PostNotification (EVENT_TYPE.PLAYER_CONNECTED, null, sessionId);
		} else if (commandType == CommandNames.START_GAME) {
			EventManager.Instance.PostNotification (EVENT_TYPE.START_GAME, null, commandJson);
		}
	}

}
