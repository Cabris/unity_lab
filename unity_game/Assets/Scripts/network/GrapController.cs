using UnityEngine;
using System.Collections;

public class GrapController : MonoBehaviour {
	public GameObject detectObj;
	private GameObject grapObj;
	public Transform graper;
	bool isGrap;
	private Transform _parent;
	private Vector3 graperPos;
	// Use this for initialization
	void Start () {
		isGrap=false;
		ScenePlayerCommand p=GetComponent<ScenePlayerCommand>();
		p.OnButtonDown+=this.onBd;
		p.OnButtonUp+=this.onBu;
		graperPos=new Vector3();
	}
	
	// Update is called once per frame
	void LateUpdate () {
		graperPos=graper.transform.position;

	}

	void Update(){
		if(isGrap&&grapObj!=null){
			grapObj.transform.position=graperPos;
		}
	}
	
	public	void onBd(string button){
		if(button=="buttonB"&&detectObj!=null){
			grapObj=detectObj;
			//_parent=grapObj.transform.parent;
			//grapObj.transform.parent=graper;
			isGrap=true;
		}
	}
	public	void onBu(string button){
		if(button=="buttonB"){
			if(grapObj!=null){
			//	grapObj.transform.parent=_parent;
			}
			grapObj=null;
			isGrap=false;
		}
	}
	
}
