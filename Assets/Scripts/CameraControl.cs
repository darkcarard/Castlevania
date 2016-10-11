using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField]
	private float xMin;
	[SerializeField]
	private float xMax;
	[SerializeField]
	private float yMin;
	[SerializeField]
	private float yMax;

	private Transform target;

	void Start(){
		target = GameObject.Find ("Player").transform;
	}

	void LateUpdate(){
		transform.position = new Vector3 (Mathf.Clamp (target.position.x, xMin, xMax), Mathf.Clamp (target.position.y, yMin, yMax), transform.position.z);
	}
}