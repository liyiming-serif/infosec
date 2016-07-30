using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class TaskBarItem : GUI
{
    Button button;

    void Awake()
    {
        base.Awake();
        button = this.GetComponent<Button>();
        Assert.IsNotNull(button);
        //TODO implement restore
        button.onClick.AddListener(delegate { });
    }

}
