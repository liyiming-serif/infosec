using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SubCommand : MonoBehaviour, IPointerClickHandler{

	public enum Code {Zero = 0, One = 1, NoAction, Boss, Distrust};

	public Code myCode;

	[SerializeField]
	GameObject targeting;
	[SerializeField]
	GameObject tetherTail;
	[SerializeField]
	GameObject tetherHead;
	[SerializeField]
	GameObject tetherLine;
	
	Vector2 tetherPoint;
	Vector2 tetherVec;

	#region IPointerClickHandler implementation

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
        startArguUpdate();
    }
	#endregion

    public void startArguUpdate()
    {
		targeting.SetActive(true);
		tetherTail.SetActive(true);
		tetherHead.SetActive(true);
		tetherLine.SetActive(true);
        ClickHandler.subCommandToBeChanged = gameObject.GetComponentInParent<TopCommand>();
        ClickHandler.isUpdated = 0;
        ClickHandler.checkUpdate = 0;
        foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot>())
        {
            s.ActivateEventTrigger(false);
        }
    }

    void Update()
    {
		if(tetherHead.activeInHierarchy)
		{
			tetherPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			tetherVec = tetherHead.transform.position - tetherTail.transform.position;
			float angle = Mathf.Atan2(tetherVec.y, tetherVec.x) * Mathf.Rad2Deg;
			tetherHead.transform.position = tetherPoint;
			tetherLine.transform.eulerAngles = new Vector3(0,0,angle);
			tetherLine.transform.localScale = new Vector2(tetherVec.magnitude/18,1);
		}
	}
}
