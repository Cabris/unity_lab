using UnityEngine;
using System.Collections;
using System.Text;
using System.Timers;
using System;
using System.Runtime.InteropServices;

public class EncodeCameraV2 : MonoBehaviour {

	[DllImport("RenderingPlugin", CallingConvention = CallingConvention.Cdecl)]
	private static extern void GetCombinedTexture (IntPtr dataP, out int size);

	[SerializeField]
	TextureSource leftSrc,rightSrc;
	[SerializeField]
	int outWidth,outHeight;
	[SerializeField]
	bool isSameAsSource;
	[SerializeField]
	int fps;
	[SerializeField]
	int bitRate;
	[SerializeField]
	bool isEncoding=false;
	[SerializeField]
	int blocking=0;
	IntPtr srcP;
	EncoderH264V2 encoder;
	StreamTcpServer server;
	long tatol=0;
	System.Timers.Timer timer;
	System.Object obj;


	// Use this for initialization
	void Start () {
		Application.targetFrameRate=-1;
		if(leftSrc==null||rightSrc==null){
			Debug.LogError("null source");
			Application.Quit();
		}

		int inW=leftSrc.Width+rightSrc.Width;
		int inH=(leftSrc.Height+rightSrc.Height)/2;
		if(isSameAsSource){
			outWidth=inW;
			outHeight=inH;
		}

		int src_size = inW * inH * 3;//bgr
		srcP=Marshal.AllocHGlobal (src_size);

		encoder=new EncoderH264V2(srcP,inW,inH,true);
		encoder.OutWidth=outWidth;
		encoder.OutHeight=outHeight;
		encoder.Fps=fps;
		encoder.BitRate=bitRate;
		encoder.Prepare();
		
		server=GetComponent<StreamTcpServer>();
		obj=this;
		
		double interval=1000.0/(double)fps;
		timer=new System.Timers.Timer(interval);
		timer.Elapsed+=Encoding;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F2)){
			isEncoding=!isEncoding;
			if(isEncoding)
				startEncoding();
			if(!isEncoding)
				stopEncoding();
			Debug.Log("isEncoding: "+isEncoding);
		}
	}
	
	public void CheckStatus(System.Object stateInfo)
	{
		Debug.Log("Encoding");
	}
	
	public void startEncoding(){
		leftSrc.StartCapture();
		rightSrc.StartCapture();
		encoder.StartEncoder();
		timer.Start();
		isEncoding=true;
	}
	
	void  Encoding(object source, ElapsedEventArgs e){
		blocking++;
		lock(obj){
			if(isEncoding){
				int size;
				GetCombinedTexture(srcP,out size);
				byte[] encoded= encoder.Encoding();
				server.Send(encoded);//blocking point
				tatol+=encoded.Length;
				//Debug.Log(encoded.Length);
			}
		}
		blocking--;
	}
	
	public void stopEncoding(){
		lock(obj){
			leftSrc.StopCapture();
			rightSrc.StopCapture();
			isEncoding=false;
			timer.Stop();
			timer.Dispose();
			encoder.StopEncoder();
			server.onDestory();
		}
	}
	
	void OnApplicationQuit() {
		if(isEncoding)
			stopEncoding();
		if (srcP != IntPtr.Zero)
			Marshal.FreeHGlobal (srcP);
		Debug.Log("tatol: "+tatol);
	}
}
