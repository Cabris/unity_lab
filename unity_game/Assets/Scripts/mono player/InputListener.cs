using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class InputListener: MonoBehaviour
{
	public  float Horizontal;
	public  float Vertical;
	static Dictionary<string,bool> topButtonStates=new Dictionary<string, bool>();
	static Dictionary<string,bool> preButtonStates=new Dictionary<string, bool>();
	static Dictionary<string,bool> buttonStates=new Dictionary<string, bool>();

	void LateUpdate(){

		preButtonStates.Clear();
		foreach(string b in buttonStates.Keys)
			preButtonStates.Add(b,buttonStates[b]);

		buttonStates.Clear();
		foreach(string b in topButtonStates.Keys)
			buttonStates.Add(b,topButtonStates[b]);
	}


	public void SetButtonValue(string b,bool p){
		if(!topButtonStates.ContainsKey(b))
			topButtonStates.Add(b,p);
		else
			topButtonStates[b]=p;
	}

	public static bool GetButtonDown(string b){
		if(!buttonStates.ContainsKey(b))
			return false;
		if(!preButtonStates.ContainsKey(b))
			return false;

		if(!preButtonStates[b]&&buttonStates[b])
			return true;
		else return false;
	}

	public static bool GetButtonUp(string b){
		if(!buttonStates.ContainsKey(b))
			return false;
		if(!preButtonStates.ContainsKey(b))
			return false;

		if(preButtonStates[b]&&!buttonStates[b])
			return true;
		else return false;
	}

	public static bool GetButton(string b){
		if(topButtonStates.ContainsKey(b))
			return topButtonStates[b];
		else
			return false;
	}

}


