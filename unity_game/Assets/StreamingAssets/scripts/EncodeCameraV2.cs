using UnityEngine;
using System.Collections;
using System.Text;
using System.Timers;
using System;
using System.Threading;
using System.Runtime.InteropServices;

public class EncodeCameraV2 : MonoBehaviour {

	[DllImport("RenderingPlugin", CallingConvention = CallingConvention.Cdecl)]
	private static extern void GetCombinedTexture (IntPtr dataP, out int size);

	[DllImport("RenderingPlugin", CallingConvention = CallingConvention.Cdecl)]
	private static extern void SetCallback(Callback fn);

	private delegate int Callback(string text);
	private Callback mInstance;   // Ensure it doesn't get garbage collected
	
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
	System.Diagnostics.Stopwatch stopWatch;
	[SerializeField]
	bool outputFile=false;

	Thread thread;

	private int Handler(string text) {
		// Do something...
		Debug.Log(text);
		return 42;
	}
	
	
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

		int src_size = inW * inH * 4;//rgba
		srcP=Marshal.AllocHGlobal (src_size);

		encoder=new EncoderH264V2(srcP,inW,inH,outputFile);
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

		stopWatch = new System.Diagnostics.Stopwatch();
//		mInstance = new Callback(Handler);
//		SetCallback(mInstance);
//		mInstance+=Handler;
		//thread=new Thread(new ThreadStart(encodeLoop));
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
		//StartCoroutine ("CallDoEncoding");
		timer.Start();
		isEncoding=true;
		//thread.Start();
	}

//	private IEnumerator CallDoEncoding ()
//	{
//		while (true) {
//			yield return new WaitForEndOfFrame ();
//			doEncoding();
//		}
//	}

	void encodeLoop(){
		while(isEncoding){
			double interval=1000.0/(double)fps;
			Thread.Sleep((int)interval);
			doEncoding();
		}
	}

	void  doEncoding ()
	{
		blocking++;
		lock (obj) {
			if (isEncoding) {
				int size;
				WatchStart ();
				GetCombinedTexture (srcP, out size);
				WatchStop ("GetCombinedTexture");
				WatchStart ();
				byte[] encoded = encoder.Encoding ();
				WatchStop ("Encoding");
				WatchStart ();
				server.Send (encoded);
				tatol += encoded.Length;
				WatchStop ("Send");
				//Debug.Log(encoded.Length);
			}
		}
		blocking--;
	}
	
	void  Encoding(object source, ElapsedEventArgs e){
		doEncoding ();
	}
	
	public void stopEncoding(){
		lock(obj){
			leftSrc.StopCapture();
			rightSrc.StopCapture();
			isEncoding=false;
			//StopCoroutine ("CallDoEncoding");
			timer.Stop();
			//thread.Join();
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

	void WatchStart ()
	{
//		stopWatch.Reset ();
//		stopWatch.Start ();
	}

	void WatchStop (string lable)
	{
//		stopWatch.Stop ();
//		TimeSpan ts = stopWatch.Elapsed;
//		string elapsedTime = String.Format ("{0:00}", ts.Milliseconds);
//		UnityEngine.Debug.Log ("RunTime_"+lable+": " + elapsedTime);
	}
}
