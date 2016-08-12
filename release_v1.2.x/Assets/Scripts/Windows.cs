using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class Windows : GUI, IHasTitle
{
    // Components 
    Button minise;
    Button close;

    [SerializeField]
    string title;

    public string GetTitle()
    {
        return title;
    }

    void Awake()
    {
        base.Awake();
        foreach (Button b in transform.Find("WindowsOp").GetComponentsInChildren<Button>())
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
            Common.ReturnTManager().SetInactiveTask(id);
        });
        close.onClick.AddListener(delegate
        {
            Destroy(gameObject);
            Common.ReturnTManager().KillTask(id);
        });
    }

}
