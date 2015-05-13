using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {
	[SerializeField]
	 Animator animator;
	public float speed=0;
	public float direction=0;
	AnimatorStateInfo stateInfo;
	//GameObject root;

	void Start () {
		//animator = GetComponent<Animator>();
		//root = transform.Find ("Root").gameObject;
	}
	
	void FixedUpdate () {
//		if(animator)
//		{
//			animator.SetFloat("Speed", speed);
//			animator.SetFloat("Direction", direction);
//		}
	}

	public void Disable(){
		animator.enabled = false;

	}

	public void Enable(){
		animator.enabled = true;

	}
}
