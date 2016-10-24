using UnityEngine;
using System.Collections;

public class ZombieControl : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private Animator myAnimator;
	[SerializeField] private float maxSpeed = -0.1f;
	private bool flip;

	void Start(){
		myRigidbody = GetComponent <Rigidbody2D> ();
	}

	void Update(){
		flip = myRigidbody.transform.GetComponent<SpriteRenderer> ().flipX;

		if (myRigidbody.position.x <= GameConfig.X_MIN || myRigidbody.position.x >= GameConfig.X_MAX) {
			Flip ();
			maxSpeed *= -1;
		}
	}

	void FixedUpdate(){
		HandleMovement ();
	}

	void HandleMovement(){
		myRigidbody.velocity = new Vector2 (maxSpeed, myRigidbody.velocity.y);
		Vector2 position;
		position.x = Mathf.Clamp (myRigidbody.position.x, GameConfig.X_MIN, GameConfig.X_MAX);
		position.y = myRigidbody.position.y;
		myRigidbody.position = position;
	}

	void Flip(){

		if (myRigidbody.velocity.x < 0 && !flip) {
			myRigidbody.transform.GetComponent<SpriteRenderer> ().flipX = true;

		}else if (myRigidbody.velocity.x > 0 && flip){
			myRigidbody.transform.GetComponent<SpriteRenderer> ().flipX = false;

		}

	}
}
