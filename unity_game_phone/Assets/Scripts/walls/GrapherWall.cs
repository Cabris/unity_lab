using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
public class GrapherWall : MonoBehaviour {
	[SerializeField]
	GameObject prefab;
	List<GameObject> bars=new List<GameObject>();
	// Use this for initialization
	void Start () {
		for(int i=0;i<21;i++){
			GameObject g=GameObject.Instantiate(prefab) as GameObject;
			g.transform.parent=transform;
			g.transform.localScale=new Vector3(1,1,1);
			g.transform.localPosition=new Vector3(i-10,0,0);
			bars.Add(g);
		}
		prefab.SetActive(false);
	}
	
	public void SetData(float[] datas){
		if(datas.Length==0){
			for(int i=0;i<21;i++){
				Vector3 p=new Vector3(i-10,0,0);
				TweenPosition.Begin(bars[i],.1f,p);
			}
			return;
		}
		float maxY=8;
		float dataMin=datas.Min(),dataMax=datas.Max();
		float dec=dataMax-dataMin;
		float increase=((float)datas.Length)/21f;
		if(dec>0)
		for(int i=0;i<21;i++){
			int index=(int)(i*increase);
			float v0=datas[index];
			float v=(v0-dataMin)*maxY/dec;
			Vector3 p=new Vector3(i-10,Extensions.toInt(v),0);
			TweenPosition.Begin(bars[i],.1f,p);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
}
