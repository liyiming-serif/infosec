using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SubCommand : MonoBehaviour, IPointerClickHandler{

	public enum Code {Zero = 0, One = 1, NoAction, Boss, Distrust};

	public Code myCode;

	#region IPointerClickHandler implementation

	void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
	{
        startArguUpdate();
    }
	#endregion

    public void startArguUpdate()
    {
        transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
        ClickHandler.subCommandToBeChanged = gameObject.GetComponentInParent<TopCommand>();
        ClickHandler.isUpdated = 0;
        ClickHandler.checkUpdate = 0;
        foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot>())
        {
            s.ActivateEventTrigger(false);
        }
    }

}
