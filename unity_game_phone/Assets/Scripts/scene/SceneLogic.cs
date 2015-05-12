using UnityEngine;
using System.Collections.Generic;

public class SceneLogic : MonoBehaviour {
	[SerializeField]
	List<Transform> stages;
	PlayerBeheaver player;
	[SerializeField]
	UILabel label;
	float deltaTime = 0.0f;
	// Use this for initialization
	void Start () {
		player=Extensions.Player;

	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
		string msg="Welcome! ";
		msg+=("[FFFF00]"+player.playerId+"[-].\n");
		msg+=("You have been chosen to handle the tests of several Physic phenomenon.\n" +
		      "Before you start your journey you need to get used abnout how to manipulate the game.\n");
		label.text=msg;
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
		
		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 50);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 50;
		style.normal.textColor = new Color (0.5f, 0.5f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	}
}
