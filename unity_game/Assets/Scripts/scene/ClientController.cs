using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ClientController : MonoBehaviour {
	public static string AssetName{get;set;} 
	private bool isLoaded=false;

	// Use this for initialization
	void Start () {
		if(AssetName!=null&&AssetName.Length>0){
			Coroutine co= StartCoroutine(LoadGameObject(AssetName));
		}
	}
	
	private IEnumerator LoadGameObject(string assetName){
		WWW bundle=null;
		string path = string.Format(Extensions.AssetBundleLoaction, assetName);
		Debug.Log("p:"+path);
		try{
			//loading assetbundle
			bundle = new WWW(path);
		}
		catch(Exception e){
			Debug.LogException(e);
		}
			//wait for loaded
			yield return bundle;
			
			//Instantiate GameObject wait
			GameObject scene=UnityEngine.Object.Instantiate(bundle.assetBundle.mainAsset) as GameObject;
			Debug.Log("loaded: "+path);
			yield return scene;
			
			Debug.Log("client loaded from asset: "+AssetName);
			//Destroy(scene);
			isLoaded=true;
			bundle.assetBundle.Unload(false);
	}
	

	void Update () {

	}
	
}
