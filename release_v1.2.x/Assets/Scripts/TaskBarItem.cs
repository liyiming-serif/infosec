using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;

public class TaskBarItem : MonoBehaviour
{
    Button button;
    public int id;

    TaskManager manager;

    public void Register(int id)
    {
        this.id = id;
    }

    public bool IsRegistered()
    {
        return id != 0;
    }

    // Use this for initialization
    void Start()
    {
        id = 0;
        manager = GameObject.FindObjectOfType<TaskManager>();
        button = this.GetComponent<Button>();
        Assert.IsNotNull(manager);
        Assert.IsNotNull(button);
        button.onClick.AddListener(delegate { });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
