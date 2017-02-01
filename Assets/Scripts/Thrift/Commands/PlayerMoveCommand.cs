using UnityEngine;
using System.Collections;

public class PlayerMoveCommand {

	private string _direction;
	private string _clientId;

	public string Direction 
	{
		get { return _direction; }
		set { _direction = value;}
	}

	public string ClientId 
	{
		get { return _clientId; }
		set { _clientId = value;}
	}

	public static PlayerMoveCommand Deserialize(string json) {
		JSONObject jsonObject = new JSONObject(json);
		PlayerMoveCommand playerMoveCommand = new PlayerMoveCommand ();
		playerMoveCommand.ClientId = jsonObject.GetField ("clientId").str;
		playerMoveCommand.Direction = jsonObject.GetField ("direction").str;
		return playerMoveCommand;
	}

	public string Serialize() {
		JSONObject jsonObject = new JSONObject();
		jsonObject.AddField ("clientId", ClientId);
		jsonObject.AddField ("direction", Direction);
		return jsonObject.Print ();
	}

}
