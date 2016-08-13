using UnityEngine;

public class AppSpawn : MonoBehaviour
{

    [SerializeField]
    Windows appPrefab;
    [SerializeField]
    Vector2 sizeDelta;
    [SerializeField]
    Vector2 localPos;
    [SerializeField]
    float delay = 1f; //this is how long in seconds to allow for a double click

    public string appName
    {
        get
        {
            return transform.name;
        }
    }

    Animator animator;
    int id;

    bool one_click;
    bool timer_running;
    float timer_for_double_click;

    public int GetID()
    {
        return id;
    }

    void OnMouseDown()
    {
        if (!one_click) // first click no previous clicks
        {
            one_click = true;

            timer_for_double_click = Time.time; // save the current time
                                                // do one click things;
        }
        else
        {
            one_click = false; // found a double click, now reset
       
            Windows existed = TaskManager.instance.LookUpWindows(id);
            if (existed)
            {
                TaskManager.instance.SetActiveTask(id);
            }
            else
            {
                //Start a new App if it's not running
                animator.SetTrigger("freeze");
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
                id = newApp.GetHashCode();
                newApp.Register(id);
                TaskManager.instance.AddNewTask(newApp);
            }

        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("dance");
        id = 0;
        timer_for_double_click = Time.time;
        one_click = false;
    }

    void Update()
    {
        if (one_click)
        {
            // if the time now is delay seconds more than when the first click started. 
            if ((Time.time - timer_for_double_click) > delay)
            {
                one_click = false; // basically if thats true its been too long and we want to reset so the next click is simply a single click and not a double click.
            }
        }
    }
}