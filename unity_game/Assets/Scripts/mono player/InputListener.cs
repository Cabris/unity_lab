using UnityEngine;
using System.Collections;
using System;

public class InputListener: MonoBehaviour
{
	public  float Horizontal;
	public  float Vertical;

	//public bool ButtonBPress{get;set;}

	public virtual void OnKeyPress(KeyCode k){}
	public virtual void OnKeyUp(KeyCode k){}

}


