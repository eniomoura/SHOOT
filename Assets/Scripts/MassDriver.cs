using UnityEngine;
using System.Collections;

public class MassDriver : MonoBehaviour {

	public GameObject ammo;
	private GameObject shot;
	private Vector3 aim;
	private float time=5;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		if((Input.GetButton("Weapon 1"))&&(time>0.5)){
			time=0;
			aim=transform.TransformPoint(Vector3.forward);
			shot = (GameObject)(GameObject.Instantiate(ammo, aim, Quaternion.identity));
			shot.rigidbody.velocity=rigidbody.velocity;
			shot.rigidbody.AddRelativeForce(transform.TransformDirection(Vector3.forward)*Mathf.Pow(10,3));
			Destroy(shot, 4);
		}
	}
}
