using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LaserGun : MonoBehaviour {
	[SerializeField]
	Laser laserPrefab;
	[SerializeField]
	List<Laser> lasers;
	[SerializeField]
	int maxReflect=3;
	[SerializeField]
	float updateReflectPeriod;
	float count=0;
	
	// Use this for initialization
	void Start () {
		lasers=new List<Laser>();
		GameObject iniLaserGo=Instantiate(laserPrefab.gameObject) as GameObject;
		iniLaserGo.transform.parent=transform;
		Laser iniLaser=iniLaserGo.GetComponent<Laser>();
		lasers.Add(iniLaser);

		for(int i=0;i<maxReflect;i++){
			Laser laser=InstantiateLaser(Vector3.zero,Vector3.zero);
			//laser.gameObject.SetActive(false);
			lasers.Add(laser);
		}

	}



	// Update is called once per frame
	void LateUpdate () {
		if(count>=updateReflectPeriod){
			count=0;
			UpdateReflect();
		}else
			count+=Time.deltaTime;
		//UpdateReflect();
	}

	void UpdateReflect(){
		for(int i=1;i<lasers.Count;i++){
			lasers[i].direction=Vector3.zero;
		}

		Laser startLaser=lasers[0];
		startLaser.start=transform.position;
		startLaser.direction=transform.forward.normalized;
		
		Laser laserIn=startLaser;
		for(int i=0;i<maxReflect;i++){
			if(laserIn.TestIfHit()){
				RaycastHit hit=laserIn.hit;
				Vector3 hitPos=hit.point;
				Vector3 directionIn=laserIn.direction;
				Vector3 normal=hit.normal.normalized;
				Vector3 directionOut=Vector3.Reflect(directionIn,normal).normalized;
				laserIn=lasers[1+i];
				laserIn.start=hitPos;
				laserIn.direction=directionOut;
			//	Debug.Log(hit.collider.gameObject.name);
				//laserIn.gameObject.SetActive(true);
			}
			else
				break;
		}
	}

	Laser InstantiateLaser(Vector3 start,Vector3 direction){
		GameObject laserGo=GameObject.Instantiate(laserPrefab.gameObject) as GameObject;
		laserGo.transform.parent=transform;
		Laser laser=laserGo.GetComponent<Laser>();
		laser.start=start;
		laser.direction=direction;
		return laser;
	}
}
