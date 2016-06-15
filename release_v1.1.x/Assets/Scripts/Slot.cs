using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {
	public bool droppable;

	public NewBox item {
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).GetComponent<NewBox> ();
			}
			return null;
		}
	}

	public void removeBox(){
		if (item != null) {
			item.OnBecameInvisible ();
			transform.DetachChildren ();
		}
	}

	#region IDropHandler implementation

	void IDropHandler.OnDrop (PointerEventData eventData)
	{
		if (droppable) {
			ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x, y) => x.HasChanged (DragHandler.itemBeingDragged));

		}
	}

	#endregion
}
