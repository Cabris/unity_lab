package com.myapp.h264streamingviwer;

import java.util.logging.Logger;

import javax.microedition.khronos.egl.EGLConfig;

import com.myapp.h264streamingviwer.R;
import com.google.vrtoolkit.cardboard.CardboardActivity;
import com.google.vrtoolkit.cardboard.CardboardView;
import com.google.vrtoolkit.cardboard.Eye;
import com.google.vrtoolkit.cardboard.HeadTransform;
import com.google.vrtoolkit.cardboard.Viewport;
import com.myapp.h264streamingviwer.funcs.Decoder;
import com.simpleMessage.sender.MessageSender;
import com.stream.source.StreamReceiver;

import android.app.Fragment;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.View;
import android.view.ViewGroup;

public class VideoFragment extends Fragment implements SurfaceHolder.Callback, IHandleVideoSize,CardboardView.StereoRenderer{
	//private static Logger log = Logger.getLogger(MessageSender.class.getName());

	protected String ip;
	protected int port;
	protected Decoder decoder;
	protected StreamReceiver receiver;
	protected SurfaceHolder holder;
	protected CardboardActivity activity;

	public VideoFragment(CardboardActivity a,String ip, int port) {
		this.ip = ip;
		this.port = port;
		activity=a;
	}

	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
		View view = inflater.inflate(R.layout.video_surface, container, false);
		return view;
	}

	@Override
	public void onActivityCreated(Bundle savedInstanceState) {
		super.onActivityCreated(savedInstanceState);
		createVideoHolder();
		receiver = new StreamReceiver(ip, port);
		decoder = new Decoder(receiver);
		decoder.videoSize=this;
	}

	protected void createVideoHolder() {
		SurfaceView surfaceView = (SurfaceView) getView().findViewById(R.id.surface);
		holder=surfaceView.getHolder();
		holder.addCallback(this);
		
		 CardboardView cardboardView = (CardboardView) 
				 getView().findViewById(R.id.cardboardView);;
		    // Associate a CardboardView.StereoRenderer with cardboardView.
		 cardboardView.setRenderer(this);
		    // Associate the cardboardView with this activity.
		 activity.setCardboardView(cardboardView);
		
	}

	@Override
	public void onStart() {
		super.onStart();
		receiver.Connect();
		
	}
	
	@Override
	public void onDestroy() {
		Log.d("VideoFragment", "onDestroy");
		super.onStop();
		if (decoder != null)
			decoder.onStop();
		if (receiver != null)
			receiver.onStop();
	}

	@Override
	public void surfaceCreated(SurfaceHolder holder) {
		Log.d("VideoFragment", "surfaceCreated");
		decoder.onCreate(holder);
	}

	@Override
	public void surfaceChanged(SurfaceHolder holder, int format, int width,
			int height) {
		Log.d("VideoFragment", "surfaceChanged");
	}

	@Override
	public void surfaceDestroyed(SurfaceHolder holder) {
		Log.d("VideoFragment", "surfaceDestroyed");
	}

	@Override
	public void handleVideoSize(final int videoWidth, final int videoHeight) {
		Log.d("Decoder", "New format :" + videoWidth + ", " + videoHeight);
		final SurfaceView surfaceView = (SurfaceView) getView().findViewById(R.id.surface);
		Handler mainHandler = new Handler(surfaceView.getContext().getMainLooper());

		Runnable runnable = new Runnable() {

			@Override
			public void run() {
				float videoProportion = (float) videoWidth / (float) videoHeight;

				android.view.ViewGroup.LayoutParams lp = surfaceView.getLayoutParams();
				int screenWidth = lp.width;
				int screenHeight = lp.height;
				float screenProportion = (float) screenWidth / (float) screenHeight;

				if (videoProportion > screenProportion) {
					lp.width = screenWidth;
					lp.height = (int) ((float) screenWidth / videoProportion);
				} else {
					lp.width = (int) (videoProportion * (float) screenHeight);
					lp.height = screenHeight;
				}
				surfaceView.setLayoutParams(lp);
			}
		};
	}

	@Override
	public void onDrawEye(Eye arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onFinishFrame(Viewport arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onNewFrame(HeadTransform arg0) {
		// TODO Auto-generated method stub
		float[] d=new float[3];
		arg0.getEulerAngles(d, 0);
		Log.d("VideoFragment","getEulerAngles: " +d.toString());
	}

	@Override
	public void onRendererShutdown() {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onSurfaceChanged(int arg0, int arg1) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onSurfaceCreated(EGLConfig arg0) {
		// TODO Auto-generated method stub
		
	}
}
