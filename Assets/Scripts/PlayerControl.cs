using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private bool facingRight = true;
	private bool jump = false;
	private bool lash = false;
	private bool died = false;
	[SerializeField]
	private bool hit = false;
	//[SerializeField]
	//private bool invulnerate = false;
	[SerializeField]
	private bool isGrounded;

	[SerializeField]
	private float maxSpeed = 5f;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private float jumpForce;
	[SerializeField]
	private float xMin;
	[SerializeField]
	private float xMax;

	private Animator myAnimator;
	private Rigidbody2D myRigidbody;
	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private LayerMask whatIsGround;
	private GameControl myGameControl;

	[SerializeField]
	private Text lifeText;
	[SerializeField]
	private Text ammoText;

	private int maxLife = 20;
	private int life = 10;
	private int maxAmmo = 10;
	private int ammo = 0;
	[SerializeField]
	private GameObject[] enemies;
	[SerializeField]
	private int maxEnemies;


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
		HandleInput ();
	}

	void FixedUpdate(){
		if (!died) {
			float horizontal = Input.GetAxis ("Horizontal");
			isGrounded = IsGrounded ();
			HandleMovement (horizontal);
			Flip (horizontal);
			HandleAttacks ();
			ResetValues ();
		}
	}

	void HandleInput(){
		if (Input.GetKeyDown(KeyCode.X)){
			jump = true;
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			lash = true;
		}
	}

	void HandleMovement(float horizontal){
		if (isGrounded && jump && !hit) {
			if (myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Run")) {
				myAnimator.SetFloat ("speed", 0f);
			}
			Jump ();
			isGrounded = false;
		}
		if (!myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Lash") && isGrounded && !hit) {
			myAnimator.SetFloat ("speed", Mathf.Abs (horizontal));
			myRigidbody.velocity = new Vector2 (horizontal * maxSpeed, myRigidbody.velocity.y);
			Vector2 position;
			position.x = Mathf.Clamp (myRigidbody.position.x, xMin, xMax);
			position.y = myRigidbody.position.y;
			myRigidbody.position = position;
		} 
	}

	void HandleAttacks(){
		if (lash && !myAnimator.GetCurrentAnimatorStateInfo (0).IsTag ("Lash")){
			Lash ();
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
		transform.Find ("Lash").GetComponent<AudioSource> ().Play ();
	}

	void Jump(){
		myAnimator.SetTrigger ("jump");
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
						myAnimator.ResetTrigger ("jump");
						return true;
					}
				}
			}
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Bow") {
			if (ammo <= maxAmmo) {
				ammo++;
				myGameControl.SetAmmo (1);
				Destroy (other.gameObject);
			}
		} else if (other.tag == "Hearth") {
			if (life <= maxLife) {
				life++;
				myGameControl.SetLife (1);
				other.GetComponent<AudioSource> ().Play ();
				Destroy (other.gameObject,0.2f);
			}
		}else if (other.tag == "Door"){
			//Application.LoadLevel(1);
			myGameControl.DeleteAll ();
			SceneManager.LoadScene(1);
		}else if(other.tag == "Enemy"){
			if (!transform.FindChild ("Lash").GetComponent<BoxCollider2D> ().enabled) {
				if (life > 0) {
					//myAnimator.SetTrigger ("hit");
					//if (!invulnerate) {
					life--;
						myGameControl.SetLife (-1);
					//}
					Die ();
				}
			}
		}
		//SetPuntaje ();
	}

	void Die(){
		if (life == 0){
			//myAnimator.ResetTrigger ("hit");
			myAnimator.SetTrigger ("died");
			died = true;
		}
	}
		
}
