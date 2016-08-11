using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Slot : MonoBehaviour, IDropHandler {

    private Domain holding;
    private int id;

    public Domain GetDomain()
    {
        return holding;
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (!holding)
        {
            holding = DragHandler.domainBeingDragged;
            holding.transform.SetParent(this.transform);
            ExecuteEvents.ExecuteHierarchy<ISlotUpdated>(this, null, (x, y) => x.);
            GameObject.FindObjectOfType<TaskManager>().wins[0].GetComponentInChildren<Domain>(true).gameObject.SetActive(true);
        }
    }
}
