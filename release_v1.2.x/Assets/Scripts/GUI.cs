using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class GUI : MonoBehaviour {

    protected int id;
    protected TaskManager manager;

    public void Register(int newid)
    {
        id = newid;
    }

    public bool IsRegistered()
    {
        return id != 0;
    }

    public bool IsTarget(int id)
    {
        return id == this.id;
    }

    public int GetID()
    {
        return id;
    }

    public void SetSelfVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    protected void Awake()
    {
        id = 0;
        manager = GameObject.FindObjectOfType<TaskManager>();
        Assert.IsNotNull(manager);
    }

}
