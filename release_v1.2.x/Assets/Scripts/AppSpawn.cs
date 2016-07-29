using UnityEngine;
using UnityEngine.Assertions;
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

    // Use this for initialization
    void Start()
    {
        manager = GameObject.FindObjectOfType<TaskManager>();
        Assert.IsNotNull(manager);
        id = 0;
        timer_for_double_click = Time.time;
        one_click = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
                id = appPrefab.GetHashCode();
                appPrefab.Register(id);
                manager.AddNewTask(appPrefab);
                Debug.Log("added");
            }
        }

        if (one_click)
        {
            // if the time now is delay seconds more than when the first click started. 
            if ((Time.time - timer_for_double_click) > delay)
            {

                //basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
                one_click = false;

            }
        }
    }
}