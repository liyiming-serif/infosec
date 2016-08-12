using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;
using System;

public class URLGenerator : MonoBehaviour, ISlotUpdated {

    Button sendButton;

    [SerializeField]
    List<Slot> slots;

    void Awake()
    {
        sendButton = GetComponentInChildren<Button>();
        Assert.IsNotNull(sendButton);
    }

    void Start()
    {
        sendButton.onClick.AddListener(delegate
        {
            Common.ReturnTManager().AlienGo();
            Common.ReturnTManager().KillTask(GetComponentInParent<GUI>().GetID());
        });
        for(int i = 0; i < slots.Count; i++)
        {
            slots[i].id = i;
        }
    }

    public void NoticeNetworkURLBoard(Domain d, int id)
    {
        Common.ReturnTManager().updateNetworkURL(d, id);
    }
}
