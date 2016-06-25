using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AttackDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static TopCommand commandBeingDragged;
	private AttackTopCommandSlot originalSlotHost;

	private Canvas codingCanvas;

	#region IBeginDragHandler implementation
	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{
		commandBeingDragged = GetComponent<TopCommand> ();
		commandBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = false;
		AttackInstructionPanel.outsidePanel = false;
		originalSlotHost = commandBeingDragged.GetComponentInParent<AttackTopCommandSlot> ();
	}
	#endregion

	#region IDragHandler implementation
	void IDragHandler.OnDrag (PointerEventData eventData)
	{
		Vector2 positionInCanvas;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(codingCanvas.transform as RectTransform, Input.mousePosition, codingCanvas.worldCamera, out positionInCanvas);
		commandBeingDragged.transform.position = codingCanvas.transform.TransformPoint(positionInCanvas);
	}
	#endregion

	#region IEndDragHandler implementation

	void IEndDragHandler.OnEndDrag (PointerEventData eventData)
	{
		if (AttackInstructionPanel.outsidePanel) {
			originalSlotHost.destructCommand ();
		} else if (commandBeingDragged.GetComponentInParent<AttackTopCommandSlot> () != originalSlotHost) {
			TopCommand newC = Instantiate (Resources.Load ("NoActionPrefab", typeof(TopCommand))) as TopCommand;
			newC.transform.SetParent (originalSlotHost.transform);
			commandBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		} else {
			commandBeingDragged.transform.localPosition = Vector2.zero;
			commandBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
		commandBeingDragged = null;
		originalSlotHost = null;
	}
	#endregion

	void Start(){
        codingCanvas = GameObject.Find("CodingCanvas").GetComponent<Canvas>();
	}
}
