//Script that removes the bomb from the scene and adds the smoke cloud
//Ashley Godbold
//last edited 12/1/2014

using UnityEngine;
using System.Collections;

public class BombExplode : MonoBehaviour {
	public float radius = 2.0f;   						//bomb blast radius
	public float explosiveDelay = 5.0f; 				//Delay between bomb throw and bomb explosion
	public GameObject smokePrefab;						//prefab for the smoke particles
	public AudioClip explosionSound;					//sound to play when bomb explodes
 
	
	void  Update (){
		
		StartCoroutine(Fire());
	}
	
	IEnumerator Fire (){
		//Makes the bomb explode after a certain amount of time
		yield return new WaitForSeconds(explosiveDelay);

		//creates the smoke cloud so that it is slightly above and parallel to the ground 
		Instantiate(smokePrefab, transform.position + transform.forward*0.5f, Quaternion.Euler(90,0,0));

		//Where the bomb originated
		Vector3 bombOrigin = transform.position;

		//Allows the bomb to have an explosive range
		Collider[] hitables = Physics.OverlapSphere (bombOrigin, radius); 
		

		foreach(Collider thingHit in hitables){  
			if (thingHit.GetComponent<Rigidbody>()){
				AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1);
				Destroy(gameObject);

				
			}	
		}	
	}
}