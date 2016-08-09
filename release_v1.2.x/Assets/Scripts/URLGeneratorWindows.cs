﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class URLGeneratorWindows : MonoBehaviour {

    Button sendButton;
    [SerializeField]
    List<Slot> slots;
    Windows parent;

    void Awake()
    {
        parent = GetComponentInParent<Windows>();
        Assert.IsNotNull(parent);
        sendButton = GetComponentInChildren<Button>();
        Assert.IsNotNull(sendButton);
    }

    List<Domain> constuctDName()
    {
        List<Domain> domainNames = new List<Domain>();
        foreach (Slot s in slots)
        {
            domainNames.Add(s.GetDomain());
        }
        return domainNames;
    }

    void Start()
    {
        sendButton.onClick.AddListener(delegate
        {
            parent.ReturnTaskManager().SendMessage("SendMail", constuctDName());
            parent.ReturnTaskManager().SendMessage("KillTask", parent.GetID());
        });
    }

    // Update is called once per frame
    void Update () {
	
	}
}
