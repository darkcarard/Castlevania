using UnityEngine;
using System.Collections;

public class ZombieControl : MonoBehaviour {

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
	[SerializeField]
	private GameObject[] loot;
	[SerializeField]
	private float xMin;
	[SerializeField]
	private float xMax;

	private bool isQuitting;

	void Start(){
		myRigidbody = GetComponent <Rigidbody2D> ();
	}

	void Update(){
		if (myRigidbody.position.x <= xMin || myRigidbody.position.x >= xMax) {
			facingLeft = false;
		}
		if (!facingLeft) {
			Flip ();
		}
	}

	void FixedUpdate(){
		HandleMovement ();
	}

	void HandleMovement(){
		myRigidbody.velocity = new Vector2 (maxSpeed, myRigidbody.velocity.y);
		Vector2 position;
		position.x = Mathf.Clamp (myRigidbody.position.x, xMin, xMax);
		position.y = myRigidbody.position.y;
		myRigidbody.position = position;
	}

	void Flip(){
		facingLeft = !facingLeft;
		Vector3 myScale = transform.localScale;
		myScale.x *= -1;
		transform.localScale = myScale;
		maxSpeed *= -1;
	}

	void OnDestroy(){
		if (!isQuitting){
			Instantiate (GetLoot (),new Vector3(transform.position.x,-0.25f,0f),transform.rotation);
		}
	}

	GameObject GetLoot(){
		int index = Random.Range (0, loot.Length);
		return loot [index];
	}

	void OnApplicationQuit(){
		isQuitting = true;
	}
}
