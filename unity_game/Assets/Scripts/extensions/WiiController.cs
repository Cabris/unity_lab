using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class WiiController : MonoBehaviour {

	#if UNITY_ANDROID
	[DllImport ("UniWii")]
	private static extern void wiimote_start();
	[DllImport ("UniWii")]
	private static extern void wiimote_stop();
	[DllImport ("UniWii")]
	private static extern int wiimote_count();

	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccX(int which);
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccY(int which);
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccZ(int which);
	
	[DllImport ("UniWii")]
	private static extern float wiimote_getIrX(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getIrY(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getRoll(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getPitch(int which);
	[DllImport ("UniWii")]
	private static extern float wiimote_getYaw(int which);

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
	#endif

	//public bool left,right,up,down;
	public InputListener inputListener;
	public int userId;
	public Renderer _renderer;
	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		wiimote_start();
		inputListener=GetComponent<InputListener>();
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if(wiimote_count()>0&&inputListener!=null){
			bool left,right,up,down;
			left=wiimote_getButtonLeft(userId);
			right=wiimote_getButtonRight(userId);
			up=wiimote_getButtonUp(userId);
			down=wiimote_getButtonDown(userId);
			float horizontal=0;
			float vertical=0;
			if(left)
				horizontal=-1;
			if(right)
				horizontal=1;
			if(up)
				vertical=1;
			if(down)
				vertical=-1;
			inputListener.Horizontal=horizontal;
			inputListener.Vertical=vertical;

			inputListener.SetButtonValue("Fire1",wiimote_getButtonB(userId));
			inputListener.SetButtonValue("Fire2",wiimote_getButtonA(userId));
			bool temp=wiimote_getButtonPlus(userId)||wiimote_getButtonMinus(userId);
			inputListener.SetButtonValue("Fire3",temp);
			//Debug.Log("wii input");
		}
		#endif
	}


	void FixedUpdate () {
	}

	void OnApplicationQuit() {

	}



}
