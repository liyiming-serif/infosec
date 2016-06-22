using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static TopCommand commandBeingDragged;

	private Canvas codingCanvas;
	private NumberPanel numberPanel;
	private InstructionPanel playerInstructionPanel;

	#region IBeginDragHandler implementation
	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{
		commandBeingDragged = gameObject.GetComponent<TopCommand> ();
		commandBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = false;
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
		if (InstructionPanel.outsidePanel) {
			ExecuteEvents.Execute<IUpdateNumbers> (numberPanel.gameObject, null, (x,y) => x.UpdateNumbers(false));
		} else {
			commandBeingDragged.GetComponent<CanvasGroup> ().blocksRaycasts = true;
		}
		ExecuteEvents.Execute<IHasFinalised> (playerInstructionPanel.gameObject, null, (x,y) => x.HasFinalised());
		commandBeingDragged = null;
	}
	#endregion

	void Start(){
		playerInstructionPanel = GameObject.Find ("InstructionPanel").GetComponent<InstructionPanel> ();
		numberPanel = GameObject.Find ("NumberPanel").GetComponent<NumberPanel> ();
		codingCanvas = GameObject.Find("CodingCanvas").GetComponent<Canvas>();
	}
}
