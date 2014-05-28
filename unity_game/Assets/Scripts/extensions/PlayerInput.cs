using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
	
	float speed=2.5f;
	public Transform character;
	bool isTurnRight=false;
	bool isTurnLeft=false;
	PlayerAnimation playerAnimation;
	void Start () 
	{
		playerAnimation=GetComponent<PlayerAnimation>();
	}

	void Right(){
		character.Rotate(new Vector3(0,1,0)*80*Time.deltaTime, Space.World);
	}
	void Left(){
		character.Rotate(new Vector3(0,1,0)*-80*Time.deltaTime, Space.World);
	}

	public void StartWalk(){
		if(!playerAnimation.isWalking)
			playerAnimation.isWalking=true;
	}
	public void EndWalk(){
		playerAnimation.isWalking=false;
	}

	public void StartTurn(string cmd){
		if(cmd=="right"){
			isTurnRight=true;
		}
		if(cmd=="left"){
			isTurnLeft=true;
		}
	}

	public void EndTurn(string cmd){
		if(cmd=="right"){
			isTurnRight=false;
		}
		if(cmd=="left"){
			isTurnLeft=false;
		}
	}

	void Update () {
		float dt=Time.deltaTime;
		if(playerAnimation.isWalking){
			character.Translate(Vector3.forward * speed*dt);
		}
		if(isTurnLeft){
			Left();
		}
		if(isTurnRight){
			Right();
		}
	}
 		  
}
