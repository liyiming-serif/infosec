using UnityEngine;
using UnityEngine.Assertions;
using System;
using UnityEngine.EventSystems;
using System.Collections;

public class AppSpawn : MonoBehaviour
{

    [SerializeField]
    Windows appPrefab;

    TaskManager manager;
    int id;

    bool one_click;
    bool timer_running;
    float timer_for_double_click;

    //this is how long in seconds to allow for a double click
    float delay = 1f;

    void OnMouseDown()
    {
        if (!one_click) // first click no previous clicks
        {
            one_click = true;

            timer_for_double_click = Time.time; // save the current time
                                                // do one click things;
        }
        else
        {
            one_click = false; // found a double click, now reset
            //Start a new App if it's not running
            //TODO Restore the windows if running
            Windows existed = manager.LookUpWindows(id);
            if (existed)
            {
                existed.SetSelfVisible(true); // The App is running.
            }
            else
            {
                Windows newApp = Instantiate(appPrefab);
                try
                {
                    newApp.transform.SetParent(GetComponentInParent<Canvas>().transform);
                }
                catch (Exception e)
                {
                    Debug.Log("Canvas is not found.");
                }
                id = newApp.GetHashCode();
                newApp.Register(id);
                manager.AddNewTask(newApp);
            }

        }
    }

    void Awake()
    {
        manager = GameObject.FindObjectOfType<TaskManager>();
        Assert.IsNotNull(manager);
        id = 0;
        timer_for_double_click = Time.time;
        one_click = false;
    }

    void Update()
    {
        if (one_click)
        {
            // if the time now is delay seconds more than when the first click started. 
            if ((Time.time - timer_for_double_click) > delay)
            {
                one_click = false; // basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
            }
        }
    }
}