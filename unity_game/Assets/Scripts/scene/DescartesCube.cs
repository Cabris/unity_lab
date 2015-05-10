using UnityEngine;
using System.Collections;

public class DescartesCube : MonoBehaviour {
	[SerializeField]
	TypogenicText text;
	public Vector2 targetPosition;
	[SerializeField]
	TypogenicText[] temp; 
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		Vector2	pos=new Vector2(Extensions.toInt(transform.localPosition.x),
		                        Extensions.toInt(transform.localPosition.z));
		foreach(TypogenicText t in temp)
			t.Text=pos.x+","+pos.y;


		string textStr=@"\0"+targetPosition.x+@"\1,\2"+targetPosition.y;
		text.Text=textStr;
	}
}
