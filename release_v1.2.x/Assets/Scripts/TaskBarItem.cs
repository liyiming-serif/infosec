using UnityEngine.UI;
using UnityEngine.Assertions;

public class TaskBarItem : GUI
{
    Button button;

    new void Awake()
    {
        base.Awake();
        button = this.GetComponent<Button>();
        Assert.IsNotNull(button);
        //TODO implement restore
        button.onClick.AddListener(delegate {
            if (!TaskManager.instance.IsActive(id))
            {
                TaskManager.instance.SetActiveTask(id);
            }
        });
    }

}
