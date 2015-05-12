using UnityEngine;
using System.Collections;

public class SlopeControl : MonoBehaviour {
	[SerializeField]
	Transform ground;
	[SerializeField]
	Transform leftCylinder,rightCylinder,leftSlope,rightSlope;
	public float width;
	public float leftHeight,leftWidth;
	public float rightHeight,rightWidth;
	//public Transform ball;
	// Use this for initialization
	void Start () {
		updateSlope ();

	}
	
	// Update is called once per frame
	void Update () {
		updateSlope ();
		float leftA = Mathf.Atan2 (leftHeight, -leftWidth);
		float rightA = Mathf.Atan2 (rightHeight, rightWidth);
//		float m=ball.rigidbody.mass;
//		Rigidbody r=ball.rigidbody;
//		if(ball.position.x<ground.position.x-width/2){//left
//			
//		}
//		if(ball.position.x>ground.position.x+width/2){//right
//
//		}
//		if(ball.position.x<ground.position.x+width/2&&
//		   ball.position.x>ground.position.x-width/2){
//			
//		}





	}

	void updateSlope ()
	{
		if(width<0)
			width=0;
		if(leftWidth<0)
			leftWidth=0;
		if(rightWidth<0)
			rightWidth=0;
		if(leftHeight<0)
			leftHeight=0;
		if(rightHeight<0)
			rightHeight=0;

		Vector3 p;
		Vector3 s = ground.localScale;
		s.x = width;
		ground.localScale = s;

		s = leftCylinder.localScale;
		s.y = leftHeight/2;
		leftCylinder.localScale = s;
		p = leftCylinder.localPosition;
		p.y = leftHeight/2;
		p.x = -width / 2 - leftWidth;
		leftCylinder.localPosition = p;

		s = rightCylinder.localScale;
		s.y = rightHeight/2;
		rightCylinder.localScale = s;
		p = rightCylinder.localPosition;
		p.y = rightHeight/2;
		p.x = width / 2 + rightWidth;
		rightCylinder.localPosition = p;

		float leftA = Mathf.Atan2 (leftHeight, -leftWidth);
		float rightA = Mathf.Atan2 (rightHeight, rightWidth);
		leftSlope.localRotation = Quaternion.Euler (0, 0, leftA * Mathf.Rad2Deg);
		rightSlope.localRotation = Quaternion.Euler (0, 0, rightA * Mathf.Rad2Deg);
		p = leftSlope.localPosition;
		p.y = leftHeight / 2;
		p.x = -width / 2 - leftWidth / 2;
		leftSlope.localPosition = p;
		s = leftSlope.localScale;
		s.x = leftWidth / Mathf.Cos (leftA);
		leftSlope.localScale = s;
		p = rightSlope.localPosition;
		p.y = rightHeight / 2;
		p.x = width / 2 + rightWidth / 2;
		rightSlope.localPosition = p;
		s = rightSlope.localScale;
		s.x = rightWidth / Mathf.Cos (rightA);
		rightSlope.localScale = s;
	}
}
