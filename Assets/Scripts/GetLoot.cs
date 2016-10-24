using UnityEngine;
using System.Collections;


public class GetLoot : MonoBehaviour{

	private GameControl myGameControl;

	void Start(){
		GameObject myGameControlObject = GameObject.FindWithTag ("GameControl");
		if (myGameControlObject != null){
			myGameControl = myGameControlObject.GetComponent<GameControl> ();
		}else{
			Debug.Log ("No se encontró el script 'GameControl'");
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if(other.tag == "Player"){
			transform.GetComponent<AudioSource> ().Play ();
			if(transform.tag == "Ammo"){
				myGameControl.SetAmmo (1);
			}else if(transform.tag == "Life"){
				myGameControl.SetLife (1);
			}
			transform.GetComponent<SpriteRenderer> ().enabled = false;
			Destroy (gameObject,0.8f);
		}
	}
}
