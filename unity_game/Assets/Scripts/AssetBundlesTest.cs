using UnityEngine;
using System.Collections;
using System;
using System.IO;


public class AssetBundlesTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine(LoadGameObject());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator LoadGameObject(){
		
		// AssetBundle 檔案路徑
		string path = string.Format("file://{0}/../_AssetBundles/{1}.assetBundles" , 
		                            Application.dataPath , "Wall");
		
		//  載入 AssetBundle
		WWW bundle = new WWW(path);
		
		//等待載入完成
		yield return bundle;
		
		//實例化 GameObject 並等待實作完成
		yield return Instantiate(bundle.assetBundle.mainAsset);
		
		//卸載 AssetBundle
		bundle.assetBundle.Unload(false);
	}

}
