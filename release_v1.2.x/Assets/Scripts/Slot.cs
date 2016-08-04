using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Slot : MonoBehaviour, IDropHandler {

    Domain holding;

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (!holding)
        {
            holding = DragHandler.domainBeingDragged;
            holding.transform.SetParent(this.transform);
        }
    }
}
