using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class Windows : MonoBehaviour
{

    // Components 
    Button minise;
    Button close;
    TaskManager manager;
    public int id;

    // This must be called before calling other methods
    public void Register(int id)
    {
        this.id = id;

    }

    public bool IsRegistered()
    {
        return id != 0;
    }

    void SetSelfVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    // Use this for initialization
    void Start()
    {
        id = 0;
        manager = GameObject.FindObjectOfType<TaskManager>();
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
        Assert.IsNotNull(manager);
        Assert.IsNotNull(minise);
        Assert.IsNotNull(close);

        minise.onClick.AddListener(delegate
        {
            SetSelfVisible(false);
            object[] values = new object[2] {id, false};
            manager.SendMessage("SetBarItemVisible", values);
        });
        close.onClick.AddListener(delegate { Destroy(gameObject); manager.SendMessage("KillTask", id); });
    }

}
