using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {
	
	protected Animator animator;
	public float speed=0;
	public float direction=0;
	AnimatorStateInfo stateInfo;
	GameObject root;

	void Start () {
		animator = GetComponent<Animator>();
		root = transform.Find ("Root").gameObject;
	}
	
	void FixedUpdate () {
		if(animator)
		{
			animator.SetFloat("Speed", speed);
			animator.SetFloat("Direction", direction);
		}
	}

	public void Apart(){
		animator.enabled = false;
		CharacterJoint[] js=root.GetComponentsInChildren<CharacterJoint>();
		foreach(CharacterJoint j in js){
			//GameObject.Destroy(j);
		}
		GameObject.Destroy(rigidbody);
		GameObject.Destroy(collider);
	}
	
}
