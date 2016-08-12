using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class Outbox : MonoBehaviour {

    Button sendButton;
    Dropdown choice;

    void Awake()
    {
        choice = transform.Find("MailBody/Choice").GetComponent<Dropdown>();
        Assert.IsNotNull(choice);
        sendButton = GetComponentInChildren<Button>();
        Assert.IsNotNull(sendButton);
    }

    void Start()
    {
        sendButton.onClick.AddListener(delegate
        {
            //TODO add functions
        });
    }
    // Update is called once per frame
    void Update () {
	
	}
}
