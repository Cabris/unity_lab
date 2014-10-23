using System;
using UnityEngine;
using System.Collections.Generic;

public class SelectController: MonoBehaviour
{
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
		
	}

	public void onSelect(GameObject obj){
		if(!selections.ContainsKey(obj)){
			select(obj);
		}
	}
	
	public void onUnselectAll(){
		foreach(GameObject selectObj in selections.Keys){
			unselect(selectObj);
		}
		selections.Clear();
	}

	Vector3 panelOffset=new Vector3(0,1,0);

	void select(GameObject obj){
		GameObject _p=GameObject.Instantiate(PanelPrefab) as GameObject;
		_p.transform.position=obj.transform.position+panelOffset;
		_p.GetComponent<CameraFacingBillboard>().Init();
		PanelController panelControl=_p.GetComponent<PanelController>();
		panelControl.owner=obj;
		selections.Add(obj,panelControl);

		Renderer r=obj.GetComponent<Renderer>();
		Material[] ms=r.materials;//add outline to  first
		Material[] newMs=new Material[ms.Length+1];
		newMs[0]=outline;
		Array.Copy(ms,0,newMs,1,ms.Length);
		r.materials=newMs;
	}

	void unselect(GameObject obj){
		GameObject p=selections[obj].transform.parent.gameObject;
		GameObject.Destroy(p);

		Renderer r=obj.GetComponent<Renderer>();
		Material[] ms=r.materials;//add outline to  first
		Material[] newMs=new Material[ms.Length-1];
		Array.Copy(ms,1,newMs,0,ms.Length-1);
		r.materials=newMs;

	}

}


