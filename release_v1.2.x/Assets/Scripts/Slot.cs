using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class Slot : MonoBehaviour, IDropHandler {

    private Domain _holding;
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

    public Domain holding
    {
        get
        {
            return GetComponentInChildren<Domain>();
        }
        set
        {
            value.transform.SetParent(this.transform);
        }
    }
    
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        if (!holding)
        {
            holding = DragHandler.domainBeingDragged;
            ExecuteEvents.ExecuteHierarchy<ISlotUpdated>(this.gameObject, null, (x, y) => x.NoticeNetworkURLBoard(holding,_id));
        }
    }
    
    void Awake()
    {
        _id = -1;
    }
}
