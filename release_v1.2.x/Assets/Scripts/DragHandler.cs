using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static Domain domainBeingDragged;

    private Transform originalParent;
    private Vector2 originalPos;
    private Canvas desktop;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        domainBeingDragged = gameObject.GetComponent<Domain>();
        domainBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = false;
        originalParent = domainBeingDragged.transform.parent;
        originalPos = domainBeingDragged.transform.localPosition;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 positionInCanvas;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(desktop.transform as RectTransform, Input.mousePosition, desktop.worldCamera, out positionInCanvas);
        domainBeingDragged.transform.position = desktop.transform.TransformPoint(positionInCanvas);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if(domainBeingDragged.transform.parent == originalParent)
        {
            domainBeingDragged.transform.localPosition = originalPos;
        }
        domainBeingDragged.GetComponent<CanvasGroup>().blocksRaycasts = true;
        domainBeingDragged = null;
    }

    // Use this for initialization
    void Start()
    {
        desktop = GameObject.Find("Desktop").GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
