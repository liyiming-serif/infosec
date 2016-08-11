using UnityEngine;
using System.Collections;

public static class Common{
    static TaskManager manager = GameObject.FindObjectOfType<TaskManager>();
    public static TaskManager ReturnTManager()
    {
        return manager;
    }
    public static void SendMSGTManager(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.RequireReceiver)
    {
        manager.SendMessage(methodName, value, options);
    }
}
