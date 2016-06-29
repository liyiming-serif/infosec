using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AttackTopCommandSlot : MonoBehaviour, IDropHandler {

	public TopCommand c {
		get{
			return transform.GetChild (0).gameObject.GetComponent<TopCommand> ();
		}
	}

	#region IDropHandler implementation

	void IDropHandler.OnDrop (PointerEventData eventData)
	{
		if (AttackSpawnCMDHandler.commandBeingSpawned) {
			AcceptCommand (AttackSpawnCMDHandler.commandBeingSpawned);
		} else if (AttackDragHandler.commandBeingDragged){
			AcceptCommand (AttackDragHandler.commandBeingDragged);
		}
	}

	#endregion

	public bool AcceptCommand (TopCommand newCommand){
		if (c.myCode != TopCommand.Code.NoAction) {
			return false;
		} else {
			Destroy (c.gameObject);
			newCommand.transform.SetParent (transform);
            return true;
		}
	}

	public void destructCommand(){
		c.DestructSubCMD ();
		Destroy (c.gameObject);
		TopCommand newC = Instantiate (Resources.Load ("NoActionPrefab", typeof(TopCommand))) as TopCommand;
		newC.transform.SetParent (transform);
	}

	public void ActivateEventTrigger (bool toActive){
		c.GetComponent<CanvasGroup> ().blocksRaycasts = toActive;
		if (c.subCommandRef) {
			c.subCommandRef.GetComponent<CanvasGroup> ().blocksRaycasts = toActive;
		}
	}
}
