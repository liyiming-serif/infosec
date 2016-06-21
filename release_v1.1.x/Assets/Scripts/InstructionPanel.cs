using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class InstructionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IHasFinalised {

	public static bool acceptingNewCommand;
	public static bool outsidePanel;

	[SerializeField] EnumPanel targetPanel;
	[SerializeField] int spacing;
	[SerializeField] int maxCommands;

	private int offset;
	private TopCommandSlot emptySlot;

	#region IPointerEnterHandler implementation

	void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
	{
		if (DragHandler.commandBeingDragged) {
			outsidePanel = false;
		}

		if (SpawnCMDHandler.commandBeingSpawned) {
			if (GetComponentsInChildren<TopCommandSlot> ().Length == maxCommands) {
				return;
			}
			acceptingNewCommand = true;
			emptySlot = Instantiate (Resources.Load("CSlotPrefab", typeof(TopCommandSlot))) as TopCommandSlot;
			emptySlot.transform.SetParent (transform);
			ExecuteEvents.Execute<IUpdateNumbers> (targetPanel.gameObject, null, (x,y) => x.UpdateNumbers(true));
		}
	}

	#endregion

	#region IPointerExitHandler implementation

	void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
	{
		if (DragHandler.commandBeingDragged) {
			outsidePanel = true;
		}

		if (SpawnCMDHandler.commandBeingSpawned) {
			if (GetComponentsInChildren<TopCommandSlot> ().Length == maxCommands) {
				return;
			}
			ExecuteEvents.Execute<IUpdateNumbers> (targetPanel.gameObject, null, (x,y) => x.UpdateNumbers(false));
			emptySlot.destructCommand ();
			Destroy (emptySlot.gameObject);
			acceptingNewCommand = false;
		}

	}

	#endregion

	#region IHasFinalised implementation
	void IHasFinalised.HasFinalised ()
	{
		if (DragHandler.commandBeingDragged) {
			if (outsidePanel) {
				TopCommandSlot tobeDiscarded = DragHandler.commandBeingDragged.GetComponentInParent<TopCommandSlot> ();
				tobeDiscarded.destructCommand ();
				Destroy (tobeDiscarded.gameObject);
				outsidePanel = false;
			} else {
				DragHandler.commandBeingDragged.transform.localPosition = Vector2.zero;
			}

		}

		if (SpawnCMDHandler.commandBeingSpawned) {
			emptySlot.AcceptCommand (SpawnCMDHandler.commandBeingSpawned);
			acceptingNewCommand = false;
			emptySlot = null;	
		}
	}
	#endregion

	public int GetLength() {
		return GetComponentsInChildren<TopCommandSlot> ().Length;
	}

	public TopCommand GetTopCommandAt (int index){
		return transform.GetChild (index).GetComponent<TopCommandSlot> ().c;
	}

	void AdjustEmptySpace(TopCommandSlot moveSlot, TopCommand movingCommand){
		//Dynamically adjust the position of emptySpace
		int emptySpaceIndex = (((int)movingCommand.transform.position.y) - offset) / spacing;
		if (emptySpaceIndex < 0) {
			moveSlot.transform.SetAsFirstSibling ();
		} else if (emptySpaceIndex >= GetComponentsInChildren<TopCommandSlot> ().Length) {
			moveSlot.transform.SetSiblingIndex (GetComponentsInChildren<TopCommandSlot> ().Length - 1);
		} else {
			moveSlot.transform.SetSiblingIndex (emptySpaceIndex);
		}
	}

	void Start () {
		acceptingNewCommand = false;

		emptySlot = null;
		spacing = -50;
		offset = (int)transform.position.y; // !!! Set Pivot Anchor (x,y) = (0,1)
		maxCommands = 6;

	}


	// Update is called once per frame
	void Update () {
		if (DragHandler.commandBeingDragged) {
			AdjustEmptySpace (DragHandler.commandBeingDragged.GetComponentInParent<TopCommandSlot>(), DragHandler.commandBeingDragged);
		}

		if (acceptingNewCommand) {
			AdjustEmptySpace (this.emptySlot, SpawnCMDHandler.commandBeingSpawned);
		}
	}
}

namespace UnityEngine.EventSystems {
	public interface IHasFinalised : IEventSystemHandler {
		void HasFinalised ();
	}
}