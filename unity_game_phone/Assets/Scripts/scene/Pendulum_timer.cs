using UnityEngine;
using System.Collections;

public class Pendulum_timer : MonoBehaviour {
	[SerializeField]
	TypogenicText typoText;
	[SerializeField]
	float counter,delta;
	public bool counting;
	[SerializeField]
	PauseRigidBody pause;
	[SerializeField]
	Transform bob;
	// Use this for initialization
	void Start () {
		counter=delta=0;
	}
	//[SerializeField]
	//Vector3 maxhight, hight, prehight;
	[SerializeField]
	Vector3 velocity,prevelocity;
	
	// Update is called once per frame
	void Update () {
		if(!pause.pause){
			
			if(counting)
				counter+=Time.deltaTime;
			else{
				delta=counter;
				counter=0;
			}
			velocity=bob.rigidbody.velocity;
			if(prevelocity.y>0&&velocity.y<0){//+->-
				counting=false;
			}else
				counting=true;
			prevelocity=velocity;
		}
		typoText.Text=counter.ToString("F2")+", "+delta.ToString("F2");
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
