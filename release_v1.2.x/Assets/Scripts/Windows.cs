using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class Windows : GUI
{
    // Components 
    Button minise;
    Button close;

    void Awake()
    {
        base.Awake();
        foreach (Button b in this.GetComponentsInChildren<Button>())
        {
            if (b.transform.name == "Minimise")
            {
                minise = b;
            }
            else if (b.transform.name == "Close")
            {
                close = b;
            }
        }
        Assert.IsNotNull(minise);
        Assert.IsNotNull(close);
        minise.onClick.AddListener(delegate
        {
            SetSelfVisible(false);
            object[] values = new object[2] { id, false };
            manager.SendMessage("SetBarItemVisible", values);
        });
        close.onClick.AddListener(delegate { Destroy(gameObject); manager.SendMessage("KillTask", id); });
    }

    void Start()
    {

    }

}
