using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class SceneSpawnController : MonoBehaviour {

	//public Transform localPlayerObject; //Note: we leave local player as object and do not instantiate it to keep existing Island Demo scripts working.
	public GameObject serverPlayerPrefab;
	public Transform[] spawnPoints;
	private static System.Random random = new System.Random();
	public Dictionary<User,GameObject> scenePlayers=new Dictionary<User,GameObject>();

	public void SpawnServerPlayer(User user) {
		int n = spawnPoints.Length;
		Transform spawnPoint = spawnPoints [scenePlayers.Count%n];
		Vector3 relativePos = transform.position - spawnPoint.position;
		relativePos.y=0;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		GameObject playerInServer = 
			Instantiate(serverPlayerPrefab, spawnPoint.position, rotation) as GameObject;
		playerInServer.name = "user_"+user.GetId();
		playerInServer.SendMessage("StartSending");


		PlayerStatus ps=playerInServer.GetComponent<PlayerStatus>();
		ps.color=cr(scenePlayers.Count);
		ps.userName=user.GetName();

		//tell other client new player status
		foreach(User u in scenePlayers.Keys){
			SendStatusToRemotePlayer(u,ps);
		}
		SendStatusToRemotePlayer(user,ps);//create local player
		foreach(GameObject g in scenePlayers.Values){//add p alre exist
			PlayerStatus pps=g.GetComponent<PlayerStatus>();
			SendStatusToRemotePlayer(user,pps);
		}
		scenePlayers.Add(user,playerInServer);
	}


	void SendStatusToRemotePlayer(User toUser,PlayerStatus status) {
		SmartFoxClient client = ClientNetworkController.GetClient();
		SFSObject o=status.ToSFSObject();
		o.Put("scene",client.myUserName);
		client.SendObject(o);
	}
	
	private void UserEnterRoom(User user) {
		SpawnServerPlayer(user);
	}
	
	public void UserLeaveRoom(int userId) {

		foreach (User u in scenePlayers.Keys){
			if(u.GetId()==userId){
				Destroy(scenePlayers[u]);
					scenePlayers.Remove(u);
			}
		}
	}
	
	private User remoteUser = null;
	void FixedUpdate() {
		if (remoteUser!=null) {
			SpawnServerPlayer(remoteUser);
			remoteUser = null;	
		}
	}

	Color cr(int i){
		Color c=Color.white;
		i = i % 7;
		switch(i){
		case 0:
			c=Color.red;
			break;
		case 1:
			c=Color.blue;
			break;
		case 2:
			c=Color.clear;
			break;
		case 3:
			c=Color.cyan;
			break;
		case 4:
			c=Color.gray;
			break;
		case 5:
			c=Color.green;
			break;
		case 6:
			c=Color.magenta;
			break;
		}
		return c;
	}

}
