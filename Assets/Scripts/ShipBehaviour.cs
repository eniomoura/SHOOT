using UnityEngine;
using System.Collections;

public class ShipBehaviour : MonoBehaviour {
	public float manueverability;
	public float thrustPower;
	public float maxThrust;
	public float hull;
	public ParticleSystem fire;
	public GameObject thrusterlight;
	public static float thrust;
	private float turnx=0;
	private float turny=0;
	private bool onFire=false;
	private bool dead=false;
	private float inimanuev;
	private float inipower;
	private float inithrust;
	private float inihull;
	private float timetorespawn;
	private Component trail;
	private Component halo;

	// Use this for initialization
	void Start () {
		inimanuev=manueverability;
		inipower=thrustPower;
		inithrust=maxThrust;
		inihull=hull;
		trail=GetComponent("TrailRenderer");
		halo=GetComponent("Halo");
	}
	
	// Update is called once per frame
	void Update (){
		if(Input.GetKeyDown("escape")){
			Application.Quit();
		}
		if(dead){
			if(timetorespawn<=0){
				respawn();
			}else{
				timetorespawn-=Time.deltaTime;
			}
		}else{
			DamageCheck();
			turnx+=Input.GetAxisRaw("Mouse X");
			turny+=Input.GetAxisRaw("Mouse Y");
			if(Input.GetKey("c")){
				thrust=0;
			}else{
				if((thrust>-maxThrust&&Input.GetAxis("Thrust")<0)||(thrust<maxThrust&&Input.GetAxis("Thrust")>0)){
					thrust+=Input.GetAxis("Thrust");
				}
			}
			rigidbody.AddRelativeTorque(-turny*(manueverability/8)*Time.deltaTime, turnx*(manueverability/8)*Time.deltaTime, -Input.GetAxis("Spin")*(manueverability)*Time.deltaTime);
			rigidbody.AddRelativeForce (0 , 0, ((thrust*thrustPower/20)*Time.deltaTime));
			if (Input.GetButton ("Drift")) {
				rigidbody.AddRelativeForce (((Input.GetAxis ("Drift")*manueverability*3)*Time.deltaTime),0,0);
			}
			if (Input.GetButton ("Altitude")) {
				rigidbody.AddRelativeForce (0, ((Input.GetAxis ("Altitude")*manueverability*3)*Time.deltaTime), 0);
			}

			if(rigidbody.position.x>100||rigidbody.position.y>100||rigidbody.position.z>100){
				rigidbody.position=new Vector3(0,0,0);
			}
			if(Input.GetButton("ICS")){
				rigidbody.drag=0.0f;
			}else{
				rigidbody.drag=1.0f;
			}
		}
	}

	void OnGUI (){
		if(dead)GUI.Box(new Rect((Screen.width/2)+100, (Screen.height/2-100), 100, 100), "Dead!\nRespawn:\n"+Mathf.Round(timetorespawn)+ "s");
		if(PropBehaviour.victorycond>=3)GUI.Box(new Rect((Screen.width/2)-200, (Screen.height/2-100), 200, 100), "A WINNER IS YOU\nPRESS ESC TO EXIT");
		GUI.Box(new Rect(0,Screen.height-100, 200, 100), "HUD"+"\nHull: "+hull+"\nSpeed: "+Mathf.Floor(rigidbody.velocity.magnitude*100)+" m/s"+"\nThrust: "+Mathf.Floor(thrust)+" kN ("+Mathf.Round((thrust/inithrust)*100)+"%)"+"\nICS: "+(rigidbody.drag==1.0 ? "ON" : "OFF"));
		//GUI.Box(new Rect(Screen.width-100, Screen.height-130, 100, 100), "Debug:\nfire:\n"+fire.isPlaying+"\nDead: "+dead);
	}

	void OnCollisionEnter(Collision obstacle){
		hull=hull-Mathf.Pow(obstacle.relativeVelocity.magnitude, 2);
		thrust=0;
		if(hull<20){
			onFire=true;
			fire.Play();
		}
	}

	void DamageCheck(){
		if(onFire&&!dead){
			hull=hull-Time.deltaTime;
		}
		if(hull<=0){
			hull=0.0f;
			manueverability=0.0f;
			maxThrust=0.0f;
			thrustPower=0.0f;
			thrust=0.0f;
			rigidbody.drag=0.0f;
			onFire=false;
			fire.Stop();
			trail.GetType().GetProperty("enabled").SetValue(trail, false, null);
			trail.GetType().GetProperty("time").SetValue(trail, 0, null);
			halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
			dead=true;
			timetorespawn=5;
		}else{
			manueverability=inimanuev*(hull/100);
			maxThrust=inithrust*(hull/100);
			thrustPower=inipower*(hull/100);
		}
	}

	void respawn(){
		hull=inihull;
		manueverability=inimanuev;
		maxThrust=inithrust;
		thrustPower=inipower;
		thrust=0.0f;
		turnx=0.0f;
		turny=0.0f;
		rigidbody.drag=1.0f;
		trail.GetType().GetProperty("enabled").SetValue(trail, true, null);
		trail.GetType().GetProperty("time").SetValue(trail, 5, null);
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
		rigidbody.position=new Vector3(0,0,0);
		transform.rotation=Quaternion.identity;
		rigidbody.velocity=new Vector3(0,0,0);
		onFire=false;
		fire.Stop();
		dead=false;
	}
}
