﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SpawnCMDHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

	public static TopCommand commandBeingSpawned;

	[SerializeField] Canvas codingCanvas;
	[SerializeField] InstructionPanel targetPanel;

	#region IBeginDragHandler implementation
	void IBeginDragHandler.OnBeginDrag (PointerEventData eventData)
	{
		commandBeingSpawned = spawnCommand(gameObject.GetComponent<TopCommand> ().myCode);
		commandBeingSpawned.transform.SetParent (transform.parent);
		commandBeingSpawned.GetComponent<CanvasGroup> ().blocksRaycasts = false;
	}
	#endregion

	#region IDragHandler implementation
	void IDragHandler.OnDrag (PointerEventData eventData)
	{
		Vector2 positionInCanvas;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(codingCanvas.transform as RectTransform, Input.mousePosition, codingCanvas.worldCamera, out positionInCanvas);
		commandBeingSpawned.transform.position = codingCanvas.transform.TransformPoint(positionInCanvas);
	}
	#endregion

	#region IEndDragHandler implementation

	void IEndDragHandler.OnEndDrag (PointerEventData eventData)
	{
		if (InstructionPanel.acceptingNewCommand) {
			commandBeingSpawned.GetComponent<CanvasGroup> ().blocksRaycasts = true;
			InstantiateSubCommand ();
			ExecuteEvents.Execute<IHasFinalised> (targetPanel.gameObject, null, (x, y) => x.HasFinalised());
		} else {
			Destroy (commandBeingSpawned.gameObject);
		}
		commandBeingSpawned = null;
	}

	#endregion

	void InstantiateSubCommand() {
		switch (commandBeingSpawned.myCode) {
		case TopCommand.Code.Outbox:
			commandBeingSpawned.UpdateSubCommand (SubCommand.Code.Boss);
			break;
		case TopCommand.Code.Load:
			commandBeingSpawned.UpdateSubCommand (SubCommand.Code.Zero);
			break;
		case TopCommand.Code.Store:
			commandBeingSpawned.UpdateSubCommand (SubCommand.Code.Zero);
			break;
		}
	}

	TopCommand spawnCommand(TopCommand.Code code){
		TopCommand cloneCommand = null;
		switch (code) {
		case TopCommand.Code.Inbox:
			cloneCommand = Instantiate (Resources.Load ("InboxPrefab", typeof(TopCommand))) as TopCommand;
			break;
		case TopCommand.Code.Outbox:
			cloneCommand = Instantiate (Resources.Load ("OutboxPrefab", typeof(TopCommand))) as TopCommand;
			break;
		case TopCommand.Code.Load:
			cloneCommand = Instantiate (Resources.Load ("LoadPrefab", typeof(TopCommand))) as TopCommand;
			break;
		case TopCommand.Code.Store:
			cloneCommand = Instantiate (Resources.Load ("StorePrefab", typeof(TopCommand))) as TopCommand;
			break;
		}
		cloneCommand.gameObject.AddComponent<DragHandler> ();
		return cloneCommand;
	}
}
