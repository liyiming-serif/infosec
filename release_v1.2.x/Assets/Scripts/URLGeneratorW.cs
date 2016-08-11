using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class URLGeneratorW : MonoBehaviour {

    Button sendButton;
    Windows parent;

    [SerializeField]
    List<Slot> slots;

    void Awake()
    {
        parent = GetComponentInParent<Windows>();
        sendButton = GetComponentInChildren<Button>();
        Assert.IsNotNull(parent);
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

}
