using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class Grapher1 : MonoBehaviour {
	public Rigidbody rb;
	[SerializeField]
	Transform axisX,axisY;
	[Range(10, 100)]
	public int resolution = 10;
	private delegate Vector3 FunctionDelegate (Rigidbody x);
	private int currentResolution;
	private ParticleSystem.Particle[] points;
	private Vector3 iniX;
	private Vector3 preV;
	public enum FunctionOption {
		Vt,
		Xt,
		At
	}
	
	private FunctionDelegate[] functionDelegates;
	
	public FunctionOption function;

	public List<float> Datas{get;private set;}

	void Start () {
		functionDelegates=new FunctionDelegate[3];
		functionDelegates[0]=funcVt;
		functionDelegates[1]=funcXt;
		functionDelegates[2]=funcAt;
		Datas=new List<float>();
	}
	
	private void CreatePoints () {
		currentResolution = resolution;
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
		if (currentResolution != resolution || points == null) {
			//CreatePoints();
		}
		//FunctionDelegate f = functionDelegates[(int)function];
		//for (int i = 0; i < points.Length; i++) {
	}
	
	public void StartLog(){
		CreatePoints ();
		isStop=false;
		iniX=rb.transform.position;
		preV=rb.velocity;
		StartCoroutine(showGrapher(.01f));
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
		float lengthX=axisX.lossyScale.y*2f;
		float lengthY=axisY.lossyScale.y*2f;
		FunctionDelegate f = functionDelegates[(int)function];
		Datas.Clear();
		for(int i=0;i<points.Length;i++){
			//Debug.Log(i);
			Datas.Add(f(rb).magnitude);
			float increment = lengthX / (float)(i+1);
			float max=Datas.Max(),min=Datas.Min();
			if(Datas.Count>0&&max-min>0)
			for(int j=0;j<=i;j++){
				Vector3 p1 = points[j].position;
				p1.x=j*increment;
				p1.y=(Datas[j]-min)*lengthY*0.75f/(max-min);
				points[j].position = p1;
			}
			particleSystem.SetParticles(points, i);
			if(isStop)
				break;
			yield return new WaitForSeconds(interval);
		}
		yield return null;
	}
	
	private Vector3 funcVt (Rigidbody b) {
		return b.velocity;
	}
	
	private  Vector3 funcAt (Rigidbody b) {
		Vector3 dV=b.velocity-preV;
		preV=b.velocity;
		Vector3 a=dV/.01f;
		return a;
	}
	
	private Vector3 funcXt (Rigidbody b){
		return b.transform.position-iniX;
	}
	
}
