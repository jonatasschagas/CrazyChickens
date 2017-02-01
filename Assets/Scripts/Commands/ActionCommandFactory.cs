using UnityEngine;
using System.Collections;

/// <summary>
/// Action command factory.
/// 
/// This class is responsible for creating "action" commands such as:
///  - connect
///  - pause
///  - disconnect
/// 
/// </summary>
public class ActionCommandFactory {

	/// <summary>
	/// Creates a "connection" command. The connection command creates a Session ID.
	/// This session ID is utilized to identify the player.
	/// </summary>
	/// <returns>The command.</returns>
	public static GameCommand CreateConnectionCommand() {

		GameCommand connectionCommand = new GameCommand ();
		connectionCommand.CommandType = CommandNames.CONNECT;
		connectionCommand.ExecutionTime = (long) Time.time;

		JSONObject commandJSON = new JSONObject(JSONObject.Type.OBJECT);
		commandJSON.AddField ("sessionId", GameManager.SESSION_ID);
		connectionCommand.CommandJSON = commandJSON.ToString ();

		return connectionCommand;
	}

}
