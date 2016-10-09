using UnityEngine;
using System.Collections;

public class ZombieController : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private Animator myAnimator;
	[SerializeField]
	private float maxSpeed = -0.1f;
	[SerializeField]
	private bool facingLeft = true;
	[SerializeField]
	private float bordeIzquierdo;
	[SerializeField]
	private float bordeDerecho;
	private bool isBorder = false;
	[SerializeField]
	private GameObject[] loot;

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

	void OnDestroy(){
		Instantiate (GetLoot (),new Vector3(transform.position.x,-0.25f,0f),transform.rotation);
	}

	GameObject GetLoot(){
		int index = Random.Range (0, loot.Length);
		return loot [index];
	}
}
