using UnityEngine;
using System.Collections;

public class PropBehaviour : MonoBehaviour {

	public float hull;
	public static int victorycond=0;

	// Update is called once per frame
	void Update () {
		if(hull<=0){
			particleSystem.Play();
			Destroy(gameObject,1);
			this.enabled=false;
			victorycond++;
		}
	}

	void OnCollisionEnter(Collision projectile){
		hull=hull-Mathf.Pow(projectile.relativeVelocity.magnitude, 2);
	}
}
