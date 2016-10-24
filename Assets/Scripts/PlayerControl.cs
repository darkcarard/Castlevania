using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private bool facingRight = true;
	[SerializeField] private bool isGrounded = true;
	[SerializeField] private float maxSpeed = 5f;
	[SerializeField] private float jumpForce;
	[SerializeField] private float xMin;
	[SerializeField] private float xMax;
	private Animator myAnimator;
	private Rigidbody2D myRigidbody;
	private GameControl myGameControl;

	void Start () {
		myAnimator = GetComponent<Animator> ();
		myRigidbody = GetComponent<Rigidbody2D> ();
		GameObject myGameControlObject = GameObject.FindWithTag ("GameControl");
		if (myGameControlObject != null){
			myGameControl = myGameControlObject.GetComponent<GameControl> ();
		}else{
			Debug.Log ("No se encontró el script 'GameControl'");
		}
	}

	void Update () {
		//Die ();
		if (isGrounded && Input.GetKeyDown(KeyCode.X)){
			myAnimator.SetBool ("ground",false);
			myRigidbody.AddForce (new Vector2(0f,jumpForce));	
		}
		if (isGrounded && Input.GetKeyDown (KeyCode.Z)) {
			myAnimator.SetTrigger ("lash");
			myRigidbody.velocity = Vector2.zero;
		}
	}

	void FixedUpdate(){
		myAnimator.SetBool ("ground", isGrounded);
		myAnimator.SetFloat ("vSpeed",myRigidbody.velocity.y);

		if (!myGameControl.GetDied ()) {
			float move = Input.GetAxis ("Horizontal");
			if (isGrounded) {
				HandleMovement (move);
				if (move > 0 && !facingRight) {
					Flip ();
				} else if (move < 0 && facingRight) {
					Flip ();
				}
			}
		}
	}

	void HandleMovement(float move){
			myAnimator.SetFloat ("speed", Mathf.Abs (move));
			myRigidbody.velocity = new Vector2 (move * maxSpeed, myRigidbody.velocity.y);
			Vector2 position;
			position.x = Mathf.Clamp (myRigidbody.position.x, xMin, xMax);
			position.y = myRigidbody.position.y;
			myRigidbody.position = position;
	}

	void Flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Door"){
			myGameControl.DeleteAll ();
			SceneManager.LoadScene(1);
		}
	}
		
	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Floor"){
			isGrounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.tag == "Floor"){
			isGrounded = false;
		}
	}

	void Hit(){
		Vector2 force = myRigidbody.velocity;
		myRigidbody.AddForce (force * Time.deltaTime);
	}
}
