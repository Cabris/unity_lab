using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	public delegate void ButtonEvent(ButtonControl control,GameObject hit);
	public ButtonEvent OnButtonPress,OnButtonUp;
	//string mask="Player|";
	public bool isPress;
	[SerializeField]
	Transform button;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(IsPress){
			button.localPosition=new Vector3(0,-12,0);
		}
		else{
			button.localPosition=new Vector3(0,-6,0);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		IsPress=true;
		if(OnButtonPress!=null)
			OnButtonPress(this,other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		IsPress=false;
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
