using UnityEngine;
using System.Collections;

public static class Common{
    static TaskManager manager = GameObject.FindObjectOfType<TaskManager>();
    public static TaskManager ReturnTManager()
    {
        return manager;
    }
}
