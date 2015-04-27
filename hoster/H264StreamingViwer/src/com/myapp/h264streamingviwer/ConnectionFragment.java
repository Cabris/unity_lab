package com.myapp.h264streamingviwer;

import android.app.Activity;
import android.app.Fragment;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.content.pm.PackageManager;
import android.os.Bundle;
import android.os.IBinder;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.Toast;

import com.example.h264streamingviwer.R;

class ConnectionFragment extends Fragment implements OnClickListener {

	EditText ipAddressText;
	EditText portText;
	IOnConnectedListener connectedListener;
    Spinner typeSpinner;
	String layoutType;
	String port2Str="8887";
	Intent intent;
	OrientationService.MyBinder binder;
	ServiceConnection connection;
	
	public ConnectionFragment() {
	}

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		intent = new Intent();
	}
	
	@Override
	public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
		View view = inflater.inflate(R.layout.connection_main, container, false);
		return view;
	}

	@Override
	public void onActivityCreated(final Bundle savedInstanceState) {
		super.onActivityCreated(savedInstanceState);
		typeSpinner=(Spinner) getView().findViewById(R.id.typeSpinner);
		ipAddressText = (EditText) getView().findViewById(R.id.ip_editText);
		portText = (EditText) getView().findViewById(R.id.port_editText);
		Button connectButton = (Button) getView().findViewById(R.id.connect_button);
		Button scanButton = (Button) getView().findViewById(R.id.scan);
		connectButton.setOnClickListener(this);
		typeSpinner.setOnItemSelectedListener(new CustomOnItemSelectedListener());
		scanButton.setOnClickListener(new OnClickListener() {
			@Override
			public void onClick(View v) {
				Intent intent = new Intent("com.google.zxing.client.android.SCAN");
			    if(getActivity().getPackageManager().queryIntentActivities(intent, PackageManager.MATCH_DEFAULT_ONLY).size()==0)
			        {
			            // ���w��
			            Toast.makeText(getActivity(), "�Ц� Play �ө��w�� ZXing ���X���y��", Toast.LENGTH_LONG).show();
			        }
			    else
			        {
			            // SCAN_MODE, �i�P�O�Ҧ��䴩�����X
			            // QR_CODE_MODE, �u�P�O QRCode
			            // PRODUCT_MODE, UPC and EAN �X
			            // ONE_D_MODE, 1 �����X
			            intent.putExtra("SCAN_MODE", "SCAN_MODE");
			     
			            // �I�sZXing Scanner�A�����ʧ@��^�� 1 �� onActivityResult �� requestCode �Ѽ�
			            startActivityForResult(intent, 1);
			        }
			}
		});
	}

	// ���� ZXing ���y��^�ǨӪ����G
	public void onActivityResult(int requestCode, int resultCode, Intent intent) 
	{
	    if(requestCode==1) 
	    {
	        if(resultCode==Activity.RESULT_OK) 
	            {
	                // ZXing�^�Ǫ����e
	                String contents = intent.getStringExtra("SCAN_RESULT");
	                //TextView textView1 = (TextView) findViewById(R.id.textView1);
	                String[] data= contents.split(":");
	                if(data.length==3){
	                	ipAddressText.setText(data[0]);
	                	portText.setText(data[1]);
	                	port2Str=data[2];
	                }else {
		                Toast.makeText(getActivity(), "�������榡", Toast.LENGTH_LONG).show();
					}
	            }
	        else
	            if(resultCode==Activity.RESULT_CANCELED) 
	            {
	                Toast.makeText(getActivity(), "�������y", Toast.LENGTH_LONG).show();
	            }
	    }
	}
	
	@Override
	public void onClick(View v) {
		final String adrs = ipAddressText.getText().toString();
		int port1 = Integer.parseInt(portText.getText().toString());
		final int port2=Integer.parseInt(port2Str);
		if(connectedListener!=null)
			connectedListener.onConnected(adrs, port1);
		
		connection = new ServiceConnection() {  
			  
	        @Override  
	        public void onServiceDisconnected(ComponentName name) {  
	        }  
	  
	        @Override  
	        public void onServiceConnected(ComponentName name, IBinder service) {  
	        	binder = (OrientationService.MyBinder) service;  
	        	binder.initialize(getActivity(), adrs, port2);
	        }  
	    };
	    
	    Intent bindIntent = new Intent(getActivity(), OrientationService.class);  
		getActivity().bindService(bindIntent, connection, Context.BIND_AUTO_CREATE);  
		
		
//		Intent intent = new Intent("com.splashtop.remote.pad.v2");
//	    if(getActivity().getPackageManager().queryIntentActivities(intent, PackageManager.MATCH_DEFAULT_ONLY).size()==0)
//	        {
//	            // ���w��
//	            Toast.makeText(getActivity(), "�Ц� Play �ө��w�� splashtop", Toast.LENGTH_LONG).show();
//	        }
//	    else
//	        {
//	    		startActivity(intent);
//	        }
		
	}

	public void setConnectedListener(IOnConnectedListener connectedListener) {
		this.connectedListener = connectedListener;
	}
	
	@Override
	public void onDestroy() {
		Log.d("ConnectionFragment", "onDestroy");
		super.onDestroy();
		if(connection!=null)
			getActivity().unbindService(connection); 
	}
	
	class CustomOnItemSelectedListener implements OnItemSelectedListener {
		 
	    public void onItemSelected(AdapterView<?> parent, View view, int pos,
	            long id) {
	    	layoutType=parent.getItemAtPosition(pos).toString();
	        Toast.makeText(parent.getContext(), 
	                "On Item Select : \n" + layoutType,
	                Toast.LENGTH_SHORT).show();
	    }
	 
	    @Override
	    public void onNothingSelected(AdapterView<?> arg0) {
	        // TODO Auto-generated method stub
	    }
	 
	}

	public String getLayoutType() {
		return layoutType;
	}
	

}
