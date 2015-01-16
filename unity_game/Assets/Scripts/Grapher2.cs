using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Grapher2 : MonoBehaviour {

	public Rigidbody rb;
	int resolution=1000;
	private ParticleSystem.Particle[] points;
	public List<Vector3> Datas{get;private set;}
	
	void Start () {
		transform.parent=null;
		transform.position=Vector3.zero;
		transform.localScale=new Vector3(1,1,1);
		transform.rotation=Quaternion.identity;
		Datas=new List<Vector3>();
		StartLog();
	}
	
	private void CreatePoints () {
//		currentResolution = resolution;
		points = new ParticleSystem.Particle[resolution];
		int i = 0;
		for (int x = 0; x < resolution; x++) {
			Vector3 p = new Vector3(0, 0f, 0);
			points[i].position = p;
			points[i].color =particleSystem.startColor;
			points[i++].size = 0.03f;
		}
	}
	
	void Update () {

	}
	
	public void StartLog(){
		CreatePoints ();
		isStop=false;
		StartCoroutine(showGrapher(.1f));
	}
	
	public void StopLog(){
		isStop=true;
		StopCoroutine("showGrapher");
	}
	
	public void Reset(){
		isStop=true;
		StopCoroutine("showGrapher");
		particleSystem.SetParticles(points, 0);
		Datas.Clear();
	}
	
	public void PauseLog(){}
	public void ResumeLog(){}
	
	bool isStop=false;
	IEnumerator showGrapher(float interval){
		Datas.Clear();
		for(int i=0;!isStop;i++){
			Datas.Add(rb.position);
			if(Datas.Count>=points.Length){
				StopLog();
				break;
			}
			if(Datas.Count>0)
				points[Datas.Count-1].position=rb.position;
			particleSystem.SetParticles(points, Datas.Count);
			if(isStop)
				break;
			yield return new WaitForSeconds(interval);
		}
		yield return null;
	}

}
