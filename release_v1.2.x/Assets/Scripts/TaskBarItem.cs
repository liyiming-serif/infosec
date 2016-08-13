using UnityEngine.UI;
using UnityEngine.Assertions;

public class TaskBarItem : GUI
{
    Button button;

    void Awake()
    {
        base.Awake();
        button = this.GetComponent<Button>();
        Assert.IsNotNull(button);
        //TODO implement restore
        button.onClick.AddListener(delegate {
            if (!Common.ReturnTManager().IsActive(id))
            {
                Common.ReturnTManager().SetActiveTask(id);
            }
        });
    }

}
