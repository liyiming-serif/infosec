using UnityEngine;

public class AppSpawn : MonoBehaviour
{

    [SerializeField]
    Windows appPrefab;
    [SerializeField]
    Vector2 sizeDelta;
    [SerializeField]
    Vector2 localPos;

    public string appName
    {
        get
        {
            return transform.name;
        }
    }

    Animator animator;
    int _id;

    public int id
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }
    public void Dance()
    {
        animator.SetTrigger("dance");
    }

    public void Freeze()
    {
        animator.SetTrigger("freeze");
    }

    void OnMouseDown()
    {
        Windows existed = TaskManager.instance.LookUpWindows(id);
        if (existed)
        {
            TaskManager.instance.SetActiveTask(id);
        }
        else
        {
            //Start a new App if it's not running
            Freeze();
            Launch();
        }
    }

    public void Launch()
    {
        Windows newApp = Instantiate(appPrefab);
        newApp.GetComponent<RectTransform>().sizeDelta = sizeDelta;
        newApp.transform.localPosition = localPos;
        try
        {
            newApp.transform.SetParent(GetComponentInParent<Canvas>().transform);
        }
        catch
        {
            Debug.Log("Canvas is not found.");
        }
        _id = newApp.GetHashCode();
        newApp.Register(_id);
        TaskManager.instance.AddNewTask(newApp, appName);
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        _id = 0;
    }
}