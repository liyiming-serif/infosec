﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AlienC : AIController
{

    public void GetRevenge(CallbackFunct func)
    {
        animator.SetTrigger("alienwins");
        StartCoroutine(AfterAnimation(1f, func));
    }

}