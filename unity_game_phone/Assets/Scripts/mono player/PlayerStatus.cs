using UnityEngine;
using System.Collections;
using System;

public class PlayerStatus : MonoBehaviour {

	public SkinnedMeshRenderer smr;
	public Color color;
	public string userName;
	// Use this for initialization
	void Start () {
		smr=GetComponentInChildren<SkinnedMeshRenderer>();
		//color=Color.cyan;
	}
	
	// Update is called once per frame
	void Update () {
		smr.material.color=color;
	}

}
