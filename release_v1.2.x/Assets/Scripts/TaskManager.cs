using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    TaskBarItem itemPrefab;

    List<GUI> items;
    List<GUI> wins;

    int nowActive;

    public void SetBarItemVisible(object[] paras)
    {
        SetGUIVisible(paras, items);
        //TODO ensure only ONE task is activated.
    }

    public void SetWindowsVisible(object[] paras)
    {
        SetGUIVisible(paras, wins);
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

    public void SetGUIVisible(object[] paras, List<GUI> guis)
    {
        if (paras.Length != 2)
        {
            return;
        }
        else if (!(paras[0] is int) || !(paras[1] is bool))
        {
            return;
        }

        int id = (int)paras[0];
        bool visible = (bool)paras[1];
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

   public void AddNewTask(Windows newTask)
    {
        wins.Add(newTask);
        TaskBarItem newItem = Instantiate(itemPrefab);
        newItem.transform.SetParent(this.transform);
        newItem.Register(newTask.GetID());
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

    void Awake()
    {
        nowActive = 0;
        items = new List<GUI>();
        wins = new List<GUI>();
    }

}
