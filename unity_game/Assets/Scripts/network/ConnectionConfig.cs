using System;

public class ConnectionConfig
{
	public string ServerIP {get; private set;}
	public int ServerPort {get; private set;}			
	public string Zone{get; private set;}
	public string Room{get; private set;}
	
	public ConnectionConfig ()
	{
		ServerIP = "140.115.53.97";
		ServerPort = 9339;			
		Zone = "city";
		Room="Central Square";
	}
		
}


