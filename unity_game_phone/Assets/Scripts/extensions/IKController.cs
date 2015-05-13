using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))] 
public class IKController : MonoBehaviour {
	
	public Animator animator;
	public bool isActive = false;
	 Transform rightHandTarget = null;
	
	void Start () 
	{
		animator = GetComponent<Animator>();
	}
	
	//a callback for calculating IK
	void OnAnimatorIK()
	{
		if(animator) {

			if(isActive) {
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,1.0f);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,1.0f);
	
				if(rightHandTarget != null) {
					animator.SetIKPosition(AvatarIKGoal.RightHand,rightHandTarget.position);
					animator.SetIKRotation(AvatarIKGoal.RightHand,rightHandTarget.rotation);
				}                   
				//Debug.Log("OnAnimatorIK");
			}
			else {          
				animator.SetIKPositionWeight(AvatarIKGoal.RightHand,0);
				animator.SetIKRotationWeight(AvatarIKGoal.RightHand,0);             
			}
		}
	}    
}
