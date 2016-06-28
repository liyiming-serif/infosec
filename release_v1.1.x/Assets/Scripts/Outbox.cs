using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Outbox : MonoBehaviour
{

    public Vector2 playerPos;

    [SerializeField]
    private Transform slotsTransform;

    public void Initialise(Vector2 playerPos)
    {
        this.playerPos = playerPos;
    }

    public int GetMaxCapacity()
    {
        return slotsTransform.childCount;
    }

    public int GetCount()
    {
        int count = 0;
        foreach (Transform slotTransform in slotsTransform)
        {
            Data item = slotTransform.GetComponent<DataSlot>().data;
            if (item != null)
            {
                count++;
            }
        }
        return count;
    }


    public int GetCapacity()
    {
        return GetMaxCapacity() - GetCount();
    }

    public void ResetOutbox(Data[] initialData = null)
    {
        EmptyAllData();
        if (initialData != null)
        {
            for (int i = 0; i < initialData.Length; i++)
            {
                initialData[i].transform.SetParent(slotsTransform.GetChild(GetMaxCapacity() - 1 - i));
            }
        }
    }

    public void EmptyAllData()
    {
        foreach (Transform slotTransform in slotsTransform)
        {
            slotTransform.GetComponent<DataSlot>().RemoveData();
        }
    }

    public string[] GetCurrentState()
    {
        List<string> tempList = new List<string>();
        for(int i = GetMaxCapacity() - 1; i >= 0; i--)
        {
            Data item = slotsTransform.GetChild(i).GetComponent<DataSlot>().data;
            if (item != null)
            {
                tempList.Add(item.dataStr);
            }else
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

    public void AcceptData(Data newData)
    {
        if (newData == null)
        {
            return;
        }
        for (int i = GetCapacity(); i < GetMaxCapacity(); i++)
        {
            Data moveData = slotsTransform.GetChild(i).GetComponent<DataSlot>().data;
            moveData.transform.SetParent(slotsTransform.GetChild(i - 1));
        }
        newData.transform.SetParent(slotsTransform.GetChild(GetMaxCapacity() - 1));
    }

    void Start()
    {
        slotsTransform = GetComponent<RectTransform>();
    }
}
