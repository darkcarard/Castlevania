using UnityEngine;
using System.Collections;

public class WhipController : MonoBehaviour {

	public GameObject goldCoin;

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Enemy") {
			Destroy (other.gameObject);
			Instantiate (goldCoin,new Vector3(other.transform.position.x,-0.25f,0f),other.transform.rotation);
		}
	}
}
