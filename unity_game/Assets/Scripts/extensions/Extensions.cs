using System;
using System.IO;
using UnityEngine;
using System.Collections;


public static class Extensions
{
	public static String AssetBundleLoaction{
		get{return "file:///C:/_AssetBundles/{0}.assetBundles";}
	}
	

	
	public static string[] GetFileNames(string filter)
	{
		string path="C:\\_AssetBundles";
		string[] files = Directory.GetFiles(path, filter);
		for(int i = 0; i < files.Length; i++)
			files[i] = Path.GetFileNameWithoutExtension(files[i]);
		return files;
	}
	
}


