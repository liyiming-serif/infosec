using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Inbox : MonoBehaviour{
	
	public Vector2 playerPos;

    [SerializeField]
	private Transform slotsTransform;

	public void Initialise(Vector2 playerPos, Data[] initialData = null){
		this.playerPos = playerPos;
		ResetInbox (initialData);
	}

	public int GetMaxCapacity(){
		return slotsTransform.childCount;
	}

	public int GetCount(){
		int count = 0;
		foreach (Transform slotTransform in slotsTransform) {
			Data item = slotTransform.GetComponent<DataSlot> ().data;
			if (item != null) {
				count++;
			}
		}
		return count;
	}


	public int GetCapacity(){
		return GetMaxCapacity () - GetCount ();
	}


	public void ResetInbox(Data[] initialData = null) {
		EmptyAllData ();
		if (initialData != null) {
			for (int i = 0; i < initialData.Length; i++) {
				initialData[i].transform.SetParent(slotsTransform.GetChild(i));
			}
		}
	}

    public string[] GetCurrentState()
    {
        List<string> tempList = new List<string>();
        foreach (Transform slotTransform in slotsTransform)
        {
            Data item = slotTransform.GetComponent<DataSlot>().data;
            if (item != null)
            {
                tempList.Add(item.dataStr);
            }
            else
            {
                break;
            }
        }
        if (tempList.Count == 0)
        {
            return null;
        }
        else
        {
            return tempList.ToArray();
        }
    }

    public void EmptyAllData()
	{
		foreach (Transform slotTransform in slotsTransform) {
			slotTransform.GetComponent<DataSlot> ().RemoveData ();
		}
	}
		
	public Data sendData(int index){
		if (GetCount () <= index) {
			return null;
		}

        Data item = slotsTransform.GetChild (index).GetComponent<DataSlot> ().data;
 		for (int i = index+1; i < GetCount (); i++) {
			Data moveBox = slotsTransform.GetChild (i).GetComponent<DataSlot>().data;
			moveBox.transform.SetParent (slotsTransform.GetChild (i - 1));
 		}

        return item;
	}


	public Data sendFirstData(){
		return sendData (0);
	}

    void Start (){
		slotsTransform = GetComponent<RectTransform> ();
	}

}
