﻿using UnityEngine;
using UnityEngine.Assertions;

using System;
using UnityEngine.EventSystems;
using System.Collections;

public class AppSpawn : MonoBehaviour
{

    [SerializeField]
    Windows appPrefab;
    [SerializeField]
    Vector2 sizeDelta;
    [SerializeField]
    Vector2 localPos;

    Animator animator;
    int id;

    bool one_click;
    bool timer_running;
    float timer_for_double_click;

    //this is how long in seconds to allow for a double click
    [SerializeField]
    float delay = 1f;

    public void PlayAnimation(string stateName)
    {
        animator.Play(stateName);
    }

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
       
            Windows existed = Common.ReturnTManager().LookUpWindows(id);
            if (existed)
            {
                Common.SendMSGTManager("SetActiveTask", id);
            }
            else
            {
                //Start a new App if it's not running
                if (animator)
                {
                    animator.Stop(); //TODO Replace it with TriggerState
                }
                Windows newApp = Instantiate(appPrefab);
                newApp.GetComponent<RectTransform>().sizeDelta = sizeDelta;
                newApp.transform.localPosition = localPos;     
                try
                {
                    newApp.transform.SetParent(GetComponentInParent<Canvas>().transform);
                }
                catch
                {
                    Debug.Log("Canvas is not found.");
                }
                id = newApp.GetHashCode();
                newApp.Register(id);
                Common.SendMSGTManager("AddNewTask", newApp);
            }

        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
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