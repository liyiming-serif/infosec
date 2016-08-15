using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CivilianC : AIController
{
    public void BecomeSafe(CallbackFunct func)
    {
        animator.SetTrigger("civilsafe");
        StartCoroutine(AfterAnimation(1f, func));
    }

}