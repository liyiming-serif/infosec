using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class AttackInstructionPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public static bool outsidePanel;

	#region IPointerEnterHandler implementation

	void IPointerEnterHandler.OnPointerEnter (PointerEventData eventData)
	{
		if (AttackDragHandler.commandBeingDragged || AttackSpawnCMDHandler.commandBeingSpawned) {
			outsidePanel = false;
		}
	}

	#endregion

	#region IPointerExitHandler implementation

	void IPointerExitHandler.OnPointerExit (PointerEventData eventData)
	{
		if (AttackDragHandler.commandBeingDragged || AttackSpawnCMDHandler.commandBeingSpawned) {
			outsidePanel = true;
		}

	}

	#endregion

	public TopCommand GetTopCommandAt (int index){
		return transform.GetChild (index).GetComponent<AttackTopCommandSlot> ().c;
	}

}
	