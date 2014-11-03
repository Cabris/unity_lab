using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {
	
	protected Animator animator;
	public float speed=0;
	public float direction=0;
	AnimatorStateInfo stateInfo;

	void Start () {
		animator = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		if(animator)
		{
			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction);
		}
	}
	
}
