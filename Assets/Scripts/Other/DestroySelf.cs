using UnityEngine;
using System.Collections;

public class DestroySelf : MonoBehaviour {

	public float destroyTime = 0f;
	public bool deactivate = false;


	void Awake () {
		if (!deactivate) {
			Destroy(gameObject, destroyTime);
		}
		else {
			gameObject.SetActive(false);
		}
	}
}
