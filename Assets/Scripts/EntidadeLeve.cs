using UnityEngine;
using System.Collections;

public class EntidadeLeve : MonoBehaviour {
	void OnCollisionEnter(){
		Destroy(gameObject);
	}
}
