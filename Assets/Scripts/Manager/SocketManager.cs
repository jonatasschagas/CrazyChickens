using UnityEngine;
using System.Collections;
using Thrift.Transport;
using Thrift.Protocol;

/// <summary>
/// Socket manager:
/// 
/// 1. Listen to events
/// 2. Assembles commands
/// 3. Sends commands to the server
/// 4. Receives commands from the server
/// 5. Propagates commands to the game based 
/// on the commands received by the server
/// 
/// </summary>
public class SocketManager : MonoBehaviour, IListener {

	private const float  SERVER_PULL_INTERVAL_SECONDS = 3; 
	private const string SERVER_HOST 				  = "localhost";
	private const int    SERVER_PORT 				  = 9090;

	private TTransport transport;
	private TProtocol protocol;
	private GameService.Client client;
	private GameManager gameManager;

	private float lastPullFromServer = 0;

	void Start () {
		RegisterAsEventListener ();
		ConnectToServer ();
	}
	
	void FixedUpdate () {
		// pull commands from the server
		float deltaPullFromServer = Time.time - lastPullFromServer;
		if (lastPullFromServer == 0 || deltaPullFromServer > SERVER_PULL_INTERVAL_SECONDS) {
			PullCommands ();
			lastPullFromServer = Time.time;
		}
	}

	void OnDestroy() {
		Debug.Log ("CLIENT AND SERVER DISCONNECTED");
		transport.Close ();
	}

	/// <summary>
	/// Registers as an event listener and declares to which kind of events it listens
	/// </summary>
	private void RegisterAsEventListener() {
		EventManager.Instance.AddListener (EVENT_TYPE.CONNECT, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_UP, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_DOWN, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_LEFT, this);
		EventManager.Instance.AddListener (EVENT_TYPE.PLAYER_TURN_RIGHT, this);
	}

	/// <summary>
	/// Connects to server.
	/// </summary>
	private void ConnectToServer() {
		// initializing socket connection to the server
		transport = new TSocket(SERVER_HOST, SERVER_PORT);
		protocol = new TBinaryProtocol(transport);
		client = new GameService.Client (protocol);
		transport.Open();
		Debug.Log ("CLIENT AND SERVER CONNECTED");
	}

	/// <summary>
	/// Pulls the commands from the server and propagates them to the game objects
	/// </summary>
	void PullCommands() {
		FlushCommandsRequest flushCommandsRequest = new FlushCommandsRequest ();
		flushCommandsRequest.ClientId = GameManager.SESSION_ID;
		FlushCommandsResponse flushCommandsResponse = client.flushCommands (flushCommandsRequest);
		foreach(GameCommand gameCommand in flushCommandsResponse.Commands) {
			PropagateCommand (gameCommand);
		}
	}

	/// <summary>
	/// Sends the command to the server.
	/// </summary>
	/// <param name="gameCommand">Game command.</param>
	void SendCommand(GameCommand gameCommand) {
		PublishCommandRequest publishCommandRequest = new PublishCommandRequest ();
		publishCommandRequest.ClientId = GameManager.SESSION_ID;
		publishCommandRequest.GameCommand = gameCommand;
		client.publishCommand (publishCommandRequest);
		Debug.Log ("CLIENT: Command " +  gameCommand.CommandType + " sent");
	}

	/// <summary>
	/// Converts the commands into Events. The events are then dispatched. The listeners from these
	/// events react to them.
	/// </summary>
	/// <param name="gameCommand">Game command.</param>
	void PropagateCommand(GameCommand gameCommand) {
		Debug.Log ("SERVER: Command " +  gameCommand.CommandType + " received");
		CommandTranslator.TranslateGameCommandToEventAndDispatch (gameCommand);
	}

	#region IListener implementation

	/// <summary>
	/// Receives the events and translates them into Game Commands.
	/// </summary>
	/// <param name="EventType">Event type.</param>
	/// <param name="sender">Sender.</param>
	/// <param name="param">Parameter.</param>
	public void OnEvent (EVENT_TYPE EventType, Component sender, object param = null) {
		GameCommand gameCommand = CommandTranslator.TranslateEventToGameCommand (EventType, sender, param);
		if (gameCommand != null) {
			SendCommand (gameCommand);
		}
	}

	#endregion



}
