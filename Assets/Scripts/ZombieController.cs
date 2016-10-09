using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	Rigidbody2D myRigidbody;
	Animator myAnimator;
	[SerializeField]
	private float maxSpeed = -0.1f;
	[SerializeField]
	private bool facingLeft = true;
	[SerializeField]
	private float bordeIzquierdo;
	[SerializeField]
	private float bordeDerecho;
	private bool isBorder = false;

	void Start(){
		myRigidbody = GetComponent <Rigidbody2D> ();
	}

	void Update(){
		if (( isBorder && !facingLeft) || (isBorder && facingLeft)) {
			Flip ();
		}
	}

	void FixedUpdate(){
		HandleMovement ();
	}

	void HandleMovement(){
		myRigidbody.velocity = new Vector2 (maxSpeed, myRigidbody.velocity.y);
	}

	void Flip(){
		facingLeft = !facingLeft;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
		maxSpeed *= -1;
		isBorder = false;
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Border") {
			isBorder = true;
		}
	}
}
