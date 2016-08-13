using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    [SerializeField]
    List<AppSpawn> apps;

    NetworkWindows _network;

    List<GUI> items;
    List<GUI> wins;

    int nowActive;

    public void AlienGo()
    {
        _network.AlienGo();
    }

    public void updateNetworkURL(Domain d, int id)
    {
        _network.SetSelfVisible(true);
        _network.updateNetworkURL(d, id);
    }

    public bool IsActive(int id)
    {
        return nowActive == id;
    }

    public void SetActiveTask(int id)
    {
        SetGUIVisible(id, true, wins);
        //TODO set taskbarItem active
        nowActive = id;
    }

   public void SetInactiveTask(int id)
    {
        SetGUIVisible(id, false, wins);
        //TODO Set TaskBarItem inactive
        nowActive = 0;
    }

    public void KillTask(int id)
    {
        RemoveGUI(id, wins);
        RemoveGUI(id, items);
    }

    public Windows LookUpWindows(string name)
    {
        Windows result = null;
        foreach (AppSpawn a in apps)
        {
            if(a.appName == name)
            {
                result = LookUpWindows(a.GetID());
            }
        }
        return result;
    }
    public Windows LookUpWindows(int id)
    {
        return LookUpGUI(id, wins) as Windows;
    }
     
    public TaskBarItem LookUpTaskBarItem(int id)
    {
        return LookUpGUI(id, wins) as TaskBarItem;
    }

    public void SetGUIVisible(int id, bool visible, List<GUI> guis)
    {
        GUI item = LookUpGUI(id, guis);
        if (item)
        {
            item.SetSelfVisible(visible);
        }
    }

    public void RemoveGUI(int id, List<GUI> guis)
    {
        GUI g = LookUpGUI(id, guis);
        if(g)
        {
            Destroy(g.gameObject);
            guis.Remove(g);
        }
    }

   public void AddNewTask(GUI newTask)
    {
        wins.Add(newTask);
        int id = newTask.GetID();
        TaskBarItem newItem = (Instantiate(Resources.Load("TaskBarItem"), transform) as GameObject).GetComponent<TaskBarItem>();
        newItem.Register(id);
        newItem.GetComponentInChildren<Text>().text = (newTask as IHasTitle).GetTitle();

        items.Add(newItem);
        // TODO set the current active task to inactive
        nowActive = id;
    }

    public GUI LookUpGUI(int id, List<GUI> guis)
    {
        foreach (GUI g in guis)
        {
            if(g.IsTarget(id))
            {
                return g;
            }
        }
        return null;
    }

    void Awake()
    {
        nowActive = 0;
        items = new List<GUI>();
        wins = new List<GUI>();
        instance = this;
    }

    void Start()
    {
        try
        {
            _network = GameObject.FindObjectOfType<NetworkWindows>();
            _network.SetSelfVisible(false);
        }
        catch
        {
            Debug.Log("Network is not found in this challenge.");
        }
    }
}
