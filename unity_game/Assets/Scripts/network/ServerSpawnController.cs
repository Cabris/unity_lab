using UnityEngine;
using System.Collections;

using System;
using SmartFoxClientAPI;
using SmartFoxClientAPI.Data;

public class ServerSpawnController : MonoBehaviour {

	//public Transform localPlayerObject; //Note: we leave local player as object and do not instantiate it to keep existing Island Demo scripts working.
	public Transform serverPlayerPrefab;
	public Transform[] spawnPoints;
	
	private static System.Random random = new System.Random();
	
	void SpawnPlayers() {
		//SpawnLocalPlayer();  // Spawn local player object
		//SpawnRemotePlayers(); // Spawn remote player object
	}
	
//	private void SpawnLocalPlayer() {
//		SmartFoxClient client = NetworkController.GetClient(); 
//		int n = spawnPoints.Length;
//		Transform spawnPoint = spawnPoints[random.Next(n)];
//		localPlayerObject.transform.position = spawnPoint.transform.position;
//		localPlayerObject.transform.rotation = spawnPoint.transform.rotation;
//		localPlayerObject.name = ""+client.myUserId;
//		localPlayerObject.SendMessage("StartSending");  // Start sending our transform to other players
//	}
	
	//Get the remote user list and spawn all remote players that have already joinded before us
//	private void SpawnRemotePlayers() {
//		SmartFoxClient client = NetworkController.GetClient(); 
//		foreach (User user in client.GetActiveRoom().GetUserList().Values) {
//			int id = user.GetId();
//			if (id!=client.myUserId) SpawnServerPlayer(user);
//		}
//	}
	
	
	private void SpawnServerPlayer(User user) {
		int n = spawnPoints.Length;
		Transform spawnPoint = spawnPoints [random.Next (n)];
		// Just spawn remote player at a very remote point
		UnityEngine.Object playerInServer = Instantiate(serverPlayerPrefab, spawnPoint.position, new Quaternion(0,0,0,1));

		//Give remote player a name like "remote_<id>" to easily find him then
		playerInServer.name = "user_"+user.GetId();

		//Start receiving trasnform synchronization messages
		//(playerInServer as Component).SendMessage("StartReceiving");
		(playerInServer as Component).SendMessage("StartSending");
		// Force this player to send us transform
		ForceRemotePlayerToSendTransform(user);
	}
	
	void ForceRemotePlayerToSendTransform(User user) {
		SmartFoxClient client = ClientNetworkController.GetClient();
		SFSObject data = new SFSObject();
		data.Put("_cmd", "f");  //We put _cmd = "f" here to know that this object contains "force send transform" demand
		data.Put("to_uid", user.GetId()); // Who this message is for
		client.SendObject(data);
	}
	
	private void UserEnterRoom(User user) {
		//When remote user enters our room we spawn his object.
		SpawnServerPlayer(user);
		//remoteUser = user;
	}
	
	private void UserLeaveRoom(int userId) {
		//Just destroy the corresponding object
		GameObject obj = GameObject.Find("user_"+userId);
		if (obj!=null) Destroy(obj);
	}
	
	private User remoteUser = null;
	void FixedUpdate() {
		if (remoteUser!=null) {
			SpawnServerPlayer(remoteUser);
			remoteUser = null;	
		}
	}
}
