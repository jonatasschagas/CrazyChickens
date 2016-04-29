//Script that allows the bomb to be thrown

//Ashley Godbold
//Last Updated 12/1/2014

using UnityEngine;
using System.Collections;

public class BombThrow : MonoBehaviour {
	public Rigidbody bomb; 						//our prefab
	public float throwPower = 10; 				//power variable determines how fast this object will be "shot out"
	
	public float timeBetweenBombs = .5f; 		//The time between throws so that the bomb will not be thrown quicky
	float timer;                                // A timer to determine when to fire.

	

	void  Update (){
		timer += Time.deltaTime;

		//Throws Bomb on Fire
		if (Input.GetButtonDown("Fire1") && timer >= timeBetweenBombs) {
			Throw ();
		}
	
	}
	
	void Throw(){
		timer=0f;
		//Clone the prefab
		Rigidbody clone; 
		clone = Instantiate(bomb, transform.position, transform.rotation) as Rigidbody;

		//apply a force to the prefab so that it is thrown forward from the point of throw
		clone.velocity = transform.TransformDirection(Vector3.forward * throwPower); 
		
	}
	
	
}