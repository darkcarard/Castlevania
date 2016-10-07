using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private bool facingRight = true;
	private bool jump = false;
	private bool lash = false;

	public float maxSpeed = 5f;
	public float jumpForce = 5f;
	//public Transform groundCheck;

	private bool grounded = false;
	private Animator myAnimator;
	private Rigidbody2D myRigidbody;

	void Start () {
		myAnimator = GetComponent<Animator> ();
		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		HandleInput ();
	}

	void FixedUpdate(){
		float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement (horizontal);
		Flip (horizontal);
		HandleAttacks ();
		ResetValues ();
	}

	void HandleInput(){
		if (Input.GetButtonDown("Jump")){// && grounded){
			jump = true;
		}
		if (Input.GetButton ("Fire1")) {
			lash = true;
		}
	}

	void HandleMovement(float horizontal){
		if (!myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Lash") && !jump ) {
			myAnimator.SetFloat ("speed", Mathf.Abs (horizontal));
			myRigidbody.velocity = new Vector2 (horizontal * maxSpeed, myRigidbody.velocity.y);

		} else if (jump) {
			Jump (horizontal);
		}
	}

	void HandleAttacks(){
		if (lash && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Lash")){
			myRigidbody.velocity = Vector2.zero;
			myAnimator.SetTrigger ("lash");
		}
	}

	void Flip(float horizontal){
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight;
			Vector3 myScale = transform.localScale;
			myScale.x *= -1;
			transform.localScale = myScale;
		}
	}

	void Lash(){
		myAnimator.SetTrigger ("lash");
		myRigidbody.velocity = Vector2.zero;
	}

	void Jump(float horizontal){
		myAnimator.SetTrigger ("jump");
		//myRigidbody.AddForce (new Vector2(myRigidbody.position.x,jumpForce));
		myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, horizontal * maxSpeed);
		myRigidbody.velocity = Vector2.zero;
	}

	void ResetValues(){
		lash = false;
		jump = false;
	}
}
