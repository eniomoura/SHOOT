using UnityEngine;
using System.Collections;

public class ThrusterLightControl : MonoBehaviour {
	public bool inverseThruster;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(!inverseThruster){
			light.intensity=ShipBehaviour.thrust;
		}else{
			light.intensity=-ShipBehaviour.thrust;
		}
	}
}
