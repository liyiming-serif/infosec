using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class URLGenerator : MonoBehaviour, IEventSystemHandler {

    Button sendButton;
    List<Slot> slots;

    void Awake()
    {
        sendButton = GetComponentInChildren<Button>();
        Assert.IsNotNull(sendButton);
        sendButton.interactable = false;
        slots = new List<Slot>();
        slots.AddRange(GetComponentsInChildren<Slot>());
    }

    void Start()
    {
        sendButton.onClick.AddListener(delegate
        {
            TaskManager.instance.AlienGo();
            TaskManager.instance.KillTask(GetComponentInParent<GUI>().GetID());
        });
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].id = i;
        }
    }

    public void NoticeNetworkURLBoard(Domain d, int id)
    {
        TaskManager.instance.updateNetworkURL(d, id);
        bool isReady = true;
        foreach(Slot s in slots)
        {
            //TODO primtive approach
            if (!s.holding)
            {
                isReady = false;
                break;
            }
        }
        if (isReady)
        {
            sendButton.interactable = true;
        }
    }
}
