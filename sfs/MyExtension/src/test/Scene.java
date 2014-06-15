package test;

import it.gotoandplay.smartfoxserver.data.User;
import it.gotoandplay.smartfoxserver.extensions.AbstractExtension;
import it.gotoandplay.smartfoxserver.lib.ActionscriptObject;

import java.nio.channels.SocketChannel;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.Timer;

public class Scene {

	private User owner;
	AbstractExtension ext;
	private HashMap<String, User> users;
	public String type;
	public String sceneName;

	// Date date;

	public Scene(User owner, AbstractExtension a) {
		sceneName = owner.getName();
		this.owner = owner;
		users = new HashMap<String, User>();
		max = 4;
		ext = a;
	}

	public User getOwner() {
		return owner;
	}

	private int max;

	public int getMax() {
		return max;
	}

	public void setMax(int max) {
		this.max = max;
	}

	public boolean isFull() {
		return users.size() >= max;
	}

	public void addUser(User user) {
		if (users.containsKey(user.getName()))
			return;
		users.put(user.getName(), user);
		ActionscriptObject a = new ActionscriptObject();
		a.put("cmd", "userJoinScene");
		a.put("userName", user.getName());
		a.putNumber("userId", user.getUserId());
		// LinkedList<SocketChannel> sceneChannel = getSceneChannel();
		// ext.sendResponse(a, owner.getRoom(), owner, sceneChannel);
		handleRequest(a, user, owner.getRoom());
		ext.trace("addUser_userJoinScene");
	}

	public void removeUser(String userName) {
		users.remove(userName);
	}

	public LinkedList<SocketChannel> getUsersChannels() {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		for (User user : users.values())
			if (user != null)
				ll.add(user.getChannel());
		return ll;
	}

	public void broadcast(ActionscriptObject ao) {
		ext.sendResponse(ao, owner.getRoom(), owner, getUsersChannels());
		String cmd = ao.getString("cmd");
		ext.trace("scene " + owner.getName() + " broadcast msg: " + cmd);
	}

	public void handleRequest(ActionscriptObject ao, User u, int fromRoom) {
		LinkedList<SocketChannel> sceneChannel = getSceneChannel();
		ext.sendResponse(ao, owner.getRoom(), owner, sceneChannel);
		String request = aoInfo(ao);
		ext.trace("scene " + owner.getName() + " handleRequest: " + request);
	}

	private String aoInfo(ActionscriptObject ao) {
		String infoString = "ao: \n";
		for (Object k : ao.keySet()) {
			Object obj = ao.get(k.toString());
			infoString+="key: "+k.toString()+", value: "+obj.toString()+".\n";
		}
		return infoString;
	}

	private LinkedList<SocketChannel> getSceneChannel() {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		ll.add(owner.getChannel());
		return ll;
	}

	public void transfer(ActionscriptObject ao, String userName) {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		ll.add(users.get(userName).getChannel());
		ext.sendResponse(ao, owner.getRoom(), owner, ll);
		ext.trace("transfer msg to " + userName);
	}

}
