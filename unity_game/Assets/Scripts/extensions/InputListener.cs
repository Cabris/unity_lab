using UnityEngine;
using System.Collections;
using System;

public class InputListener: MonoBehaviour
{
	public  float Horizontal{get;set;}
	public  float Vertical{get;set;}
	//public bool ButtonBPress{get;set;}

	public virtual void OnKeyPress(KeyCode k){}
	public virtual void OnKeyUp(KeyCode k){}

}


