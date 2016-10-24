using UnityEngine;
using System.Collections;


public class HitPlayer : MonoBehaviour{

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
			if (!other.transform.FindChild ("Lash").GetComponent<BoxCollider2D> ().enabled) {
				if (myGameControl.GetLife() > 0) {
					myGameControl.SetLife (-1);
				}
				if(myGameControl.GetLife() <= 0 && !myGameControl.GetDied ()){
					other.GetComponent<Animator> ().SetTrigger ("died");
					myGameControl.SetDied (true);
				}
			}
		}
	}
}
