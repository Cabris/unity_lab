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

	//private HashMap<String, Scene> scenes;
	private ExtensionHelper helper;
Scene myScene;
	/**
	 * Initialize the extension.<br>
	 * It's always good practice to keep a reference to the ExtensionHelper
	 * object
	 */
	public void init() {
		helper = ExtensionHelper.instance();
	//	scenes = new HashMap<String, Scene>();
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
			if(cmd=="registScene"){
				Scene scene=new Scene(u, u.getName(), this);
				String sceneType=ao.getString("type");
				scene.type=sceneType;
				
			}
			if (cmd.equals("sceneOnline")) {
				myScene=new Scene(u, u.getName(), this);
			}
			if (cmd.equals("clientOnline")) {
				myScene.getUsers().put(u.getName(), u);
			}
			if (cmd.equals("b")) {// tf to all clients
				Scene scene = myScene;
				scene.sendMsg(ao);
			}
			if (cmd.equals("#")) {// tf to spec clients
				String to = ao.getString("to");
				Scene scene = myScene;
				scene.sendMsg(to, ao);
			}
			if (cmd.equals("s")) {// to scene
				String to = ao.getString("to");
				Scene scene = myScene;
				scene.sendMsgToScene(u, ao);
			}
		} catch (Exception e) {
			trace("ex: " + e.toString());
		}

	}

	void AddClientToScene(User client, Scene scene) {
		scene.getUsers().put(client.getName(), client);

		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		ll.add(scene.getOwner().getChannel());
		ActionscriptObject pao = new ActionscriptObject();
		pao.put("cmd", "getScene");
		pao.put("to", client.getName());
		sendResponse(pao, scene.getOwner().getRoom(), client, ll);
		trace("AddClientToScene: c=" + client.getName() + ", s="
				+ scene.sceneId);
	}

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
			int uId = Integer.valueOf(ieo.getParam("uid"));

		}
	}

}
