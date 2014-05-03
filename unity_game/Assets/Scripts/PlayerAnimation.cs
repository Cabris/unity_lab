using UnityEngine;
using System.Collections;

public class PlayerAnimation : MonoBehaviour {

	protected Animator animator;
	public bool isWalking=false;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(animator)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
			float speed=0;
			if(isWalking){
				speed=5;
			}
			else{
				speed=0;
			}
			animator.SetFloat("Speed", speed);
		}	
	}
}
