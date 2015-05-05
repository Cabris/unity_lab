package com.myapp.orientationService;

import com.myapp.orientationService.funcs.SensorClient;
import com.simpleMessage.sender.MessageSender;

import android.R.string;
import android.app.Activity;
import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.util.Log;

public class OrientationService extends Service {

	private MyBinder mBinder = new MyBinder();
	SensorClient sClient;
	MessageSender sender;
	Activity activity;
	String adrs;
	int port2;

	@Override
	public IBinder onBind(Intent intent) {
		return mBinder;
	}

	@Override
	public void onCreate() {
		super.onCreate();
		Log.d("OrientationService", "onCreate"); 
	}

	@Override
	public void onDestroy() {
		super.onDestroy();
		if (sClient != null)
			sClient.onStop();
		if (sender != null)
			sender.onStop();
		Log.d("OrientationService", "onDestroy"); 
	}

	class MyBinder extends Binder {

		public void initialize(Activity act, String a, int p) {
			adrs = a;
			port2 = p;
			activity=act;
			sender = new MessageSender(adrs, port2);
			sClient = new SensorClient(activity, sender);
			sender.connect();
			sClient.onStart();
			Log.d("OrientationService", "initialize: "+adrs+":"+port2); 
		}

	}

}
