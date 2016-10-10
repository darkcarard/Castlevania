using UnityEngine;
using System.Collections;

public class WhipControl : MonoBehaviour {

	public GameObject explosion;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Enemy") {
			Instantiate (explosion,other.transform.position,other.transform.rotation);
			Destroy (other.gameObject);	
		}
	}


}
