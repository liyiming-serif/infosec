using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class Outbox : MonoBehaviour {

    Button sendButton;
    Dropdown choice;
    Windows parent;

    void Awake()
    {
        parent = GetComponentInParent<Windows>();
        Assert.IsNotNull(parent);
        choice = transform.Find("MailBody/Choice").GetComponent<Dropdown>();
        Assert.IsNotNull(choice);
        sendButton = GetComponentInChildren<Button>();
        Assert.IsNotNull(sendButton);
    }

    void Start()
    {
        sendButton.onClick.AddListener(delegate
        {
            parent.ReturnTaskManager().SendMessage("SendMail", choice.GetComponentInChildren<Text>().text);
            parent.ReturnTaskManager().SendMessage("KillTask", parent.GetID());
        });
    }
    // Update is called once per frame
    void Update () {
	
	}
}
