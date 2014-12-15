using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	public delegate void ButtonEvent(ButtonControl control,GameObject hit);
	public ButtonEvent OnButtonPress,OnButtonUp;
	//string mask="Player|";
	[SerializeField]
	bool isPress;
	[SerializeField]
	Transform button;
	HashSet<Collider> colliders=new HashSet<Collider>();
	float duration=.05f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		isPress=colliders.Count>0;
		if(IsPress){
			TweenPosition.Begin(button.gameObject,duration,new Vector3(0,-12,0));
			//button.localPosition=new Vector3(0,-12,0);
		}
		else{
			TweenPosition.Begin(button.gameObject,duration,new Vector3(0,-6,0));
			//button.localPosition=new Vector3(0,-6,0);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		colliders.Add(other);
		if(OnButtonPress!=null)
			OnButtonPress(this,other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		colliders.Remove(other);
		if(OnButtonUp!=null)
			OnButtonUp(this,other.gameObject);
	}
	
	public bool IsPress{
		get{return isPress;}
		set{
			isPress=value;
		}
	}
	
}
