using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickHandler : MonoBehaviour, IPointerClickHandler {

	public static TopCommand subCommandToBeChanged;

	public SubCommand.Code myCode;

	#region IPointerClickHandler implementation
	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		if (subCommandToBeChanged) {
			ExecuteEvents.Execute<IUpdateSubCMDChoice> (subCommandToBeChanged.gameObject, null, (x,y) => x.FinaliseSubCMDChoice(myCode));
			foreach (Slot s in GameObject.FindObjectsOfType<Slot> ()) {
				s.ActivateEventTrigger (true);
			}
			subCommandToBeChanged = null;
		}
	}
	#endregion

}
