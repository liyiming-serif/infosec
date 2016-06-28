using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ButtonCode
{
    Back = 0, Run = 1, Step = 2, Stop = 3
};

public class DebugPanel : MonoBehaviour
{

    public Button[] debugButtons;

    protected Sprite[] activeImages;
    protected Sprite[] inactiveImages;

    public void SetDebugButtonActive(ButtonCode bcode, bool setting)
    {
        if (setting)
        {
            debugButtons[(int)bcode].image.sprite = activeImages[(int)bcode];
        }
        else
        {
            debugButtons[(int)bcode].image.sprite = inactiveImages[(int)bcode];
        }
        debugButtons[(int)bcode].interactable = setting;
    }

    void Start()
    {
        debugButtons = GetComponentsInChildren<Button>();
        activeImages = new Sprite[4];
        inactiveImages = new Sprite[4];
         
        activeImages[(int)ButtonCode.Run] = Resources.Load<Sprite>("ui_run");
        inactiveImages[(int)ButtonCode.Run] = Resources.Load<Sprite>("ui_run_pressed");
        activeImages[(int)ButtonCode.Back] = Resources.Load<Sprite>("ui_back");
        inactiveImages[(int)ButtonCode.Back] = Resources.Load<Sprite>("ui_back_pressed");
        activeImages[(int)ButtonCode.Stop] = Resources.Load<Sprite>("ui_stop");
        inactiveImages[(int)ButtonCode.Stop] = Resources.Load<Sprite>("ui_stop_pressed");
        activeImages[(int)ButtonCode.Step] = Resources.Load<Sprite>("ui_step");
        inactiveImages[(int)ButtonCode.Step] = Resources.Load<Sprite>("ui_step_pressed");
    }


}
