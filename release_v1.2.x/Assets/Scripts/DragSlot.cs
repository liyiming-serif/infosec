using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSlot : MonoBehaviour, IDropHandler
{

    Domain _holding;
    int _id;

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
        if(!holding)
        {
            holding = DragHandler.domainBeingDragged;
        }
    }
    
    void Awake()
    {
        _id = -1;
    }

}
