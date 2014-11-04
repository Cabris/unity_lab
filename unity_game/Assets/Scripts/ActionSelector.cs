using UnityEngine;

using System.Collections;

public class ActionSelector: MonoBehaviour
{
	public ActionType actionTypeMask = 0;

	void Start () {

	}

	void Update () {
		
	}
	
	public enum ActionType
	{
		fall    = 1,
		Air        = 2,
		Water    = 4,
		Wall    = 8,
		//etc    = 16,
		//etc    = 32,
	}

}
