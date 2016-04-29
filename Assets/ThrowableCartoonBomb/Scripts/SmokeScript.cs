//Script that removes the smoke prefab after a set amount of time

//Ashley Godbold
//Last updated 12/1/2014
using UnityEngine;
using System.Collections;

public class SmokeScript : MonoBehaviour {

public float lifetime =3f;			//Determines time until smoke is Destroyed

	// Use this for initialization
	void Update () {
		Destroy(gameObject, lifetime);

	}
}
