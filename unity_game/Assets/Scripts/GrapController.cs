using UnityEngine;
using System.Collections;

public class GrapController : MonoBehaviour {
	public GameObject detectObj;
	private GameObject grapObj;
	public Transform target;
	bool isGrap;
	private Transform _parent;
	// Use this for initialization
	void Start () {
		isGrap=false;
		ScenePlayerCommand p=GetComponent<ScenePlayerCommand>();
		p.OnButtonDown+=this.onBd;
		p.OnButtonUp+=this.onBu;
	}
	
	// Update is called once per frame
	void Update () {
		if(isGrap&&grapObj!=null){
			//grapObj.transform.position=target.position;
		}
	}
	
	public	void onBd(string button){
		if(button=="buttonB"&&detectObj!=null){
			grapObj=detectObj;
			_parent=grapObj.transform.parent;
			grapObj.transform.parent=target;
			isGrap=true;
		}
	}
	public	void onBu(string button){
		if(button=="buttonB"){
			if(grapObj!=null){
				grapObj.transform.parent=_parent;
			}
			grapObj=null;
			isGrap=false;
		}
	}
	
}
