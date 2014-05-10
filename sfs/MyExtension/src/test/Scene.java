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
	//Date date;
	
	public Scene(User owner, String id, AbstractExtension a) {
		sceneId = id;
		this.owner = owner;
		users = new HashMap<String, User>();
		max = 5;
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

	String sceneId;

	public boolean isFull() {
		return users.size() >= max;
	}

	public HashMap<String, User> getUsers() {
		return users;
	}

	public String getSceneId() {
		return sceneId;
	}

	public void setSceneId(String sceneId) {
		this.sceneId = sceneId;
	}

	public LinkedList<SocketChannel> getUsersChannels() {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		for (User user : users.values())
			if (user != null)
				ll.add(user.getChannel());
		return ll;
	}

	public void sendMsg(ActionscriptObject ao) {// send to all client
		Calendar cal = Calendar.getInstance();
		Date now = cal.getTime();
		//ao.put("cmd", "t");
		//String t= ""+now.getTime();
		//ao.put("ts", t);
		//int time=Date.
		ext.sendResponse(ao, owner.getRoom(), owner, getUsersChannels());
		ext.trace("sendMsg all");
	}

	public void sendMsg(String client, ActionscriptObject ao) {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		ll.add(users.get(client).getChannel());
		ext.sendResponse(ao, owner.getRoom(), owner, ll);
		ext.trace("sendMsg");
	}
	
	public void sendMsgToScene(User from, ActionscriptObject ao) {
		LinkedList<SocketChannel> ll = new LinkedList<SocketChannel>();
		ll.add(owner.getChannel());
		ext.sendResponse(ao, owner.getRoom(), owner, ll);
		ext.trace("sendMsgToScene");
	}
	
	
}
