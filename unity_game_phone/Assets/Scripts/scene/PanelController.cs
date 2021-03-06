﻿using UnityEngine;
using System.Collections;
using System.Text;

public class PanelController : MonoBehaviour {
	
	UILabel textLabel,headerLabel;
	string header;
	StringBuilder text;
	public GameObject owner{get;private set;}
	SelectController controller;
	[SerializeField]
	LineRenderer line;
	GameObject close;
	public Transform actualTransform{get;private set;}

	// Use this for initialization
	void Start () {
		textLabel=transform.Find("Text").GetComponent<UILabel>();
		headerLabel=transform.Find("Header").GetComponent<UILabel>();
		text=new StringBuilder();
		line=transform.Find("Link").GetComponent<LineRenderer>();
		close=transform.Find("Close").gameObject;
		UIEventListener.Get(close).onClick = ButtonClick;
	}

	void ButtonClick(GameObject button)
	{
		if(button==close){
			controller.OnUnselect(owner);
		}
	}

	public void Init(SelectController controller,GameObject owner){
		this.controller=controller;
		this.owner=owner;
		actualTransform= GetComponent<CameraFacingBillboard>().Init();
	}

	// Update is called once per frame
	void Update () {
		if(owner!=null){
			header=owner.name;
			text.Remove(0,text.Length);
			updateText (text);
			line.SetPosition(0,line.transform.position);
			line.SetPosition(1,owner.transform.position);
		}
		textLabel.text=text.ToString();
		headerLabel.text=header;
	}
	
	void updateText (StringBuilder text )
	{
		Rigidbody r = owner.rigidbody;
		if (r != null) {
			text.AppendLine (@"mass: " + r.mass);
			text.AppendLine (@"position: " + r.position);
			text.AppendLine (@"velocity: " + r.velocity);
			text.AppendLine (@"rotation: " + r.rotation.eulerAngles);
			text.AppendLine (@"drag: " + r.drag);
		}
	}
}
