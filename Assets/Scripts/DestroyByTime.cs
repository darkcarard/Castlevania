using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

	[SerializeField]
	private float lifetime;

	void Start () {
		Destroy (gameObject, lifetime);
	}
}
