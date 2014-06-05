using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ClientSpawnController : MonoBehaviour
{
	public GameObject localPlayerPrefab; //Note: we leave local player as object and do not instantiate it to keep existing Island Demo scripts working.
	public GameObject remotePlayerPrefab;
	public GameObject localPlayer;
	public List<GameObject> remotePlayers=new List<GameObject>();
	
	public void SpawnLocalPlayer (SFSObject data)
	{
		string name=data.GetString("name");
		if(GameObject.Find(name)!=null)
			return;
		localPlayer = 
			Instantiate(localPlayerPrefab) as GameObject;
		PlayerStatus ps=localPlayer.GetComponent<PlayerStatus>();
		ps.FromHashtable(data);
		localPlayer.GetComponent<NetworkTransformReceiver>().StartReceiving();
		//localPlayer.SendMessage ("StartReceiving");
	}

	public void SpawnRemotePlayer (SFSObject data)
	{
		string name=data.GetString("name");
		if(GameObject.Find(name)!=null)
			return;
		GameObject remotePlayer = 
			Instantiate(remotePlayerPrefab)as GameObject;
		PlayerStatus ps=remotePlayer.GetComponent<PlayerStatus>();
		ps.FromHashtable(data);
		//remotePlayer.SendMessage ("StartReceiving");
		remotePlayer.GetComponent<NetworkTransformReceiver>().StartReceiving();
		remotePlayers.Add(remotePlayer);
	}

	public void UserEnterRoom (User user)
	{
		//When remote user enters our room we spawn his object.
		//SpawnRemotePlayer (user);
		//remoteUser = user;
	}
	
	public void UserLeaveRoom (int userId)
	{
		//Just destroy the corresponding object
		GameObject obj = GameObject.Find ("user_" + userId);
		if (obj != null){
			remotePlayers.Remove(obj);
			Destroy (obj);
		}
	}

}
