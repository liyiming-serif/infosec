using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class URLGenerator : MonoBehaviour, IEventSystemHandler {

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
    }
}
