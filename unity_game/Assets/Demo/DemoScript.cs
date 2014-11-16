using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoScript : MonoBehaviour {
	[SerializeField]
	List<GameObject> gs;
	int index=0;
	[SerializeField]
	Transform UIp;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.J)&&index<gs.Count-1)
		{
			index++;
		}
		for(int i =0;i<gs.Count;i++)
		{
			gs[i].SetActive(i==index);
		}
		if(index==gs.Count-1){
			UIp.transform.localScale=new Vector3(1,1,1);
		}

	}


}
