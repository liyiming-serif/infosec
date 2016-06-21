using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SubCommand : MonoBehaviour, IPointerClickHandler{

	public enum Code {Boss, Distrust};

	public Code myCode;

	#region IPointerClickHandler implementation

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		ClickHandler.subCommandToBeChanged = gameObject.GetComponentInParent<TopCommand> ();
		foreach (Slot s in GameObject.FindObjectsOfType<Slot> ()) {
			s.ActivateEventTrigger (false);
		}
	}

	#endregion

}
