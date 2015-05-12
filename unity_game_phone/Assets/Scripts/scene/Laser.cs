using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class Laser : MonoBehaviour {
	LineRenderer lineRenderer;
	public Vector3 start;
	public Vector3 direction;
	public float distanceMax;
	[SerializeField]
	float distance;
	public RaycastHit hit{get; private set;}
	public bool isHit{get;private set;}
	int layerMask=(1<<8)|(1<<11);
	// Use this for initialization
	void Start () {
		lineRenderer= renderer as LineRenderer;
		lineRenderer.SetVertexCount(2);
	}
	
	// Update is called once per frame
	void Update () {
		lineRenderer.SetPosition(0,start);
		TestIfHit();
		Vector3 end=start+direction*distance;
		lineRenderer.SetPosition(1,end);
	}

	public bool TestIfHit(){
		distance=distanceMax;
		Ray ray=new Ray(start,direction);
		Debug.DrawRay(ray.origin, direction*distance, Color.red);
		
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit,distanceMax,layerMask)){
			distance= hit.distance;
			this.hit=hit;
			isHit=true;
		}else
			isHit=false;
		return isHit;
	}












}
