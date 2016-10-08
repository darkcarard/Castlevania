using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	Rigidbody2D myRigidbody;
	Animator myAnimator;
	[SerializeField]
	private float maxSpeed=0.1f;
	private float horizontal=-1f;

	private bool facingRight = false;

	void Start(){

		myRigidbody = GetComponent <Rigidbody2D> ();
		myAnimator = GetComponent <Animator> ();
	}

	void FixedUpdate(){
		HandleMovement ();
		Flip (horizontal);
	}

	void HandleMovement(){
		myRigidbody.velocity = new Vector2 (horizontal * maxSpeed, myRigidbody.velocity.y);
	}

	void Flip(float horizontal){
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 myScale = transform.localScale;
			myScale.x *= -1;
			transform.localScale = myScale;
		}
	}
}
