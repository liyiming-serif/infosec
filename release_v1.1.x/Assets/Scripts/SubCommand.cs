using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class SubCommand : MonoBehaviour, IPointerClickHandler{

	public enum Code {Boss, Distrust, Zero, One};

	public Code myCode;

	#region IPointerClickHandler implementation

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
		ClickHandler.subCommandToBeChanged = gameObject.GetComponentInParent<TopCommand> ();
		foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot> ()) {
			s.ActivateEventTrigger (false);
		}
	}

	#endregion

}
