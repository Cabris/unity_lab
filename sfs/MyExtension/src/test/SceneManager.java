package test;

import java.nio.channels.SocketChannel;
import java.util.HashMap;
import java.util.LinkedList;

import it.gotoandplay.smartfoxserver.data.User;
import it.gotoandplay.smartfoxserver.extensions.AbstractExtension;
import it.gotoandplay.smartfoxserver.lib.ActionscriptObject;

public class SceneManager {

	HashMap<String, Scene> sceneMap;
	AbstractExtension abstractExtension;

	public SceneManager(AbstractExtension a) {
		abstractExtension = a;
		sceneMap = new HashMap<String, Scene>();
	}

	public void addUser(User user, String sceneName) {
		Scene scene = sceneMap.get(sceneName);
		scene.addUser(user);
	}

	public void removeUser(String userName) {
		for (Scene scene : sceneMap.values()) {
			scene.removeUser(userName);
		}
		sceneMap.remove(userName);
	}

	public void registScene(Scene scene) {
		String name = scene.sceneName;
		if (!sceneMap.containsKey(name))
			sceneMap.put(name, scene);
	}

	public void SendSceneInfo(String cmd, User u, int fromRoom) {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		ll.add(u.getChannel());
		ActionscriptObject sceneInfo = getSceneInfo();
		abstractExtension.sendResponse(sceneInfo, fromRoom, null, ll);
		abstractExtension.trace(cmd);
	}

	public void handleRequest(String cmd, ActionscriptObject ao, User u,
			int fromRoom) {

		if (cmd.equals("b")) {// scene to all clients
			// String host = ao.getString("host");
			Scene scene = sceneMap.get(u.getName());
			scene.broadcast(ao);
		}
		if (cmd.equals("#")) {// to spec user
			String host = ao.getString("host");
			String to = ao.getString("to");
			Scene scene = sceneMap.get(host);
			scene.transfer(ao, to);
		}
		if (cmd.equals("s")) {// to scene
			String host = ao.getString("host");
			Scene scene = sceneMap.get(host);
			scene.handleRequest(ao, u, fromRoom);
		}

	}

	public ActionscriptObject getSceneInfo() {
		ActionscriptObject ao = new ActionscriptObject();
		ActionscriptObject alist = new ActionscriptObject();
		for (Scene scene : sceneMap.values()) {
			alist.put(scene.sceneName, scene.type);
		}
		ao.put("cmd", "SceneInfo");
		ao.put("dataList", alist);
		return ao;
	}

}
