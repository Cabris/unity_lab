using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DemoScript : MonoBehaviour {
	[SerializeField]
	//List<GameObject> gs;
	int index=0;
	//[SerializeField]
	//Transform UIp;
	[SerializeField]
	Renderer r;
	[SerializeField]
	GameObject ui;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(index>21)
			ui.SetActive(true);
		else
			ui.SetActive(false);

		if(Input.GetKeyDown(KeyCode.J))
		{
			index++;
			if(index<=21)
			r.material.mainTexture = Resources.Load("demo ("+index+")", typeof(Texture2D)) as Texture;
		}
		if(Input.GetKeyDown(KeyCode.M))
		{
			index++;
			if(index<=21)
				r.material.mainTexture = Resources.Load("demo ("+index+")", typeof(Texture2D)) as Texture;
		}
//		for(int i =0;i<gs.Count;i++)
//		{
//			gs[i].SetActive(i==index);
//		}
//		if(index==gs.Count-1){
//			UIp.transform.localScale=new Vector3(1,1,1);
//		}




	}


}
