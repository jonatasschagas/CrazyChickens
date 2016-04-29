using UnityEngine;
using System.Collections;

public class AdjustCamera : MonoBehaviour {

	public GameObject camera;

	public GameObject btnUp;
	public GameObject btnDown;
	public GameObject btnLeft;
	public GameObject btnRight;
	public GameObject btnZoomIn;
	public GameObject btnZoomOut;

	private bool hide = false;

	public void Up() {
		Vector3 pos = camera.transform.position;
		pos.z--;
		camera.transform.position = pos;
	}

	public void Down() {
		Vector3 pos = camera.transform.position;
		pos.z++;
		camera.transform.position = pos;
	}

	public void Left() {
		Vector3 pos = camera.transform.position;
		pos.x++;
		camera.transform.position = pos;
	}

	public void Right() {
		Vector3 pos = camera.transform.position;
		pos.x--;
		camera.transform.position = pos;
	}

	public void ZoomIn() {
		Vector3 pos = camera.transform.position;
		pos.y--;
		camera.transform.position = pos;
	}

	public void ZoomOut() {
		Vector3 pos = camera.transform.position;
		pos.y++;
		camera.transform.position = pos;
	}

	public void HideShowBtns() {
		if (!hide) {
			btnUp.active = false;
			btnDown.active = false;
			btnLeft.active = false;
			btnRight.active = false;
			btnZoomIn.active = false;
			btnZoomOut.active = false;
			hide = true;
		} else {
			btnUp.active = true;
			btnDown.active = true;
			btnLeft.active = true;
			btnRight.active = true;
			btnZoomIn.active = true;
			btnZoomOut.active = true;
			hide = false;
		}
	}

}
