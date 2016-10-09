using UnityEngine;
using System.Collections;

public class WhipController : MonoBehaviour {

	public GameObject hearth;
	public GameObject explosion;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Enemy") {
			Instantiate (explosion,other.transform.position,other.transform.rotation);
			Destroy (other.gameObject);	
			Instantiate (hearth,new Vector3(other.transform.position.x,-0.25f,0f),other.transform.rotation);
		}
	}
}
