using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    List<AppSpawn> apps;

    NetworkW _network;

    List<GUI> items;
    List<GUI> wins;

    int nowActive;

    public NetworkW ReturnNetwork()
    {
        return _network;
    }

    public List<AppSpawn> ReturnApps()
    {
        return apps;
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

    public Windows LookUpWindows(int id)
    {
        return (Windows)LookUpGUI(id, wins);
    }
     
    public TaskBarItem LookUpTaskBarItem(int id)
    {
        return (TaskBarItem)LookUpGUI(id, wins);
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

        TaskBarItem newItem = Instantiate(Resources.Load("TaskBarItem"), transform) as TaskBarItem;
        newItem.Register(newTask.GetID());
        newItem.GetComponentInChildren<Text>().text = (newTask as IHasTitle).GetTitle();

        items.Add(newItem);
        // TODO set the current active task to inactive
        nowActive = newTask.GetID();
    }

    public GUI LookUpGUI(int id, List<GUI> guis)
    {
        //Primitive Imp.
        foreach (GUI g in guis)
        {
            if(g.IsTarget(id))
            {
                return g;
            }
        }
        return null;
    }

    public void SendURLString(List<Domain> urlString)
    {
       _network.SendMessage("SendVictimTo", urlString);
    }

    void Awake()
    {
        nowActive = 0;
        items = new List<GUI>();
        wins = new List<GUI>();
    }

    void Start()
    {
        try
        {
           _network = GameObject.FindObjectOfType<NetworkW>();
        }
        catch
        {
            Debug.Log("Network is not found in this challenge.");
        }
    }
}
