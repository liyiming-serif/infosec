using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TaskManager : MonoBehaviour
{
    [SerializeField]
    TaskBarItem itemPrefab;

    List<TaskBarItem> items;
    List<Windows> tasks;

    int nowActive;

    public void SetBarItemVisible(object[] paras)
    {
        //TODO ensure only ONE task is activated.

        if(paras.Length != 2)
        {
            return;
        }
        if(!(paras[0] is int) || !(paras[1] is bool))
        {
            return;
        }
        int id = (int)paras[0];
        bool visible = (bool) paras[1];
        TaskBarItem item = LookUpBarItem(id);
        if (item)
        {
            item.gameObject.SetActive(visible);
        }

    }

    public void KillTask(int id)
    {
        TaskBarItem item = LookUpBarItem(id);
        if (item)
        {
            Destroy(item.gameObject);
        }

        Windows task = LookUpWindows(id);
        if(task)
        {
            Destroy(task.gameObject);
        }
    }

   public  void AddNewTask(Windows newTask)
    {
        tasks.Add(newTask);
        TaskBarItem newItem = Instantiate(itemPrefab);
        newItem.transform.SetParent(this.transform);
        newItem.Register(newTask.id);
        items.Add(newItem);
        // TODO set the current active task to inactive
        nowActive = newTask.id;
    }

    public TaskBarItem LookUpBarItem(int id)
    {
        //Primitive Imp.
        foreach (TaskBarItem item in items)
        {
            if(id == item.id)
            {
                return item;
            }
        }
        return null;
    }

    public Windows LookUpWindows(int id)
    {
        //Primitive Imp.
        foreach (Windows task in tasks)
        {
            if (id == task.id)
            {
                return task;
            }
        }
        return null;
    }

    // Use this for initialization
    void Start()
    {
        nowActive = 0;
        items = new List<TaskBarItem>();
        items.AddRange(this.GetComponentsInChildren<TaskBarItem>());
        tasks = new List<Windows>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
