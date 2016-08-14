using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

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
        if(holding)
        {
            Destroy(holding.gameObject);
        }
        holding = DragHandler.domainBeingDragged;
        ExecuteEvents.ExecuteHierarchy<URLGenerator>(this.gameObject, null, (x, y) => x.NoticeNetworkURLBoard(DragHandler.domainBeingDragged, _id));
    }
    
    void Awake()
    {
        _id = -1;
    }
}
