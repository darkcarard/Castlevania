using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	private bool facingRight = true;
	private bool died = false;
	[SerializeField] private bool hit = false;
	//[SerializeField]
	//private bool invulnerate = false;
	[SerializeField] private bool isGrounded = true;

	[SerializeField] private float maxSpeed = 5f;
	private float groundRadius = 0.2f;
	[SerializeField] private float jumpForce;
	[SerializeField] private float xMin;
	[SerializeField] private float xMax;
	[SerializeField] private float xHitForce;
	[SerializeField] private float yHitForce;

	private Animator myAnimator;
	private Rigidbody2D myRigidbody;
	[SerializeField] private Transform[] groundPoints;
	[SerializeField] private LayerMask whatIsGround;
	private GameControl myGameControl;

	[SerializeField] private Text lifeText;
	[SerializeField] private Text ammoText;

	private int maxLife = 20;
	private int life = 10;
	private int maxAmmo = 10;
	private int ammo = 0;
	[SerializeField] private GameObject[] enemies;
	[SerializeField] private int maxEnemies;


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
		if (isGrounded && Input.GetKeyDown(KeyCode.X)){
			myAnimator.SetBool ("ground",false);
			myRigidbody.AddForce (new Vector2(0f,jumpForce));	
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			myAnimator.SetTrigger ("lash");
			myRigidbody.velocity = Vector2.zero;
			transform.Find ("Lash").GetComponent<AudioSource> ().Play ();
		}
	}

	void FixedUpdate(){
		isGrounded = IsGrounded ();
		myAnimator.SetBool ("ground", isGrounded);
		myAnimator.SetFloat ("vSpeed",myRigidbody.velocity.y);

		if (!died) {
			float move = Input.GetAxis ("Horizontal");
			HandleMovement (move);
			if (move > 0 && !facingRight){
				Flip ();
			}else if(move < 0 && facingRight){
				Flip ();
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

	void Hit(){
		Vector2 force = myRigidbody.velocity;
		//if (facingRight){
			//force *= -1;
		//}
		myRigidbody.AddForce (force * Time.deltaTime);
		//myRigidbody.AddForce (transform.right * xHitForce);
	}

	private bool IsGrounded(){
		foreach (Transform point in groundPoints) {
			Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
			for (int i = 0; i < colliders.Length; i++) {
				if(colliders[i].gameObject != gameObject) {
					myAnimator.ResetTrigger ("jump");
					return true;
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
				LootPlaySound (other.gameObject);
				other.transform.GetComponent<SpriteRenderer> ().enabled = false;
				Destroy (other.gameObject,0.8f);
			}
		} else if (other.tag == "Hearth") {
			if (life <= maxLife) {
				life++;
				myGameControl.SetLife (1);
				LootPlaySound (other.gameObject);
				other.transform.GetComponent<SpriteRenderer> ().enabled = false;
				Destroy (other.gameObject,0.8f);
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
					Hit ();
					Die ();
				}
			}
		}
	}

	void Die(){
		if (life == 0){
			//myAnimator.ResetTrigger ("hit");
			myAnimator.SetTrigger ("died");
			died = true;
		}
	}

	void LootPlaySound(GameObject item){
		item.GetComponent<AudioSource> ().Play ();
	}
		
}
