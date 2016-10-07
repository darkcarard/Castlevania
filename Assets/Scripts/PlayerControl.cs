using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private bool facingRight = true;
	private bool jump = false;
	private bool lash = false;

	[SerializeField]
	private float maxSpeed = 5f;
	[SerializeField]
	private float jumpForce;

	private Animator myAnimator;
	private Rigidbody2D myRigidbody;

	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private LayerMask whatIsGround;
	private bool isGrounded;
	[SerializeField]
	private bool airControl=false;

	void Start () {
		myAnimator = GetComponent<Animator> ();
		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	void Update () {
		HandleInput ();
	}

	void FixedUpdate(){
		float horizontal = Input.GetAxis ("Horizontal");
		isGrounded = IsGrounded ();
		HandleMovement (horizontal);
		Flip (horizontal);
		HandleAttacks ();
		ResetValues ();
	}

	void HandleInput(){
		if (Input.GetKeyDown(KeyCode.Space)){
			jump = true;
		}
		if (Input.GetButton ("Fire1")) {
			lash = true;
		}
	}

	void HandleMovement(float horizontal){
		if (!myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Lash") && (isGrounded || airControl)) {
			myAnimator.SetFloat ("speed", Mathf.Abs (horizontal));
			myRigidbody.velocity = new Vector2 (horizontal * maxSpeed, myRigidbody.velocity.y);
		} 
		if (isGrounded && jump) {
			isGrounded = false;
			Jump ();
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

	void Jump(){
		//myAnimator.SetTrigger ("jump");
		myRigidbody.AddForce (new Vector2(0f,jumpForce));
	}

	void ResetValues(){
		lash = false;
		jump = false;
	}

	private bool IsGrounded(){
		if (myRigidbody.velocity.y <= 0) {
			foreach (Transform point in groundPoints) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++) {
					if(colliders[i].gameObject != gameObject) {
						return true;
					}
				}
			}
		}
		return false;
	}
}
