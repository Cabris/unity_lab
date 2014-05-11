package test;

import java.nio.channels.SocketChannel;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedList;

import it.gotoandplay.smartfoxserver.data.User;
import it.gotoandplay.smartfoxserver.events.InternalEventObject;
import it.gotoandplay.smartfoxserver.extensions.AbstractExtension;
import it.gotoandplay.smartfoxserver.extensions.ExtensionHelper;
import it.gotoandplay.smartfoxserver.lib.ActionscriptObject;

public class MyExtension extends AbstractExtension {

	private ExtensionHelper helper;
	// Scene myScene;
	SceneManager sceneManager;

	/**
	 * Initialize the extension.<br>
	 * It's always good practice to keep a reference to the ExtensionHelper
	 * object
	 */
	public void init() {
		helper = ExtensionHelper.instance();
		sceneManager = new SceneManager(this);
		// Traces the message to the AdminTool and to the console
		trace("Hi! The Simple Extension is initializing");

	}

	/**
	 * Destroy the extension
	 */
	public void destroy() {
		trace("Bye bye! SimpleExtension is shutting down!");
	}

	/**
	 * Handle client requests sent in XML format. The AS objects sent by the
	 * client are serialized to an ActionscriptObject
	 * 
	 * @param ao
	 *            the ActionscriptObject with the serialized data coming from
	 *            the client
	 * @param cmd
	 *            the cmd name invoked by the client
	 * @param fromRoom
	 *            the id of the room where the user is in
	 * @param u
	 *            the User who sent the message
	 */
	public void handleRequest(String cmd, ActionscriptObject ao, User u,
			int fromRoom) {

		try {
			if (cmd.equals("registScene")) {
				Scene scene = new Scene(u, this);
				String sceneType = ao.getString("type");
				scene.type = sceneType;
				sceneManager.registScene(scene);
				trace(cmd);
			} else if (cmd.equals("requestSceneInfo")) {
				sceneManager.SendSceneInfo(cmd, u, fromRoom);
			} else if (cmd.equals("joinScene")) {
				String sceneName = ao.getString("host");
				sceneManager.addUser(u, sceneName);
				trace(cmd);
			}
			sceneManager.handleRequest(cmd, ao, u, fromRoom);
		} catch (Exception e) {
			trace("ex: " + e.fillInStackTrace());
		}

	}

	// void AddClientToScene(User client, Scene scene) {
	// scene.getUsers().put(client.getName(), client);
	//
	// LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
	// ll.add(scene.getOwner().getChannel());
	// ActionscriptObject pao = new ActionscriptObject();
	// pao.put("cmd", "getScene");
	// pao.put("to", client.getName());
	// sendResponse(pao, scene.getOwner().getRoom(), client, ll);
	// trace("AddClientToScene: c=" + client.getName() + ", s="
	// + scene.sceneId);
	// }

	/**
	 * Handle client requests sent in String format. The parameters sent by the
	 * client are split in a String[]
	 * 
	 * @param params
	 *            an array of data coming from the client
	 * @param cmd
	 *            the cmd name invoked by the client
	 * @param fromRoom
	 *            the id of the room where the user is in
	 * @param u
	 *            the User who sent the message
	 */
	public void handleRequest(String cmd, String[] params, User u, int fromRoom) {
		String psString = "";
		for (int i = 0; i < params.length; i++) {
			psString += params[i];
			psString += ",";
		}

		trace("The command -> " + cmd + " was invoked by user -> "
				+ u.getName() + " ,ps:" + psString);
	}

	/**
	 * Handles an event dispateched by the Server
	 * 
	 * @param ieo
	 *            the InternalEvent object
	 */
	public void handleInternalEvent(InternalEventObject ieo) {
		trace("Received a server event --> " + ieo.getEventName());
		String evtName = ieo.getEventName();
		if (evtName.equals("userExit") || evtName.equals("userLost")) {
			// int uId = Integer.valueOf(ieo.getParam("uid"));
			User user = (User) ieo.getObject("user");
			if (user != null)
				trace("user offline: " + user.getName());
			else {
				trace("null user");
			}
			sceneManager.removeUser(user.getName());
		}
	}

}
