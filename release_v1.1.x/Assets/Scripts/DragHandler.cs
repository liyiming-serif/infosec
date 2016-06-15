using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour,  IBeginDragHandler, IDragHandler, IEndDragHandler{

	public static NewBox itemBeingDragged;
	[SerializeField] Canvas inCanvas;
	private Vector3 startPosition;
	private Transform startParent;

	#region IBeginDragHandler implementation

	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{	
		itemBeingDragged = Instantiate(this.GetComponent<Slot>().item);
		itemBeingDragged.transform.SetParent (this.GetComponent<Slot> ().item.transform.parent);
		itemBeingDragged.transform.localScale = new Vector3 (1.25f, 1.25f, 1f);
		startPosition = itemBeingDragged.transform.position;
		startParent = itemBeingDragged.transform.parent;
		itemBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}

	#endregion


	#region IDragHandler implementation
	void IDragHandler.OnDrag (PointerEventData eventData)
	{
		Vector2 positionInCanvas;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(inCanvas.transform as RectTransform, Input.mousePosition, inCanvas.worldCamera, out positionInCanvas);
		itemBeingDragged.transform.position = inCanvas.transform.TransformPoint(positionInCanvas);
	}
	#endregion

	#region IEndDragHandler implementation

	void IEndDragHandler.OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		if (itemBeingDragged.transform.parent == startParent) {
			itemBeingDragged.OnBecameInvisible();
		}
	}

	#endregion
}
