using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class UniiWiiCheck : MonoBehaviour {
	
	[DllImport ("UniWii")]
	private static extern void wiimote_start();
	
	[DllImport ("UniWii")]
	private static extern void wiimote_stop();
	
	[DllImport ("UniWii")]
	private static extern int wiimote_count();
	
	private String display;
	
	void OnGUI() {
		int c = wiimote_count();
		if (c>0) {
			display = "";
			for (int i=0; i<=c-1; i++) {
				display += "Wiimote " + i + " found!\n";
			}
		}
		else 
			display = "Press the '1' and '2' buttons on your Wii Remote.";
		
		GUI.Label( new Rect(10,Screen.height-100, 500, 100), display);
	}
	
	void Start ()
	{
		wiimote_start();
	}
	
	void OnApplicationQuit() {
		//wiimote_stop();
	}

	void OnApplicationPause(bool pauseStatus) {
		//paused = pauseStatus;
		//wiimote_stop();
	}
	
	
	
}