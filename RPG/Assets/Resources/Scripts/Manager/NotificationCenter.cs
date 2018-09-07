using UnityEngine;  
using System;  
using System.Collections;  
using System.Collections.Generic;  
  
// 消息的类型  
public enum NotifyType  
{
    None = 0,
    DialogFunc = 1,
    GetItem = 2,
}  
   
public class NotifyEvent  
{  
    protected Dictionary<string, string> arguments;  //参数  
    protected NotifyType type;  //事件类型  
    protected System.Object sender;    //发送者  
  
  
    public NotifyType Type  
    {  
        get { return type; }  
        set { type = value; }  
    }  
  
    public Dictionary<string, string> Params  
    {  
        get { return arguments; }  
        set { arguments = value; }  
    }  
  
    public System.Object Sender  
    {  
        get { return sender; }  
        set { sender = value; }  
    }  
  
    
    public override string ToString()  
    {  
        return type + " [ " + ((sender == null) ? "null" : sender.ToString()) + " ] ";  
    }  
  
    public NotifyEvent Clone()  
    {  
        return new NotifyEvent(type, arguments, sender);  
    }  
  
  
    public NotifyEvent(NotifyType type, System.Object sender)  
    {  
        Type = type;  
        Sender = sender;  
        if (arguments == null)  
        {  
            arguments = new Dictionary<string, string>();  
        }  
    }  
  
    public NotifyEvent(NotifyType type, Dictionary<string, string> args, System.Object sender)  
    {  
        Type = type;  
        arguments = args;  
        Sender = sender;  
        if (arguments == null)  
        {  
            arguments = new Dictionary<string, string>();  
        }  
    }  
}  
  
 
public delegate void EventListenerDelegate(NotifyEvent evt);  
   
public class NotificationCenter : MonoBehaviour
{  
    
    public static NotificationCenter m_instance;  
    private NotificationCenter() { }  
	
	void Awake()
	{
		if (m_instance == null)  
        {   
            m_instance = this;  
        }  
	}
	
    public static NotificationCenter GetInstance()  
    {  
        
        return m_instance;  
    }  
  
    Dictionary<NotifyType, EventListenerDelegate> notifications = new Dictionary<NotifyType, EventListenerDelegate>() ; // 所有的消息  
  
  
    public void RegisterObserver(NotifyType type, EventListenerDelegate listener)  
    {  
        if (listener == null)  
        {  
            // Debug.LogError("registerObserver: listener不能为空");  
            return;  
        }  
  
        // 将新来的监听者加入调用链，这样只要调用Combine返回的监听者就会调用所有的监听者  
        // Debug.Log("NotificationCenter: 添加监视" + type);  
  
        EventListenerDelegate myListener = null;  
        notifications.TryGetValue(type, out myListener);  
        notifications[type] = (EventListenerDelegate)Delegate.Combine(myListener, listener);  
    }  
  
    public void RemoveObserver(NotifyType type, EventListenerDelegate listener)  
    {  
        if (listener == null)  
        {  
            // Debug.LogError("removeObserver: listener不能为空");  
            return;  
        }  
          
        // 与添加的思路相同，只是这里是移除操作  
        // Debug.Log("NotificationCenter: 移除监视" + type);  
        notifications[type] = (EventListenerDelegate)Delegate.Remove(notifications[type], listener);  
    }  
  
    public void RemoveAllObservers()  
    {  
        notifications.Clear();  
    }  
  
    public void PostNotification(NotifyEvent evt)  
    {  
        EventListenerDelegate listenerDelegate;  
        if(notifications.TryGetValue(evt.Type, out listenerDelegate))  
        {  
            try  
            {  
                // 执行调用所有的监听者  
                listenerDelegate(evt);  
            }  
            catch(System.Exception e)  
            {  
                throw new Exception(string.Concat(new string[] { "Error dispatching event", evt.Type.ToString(), ": ", e.Message, " ", e.StackTrace }), e);  
            }  
        }  
    }  
  
}  