using System;
using UnityEngine;
using System.Collections.Generic;

public class SelectController: MonoBehaviour
{

	[SerializeField]
	float offsetValue=1.3f;
	[SerializeField]
	Material outline;
	[SerializeField]
	GameObject PanelPrefab;
	Dictionary<GameObject,PanelController> selections=new Dictionary<GameObject, PanelController>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(PanelController p in selections.Values){

		}
	}

	public void onSelect(GameObject obj){
		if(!selections.ContainsKey(obj)){
			select(obj);
		}else
			OnUnselect(obj);
	}
	
	public void OnUnselect(GameObject obj){
		if(!selections.ContainsKey(obj))
			return;
		GameObject p=selections[obj].transform.parent.gameObject;
		GameObject.Destroy(p);
		
		Renderer r=obj.GetComponent<Renderer>();
		Material[] ms=r.materials;//add outline to  first
		Material[] newMs=new Material[ms.Length-1];
		Array.Copy(ms,1,newMs,0,ms.Length-1);
		r.materials=newMs;
		selections.Remove(obj);
	}

	void select(GameObject obj){
		Camera c=Camera.main;
		Vector3 panelPos=c.transform.position;
		Vector3 offset= c.transform.rotation * Vector3.forward;
		panelPos+=offset*offsetValue;

		GameObject panel=GameObject.Instantiate(PanelPrefab) as GameObject;
		PanelController panelControl=panel.GetComponent<PanelController>();
		panelControl.Init(this,obj);
		panelControl.actualTransform.position=panelPos;
		selections.Add(obj,panelControl);

		Renderer r=obj.GetComponent<Renderer>();
		Material[] ms=r.materials;//add outline to  first
		Material[] newMs=new Material[ms.Length+1];
		newMs[0]=outline;
		Array.Copy(ms,0,newMs,1,ms.Length);
		r.materials=newMs;
	}



	public void onUnselectAll(){
		foreach(GameObject selectObj in selections.Keys){
			OnUnselect(selectObj);
		}
		selections.Clear();
	}

}


