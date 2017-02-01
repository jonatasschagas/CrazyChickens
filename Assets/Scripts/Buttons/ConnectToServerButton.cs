using UnityEngine;
using System.Collections;

/// <summary>
/// Performs the actions required when the button connect to server is pressed
/// </summary>
public class ConnectToServerButton : MonoBehaviour {

	/// <summary>
	/// Sends the "connect" event.
	/// </summary>
	public void ConnectToServer() {
		EventManager.Instance.PostNotification (EVENT_TYPE.CONNECT, null, null);
	}

}
