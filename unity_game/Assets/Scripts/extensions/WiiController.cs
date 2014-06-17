using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class WiiController : MonoBehaviour {
	
	[DllImport ("UniWii")]
	private static extern void wiimote_start();
	[DllImport ("UniWii")]
	private static extern void wiimote_stop();
	[DllImport ("UniWii")]
	private static extern int wiimote_count();

	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonA(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonB(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonUp(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonLeft(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonRight(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonDown(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButton1(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButton2(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonNunchuckC(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonNunchuckZ(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonPlus(int which);
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonMinus(int which);
	
	//public bool left,right,up,down;
	public InputListener inputListener;
	public float horizontal;
	public float vertical;
	KeyboardController keyboard;
	public int userId;
	// Use this for initialization
	void Start () {
		wiimote_start();
		inputListener=GetComponent<InputListener>();
		keyboard=GetComponent<KeyboardController>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(wiimote_count()>0){
			bool left,right,up,down;
			left=wiimote_getButtonLeft(userId);
			right=wiimote_getButtonRight(userId);
			up=wiimote_getButtonUp(userId);
			down=wiimote_getButtonDown(userId);
			horizontal=0;
			vertical=0;
			if(left)
				horizontal=-1;
			if(right)
				horizontal=1;
			if(up)
				vertical=1;
			if(down)
				vertical=-1;
			if(inputListener!=null){
				inputListener.Horizontal=horizontal;
				inputListener.Vertical=vertical;
				inputListener.ButtonBPress=wiimote_getButtonB(userId);
			}
			//Debug.Log("wii input");
		}
		if(keyboard!=null){
			if(wiimote_count()>0)
				keyboard.enabled=false;
			else
				keyboard.enabled=true;
		}
		Color c=r.material.color;
		isPressA=wiimote_getButtonA(userId);
		if(isPressA){
			c.a=0;
			r.enabled=false;
		}
		else{
			c.a=1;
			r.enabled=true;
		}
		r.material.color=c;

	}

	public bool isPressA=false;
	public Renderer r;



}
