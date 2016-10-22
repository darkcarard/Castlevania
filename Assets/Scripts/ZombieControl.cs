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
	private GameObject[] loot;
	private bool flip;


	private bool isQuitting;

	void Start(){
		myRigidbody = GetComponent <Rigidbody2D> ();
	}

	void Update(){
		flip = myRigidbody.transform.GetComponent<SpriteRenderer> ().flipX;
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
		print (myRigidbody.velocity.x);
		if ((myRigidbody.position.x <= GameConfig.X_MIN || myRigidbody.position.x >= GameConfig.X_MAX) && !flip){
			Flip ();
			maxSpeed *= -1;
		}
	}

	void Flip(){
		if (myRigidbody.velocity.x < 0 && !flip) {
			myRigidbody.transform.GetComponent<SpriteRenderer> ().flipX = true;
		}else if (myRigidbody.velocity.x > 0 && !flip){
			myRigidbody.transform.GetComponent<SpriteRenderer> ().flipX = false;
		}
	}

	void OnDestroy(){
		if (!isQuitting){
			Instantiate (GetLoot (),new Vector3(transform.position.x,GameConfig.Y_MIN,0f),transform.rotation);
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
