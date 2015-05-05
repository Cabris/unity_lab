package com.myapp.orientationService.funcs;

import com.google.vrtoolkit.cardboard.HeadTransform;
import com.google.vrtoolkit.cardboard.sensors.HeadTracker;

import android.content.Context;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorManager;
import android.util.Log;

public class OrientationSensor extends BaseSensor {
	private HeadTracker mHeadTracker; 
	protected HeadTransform mHeadTransform;
	protected float[] mHeadViewMatrix;
	//protected Matrix4 mHeadViewMatrix4;
	//private Quaternion mCameraOrientation;
	
	
	public OrientationSensor(Context context, IOrientationChange orientationChange) {
		super(context, orientationChange);
		mHeadTracker =  HeadTracker.createFromContext (context);
		mHeadTracker.setNeckModelEnabled(true);
		mHeadTracker.setGyroBiasEstimationEnabled(true);
		mHeadViewMatrix = new float[16];
	}

	@Override
	public void strat() {
		super.strat();
		mHeadTracker.startTracking();
		mSensorManager.registerListener(this, mSensorManager.getDefaultSensor(Sensor.TYPE_ROTATION_VECTOR),
				SensorManager.SENSOR_DELAY_GAME);
	}

	@Override
	public void destroy() {
		mHeadTracker.stopTracking();
		mSensorManager.unregisterListener(this);
		super.destroy();
	}

	@Override
	public void onSensorChanged(SensorEvent event) {
		float[] values=new float[4];
		SensorManager.getQuaternionFromVector(values, event.values.clone());
		//float mOrientationData[] = new float[3];
		//calcOrientation(mOrientationData, event.values.clone());
		
		
//		mHeadTracker.getLastHeadView(mHeadViewMatrix, 0);
//		float[] dd=new float[3];
//		SensorManager.getOrientation(mHeadViewMatrix,dd);
//		
//		final float rad2deg = (float)(180.0 / Math.PI);
//		for (int i = 0; i < dd.length; i++) {
//			dd[i]*=rad2deg;
//		}
//		Log.d("OrientationSensor","dd: " +dd[0]+", " +dd[1]+", " +dd[2]);
		this.orientationChange.onOrientationChange(values);
	}

	private void calcOrientation(float[] orientation, float[] incomingValues) {
	    // Get the quaternion
	    float[] quatF = new float[4];
	    SensorManager.getQuaternionFromVector(quatF, incomingValues);

	    // Get the rotation matrix
	    float[][] rotMatF = new float[3][3];
	    rotMatF[0][0] = quatF[1]*quatF[1] + quatF[0]*quatF[0] - 0.5f;
	    rotMatF[0][1] = quatF[1]*quatF[2] - quatF[3]*quatF[0];
	    rotMatF[0][2] = quatF[1]*quatF[3] + quatF[2]*quatF[0];
	    rotMatF[1][0] = quatF[1]*quatF[2] + quatF[3]*quatF[0];
	    rotMatF[1][1] = quatF[2]*quatF[2] + quatF[0]*quatF[0] - 0.5f;
	    rotMatF[1][2] = quatF[2]*quatF[3] - quatF[1]*quatF[0];
	    rotMatF[2][0] = quatF[1]*quatF[3] - quatF[2]*quatF[0];
	    rotMatF[2][1] = quatF[2]*quatF[3] + quatF[1]*quatF[0];
	    rotMatF[2][2] = quatF[3]*quatF[3] + quatF[0]*quatF[0] - 0.5f;

	    // Get the orientation of the phone from the rotation matrix
	    final float rad2deg = (float)(180.0 / Math.PI);
	    orientation[0] = (float)Math.atan2(-rotMatF[1][0], rotMatF[0][0]) * rad2deg;
	    orientation[1] = (float)Math.atan2(-rotMatF[2][1], rotMatF[2][2]) * rad2deg;
	    orientation[2] = (float)Math.asin ( rotMatF[2][0])                * rad2deg;
	    if (orientation[0] < 0) orientation[0] += 360;
	}
	
}
