using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Slot : MonoBehaviour, IDropHandler {

    private Domain holding;
    private int _id;

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
            ExecuteEvents.ExecuteHierarchy<ISlotUpdated>(this.gameObject, null, (x, y) => x.NoticeNetworkURLBoard(holding,_id));
        }
    }
    
    void Awake()
    {
        _id = -1;
        holding = null;
    }
}
