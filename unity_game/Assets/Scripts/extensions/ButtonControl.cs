using UnityEngine;
using System.Collections;

public class ButtonControl : MonoBehaviour {

	public delegate void ButtonEvent(ButtonControl control);
	public ButtonEvent OnButtonPress,OnButtonUp;

	public bool isPress;
	[SerializeField]
	Transform button;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(IsPress){
			button.localPosition=new Vector3(0,-6,0);
		}
		else{
			button.localPosition=new Vector3(0,0,0);
		}
	}
	
	void OnTriggerEnter(Collider other) {
		IsPress=true;
		if(OnButtonPress!=null&&other.gameObject.tag=="Player")
			OnButtonPress(this);
	}

	void OnTriggerExit(Collider other) {
		IsPress=false;
		if(OnButtonUp!=null&&other.gameObject.tag=="Player")
			OnButtonUp(this);
	}
	
	public bool IsPress{
		get{return isPress;}
		set{
			isPress=value;
		}
	}
	
}
