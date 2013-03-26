using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Events {
	
	// Public interface
	public static void Listen(GameObject observer, string messageName) { events._Listen(observer, messageName, null); }
	public static void Listen(GameObject observer, string messageName, object sender) { events._Listen(observer, messageName, sender); }
	public static void StopListening(GameObject observer, string messageName) { events._StopListening(observer, messageName);}
	public static void Send(object sender, string messageName) { events._Send(new Notification(sender, messageName)); }
	public static void Send(object sender, string messageName, object data) { events._Send(new Notification(sender, messageName, data)); }
	public static void Send(Notification notification) { events._Send(notification); }
	
	// The singleton instance
	private static Events events = new Events();
	
	// The Notification class is the object that is send to receiving objects of a notification type.
	public class Notification {
	    public object sender;
	    public string messageName;
	    public object data;
	    
		public Notification(object sender, string messageName) : this(sender, messageName, null) {}
	    public Notification(object sender, string messageName, object data) {
			this.sender = sender;
			this.messageName = messageName;
			this.data = data;
		}
	}
	
	// The hashtable containing all the notifications.  Each notification
	// in the hash table is an ArrayList that contains all the observers
	// for that notification.
	private Dictionary<string, List<GameObject>> notifications = new Dictionary<string, List<GameObject>>();

	private void _Listen(GameObject observer, string messageName, object sender) {
	
	    // If the name isn't good, then throw an error and return.
	    if (messageName == null || messageName == "") { 
			Debug.Log("Null name specified for notification in Listen.");
			return;
		}

		// If this specific notification doens't exist yet, then create it.
	    if (!notifications.ContainsKey(messageName))
	        notifications[messageName] = new List<GameObject>();
	 
	    List<GameObject> notifyList = notifications[messageName];
	 
	    // If the list of observers doesn't already contains the one that's registering, then add it.
	    if (!notifyList.Contains(observer))
			notifyList.Add(observer);
	}

	private void _StopListening(GameObject observer, string messageName) {
		if (!notifications.ContainsKey(messageName)) return;
	    List<GameObject> notifyList = notifications[messageName];
	 
	    // Assuming that this is a valid notification type, remove the observer from the list.
	    if (notifyList != null) {
	        if (notifyList.Contains(observer)) 
				notifyList.Remove(observer);
	        if (notifyList.Count == 0)
				notifications.Remove(messageName);
	    }
	}

	// Send a notification object to all objects that have requested to receive this type of notification.
	// A notification can either be posted with a notification object or by just sending the individual components.
	private void _Send(Notification notification) {
	    // First make sure that the name of the notification is valid.
	    if (notification.messageName == null || notification.messageName == "") {
			Debug.Log("Null name sent to Send.");
			return;
		}

		// Obtain the notification list, and make sure that it is valid as well
		List<GameObject> notifyList = null;
		if (notifications.ContainsKey(notification.messageName))
			notifyList = notifications[notification.messageName];
	    if (notifyList == null) { 
			return;
		}
	 
	    // Create an array to keep track of invalid observers that we need to remove
	    List<GameObject> observersToRemove = new List<GameObject>();
	 
	    // Iterate through all the objects that have signed up to be notified by this type of notification.
	    foreach (GameObject observer in notifyList) {
	        if (observer == null) { 
	        	// observer is invalid
	        	observersToRemove.Add(observer);
	        } else { 
	        	// observer is valid
	            observer.SendMessage(notification.messageName, notification, SendMessageOptions.DontRequireReceiver);
	        }
	    }
	 
	    // Remove all the invalid observers
	    foreach (GameObject observer in observersToRemove) {
	        notifyList.Remove(observer);
	    }
	}
}
