﻿using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

public class TextureSource : MonoBehaviour
{
	
	[DllImport ("RenderingPlugin")]
	private static extern void CreateTextureCapt (int id, IntPtr texture);
	
	[DllImport("RenderingPlugin")]
	private static extern void StartCapt (int id);
	
	[DllImport("RenderingPlugin")]
	private static extern void StopCapt (int id);
	
	[DllImport("RenderingPlugin")]
	private static extern void getTexture (int id, IntPtr dataP, out int size);
	
	[SerializeField]
	Camera listeningCamera;
	bool isCapt = false;
	
	public IntPtr srcP{ get; private set; }
	
	//[SerializeField]
	//Renderer r;

	[SerializeField]
	RenderTexture tex;
	System.Object obj;
	public int Id;

	void Awake(){
		Width = tex.width;
		Height = tex.height;
		obj = this;
		srcP = IntPtr.Zero;
	}

	// Use this for initialization
	void Start ()
	{
		try{
			CreateTextureAndPassToPlugin ();
		}catch(Exception e){
			Debug.LogException(e);
		}
	}
	
	private void CreateTextureAndPassToPlugin ()
	{
		listeningCamera.targetTexture = tex as RenderTexture;

	}
	
	public void StartCapture ()
	{
		lock (obj) {
			IntPtr tp=tex.GetNativeTexturePtr ();
			if(tp==IntPtr.Zero)
				throw new Exception("y r u null!!!!????");
			CreateTextureCapt (Id, tp);
			isCapt = true;
			StartCapt (Id);
			StartCoroutine ("CallPluginAtEndOfFrames");
		}
	}
	
	public void StopCapture ()
	{
		lock (obj) {
			isCapt = false;
			StopCapt (Id);
			StopCoroutine ("CallPluginAtEndOfFrames");
			if (srcP != IntPtr.Zero)
				Marshal.FreeHGlobal (srcP);
			srcP=IntPtr.Zero;
		}
	}
	
	public void GetTexture ()
	{
		int size = 0;
		if (isCapt) {
			lock (obj) {
				getTexture (Id, srcP, out size);
			}
		}
	}
	
	private IEnumerator CallPluginAtEndOfFrames ()
	{
		while (true) {
			if (isCapt) {
				yield return new WaitForEndOfFrame ();
				GL.IssuePluginEvent (Id);
			}//else break;	
		}
	}
	
	public void CreatePointer ()
	{
		int src_size = Width * Height * 3;//bgr
		srcP = Marshal.AllocHGlobal (src_size);
	}
	
	public int Width {
		get;
		protected set;
	}
	
	public int Height {
		get;
		protected set;
	}
	
	void OnApplicationQuit ()
	{
		//StopCapture ();
	}
}