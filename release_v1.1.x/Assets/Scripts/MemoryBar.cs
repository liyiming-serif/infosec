using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryBar : MonoBehaviour
{
	public enum ReadOwner {Distrust, Boss};

	public List<Vector2> pickupsPos;
	public List<ReadOwner> accessControl;

	private Transform slotsTransform;

	public void Initialise(Vector2[] pickupsPos){
		this.pickupsPos = new List<Vector2> (pickupsPos);
		this.accessControl = new List<ReadOwner> ();
		for (int i = 0; i < GetCount(); i++) {
			this.accessControl.Add (ReadOwner.Distrust);
		}
	}

	int GetCount(){
		return slotsTransform.childCount;
	}

	public void EmptyMemoryBar(){
		int count = 0;
		foreach (Transform slot in slotsTransform) {
			slot.GetComponent<DataSlot> ().RemoveData ();
			accessControl[count++] = ReadOwner.Distrust;
		}
	}

	public bool HasDataAt(int index){
		if (GetCount () <= index) {
			return false;
		}
		return (slotsTransform.GetChild (index).GetComponent<DataSlot> ().data != null);
	}

	public void AcceptDataAt(int index, Data box){
		if (GetCount () <= index || box == null) {
			return;
		}
		DataSlot boxBeReplaced = slotsTransform.GetChild (index).GetComponent<DataSlot> ();
		if (boxBeReplaced.data) {
			boxBeReplaced.RemoveData ();
		}
		box.transform.SetParent (slotsTransform.GetChild (index));
	}

	public void SetOwnershipAt(int index, ReadOwner id){
		if (GetCount () <= index) {
			return;
		}
		accessControl [index] = id;
	}

	public bool CanAccessAt(int index, ReadOwner id){
		if (GetCount () <= index) {
			return false;
		}
		switch (accessControl [index]) {
		case ReadOwner.Distrust:
			return true;

		case ReadOwner.Boss:
			return (id == ReadOwner.Boss);

		default:
			return false;
		}
	}

	public Data CloneDataAt(int index){
		if (GetCount () <= index) {
			return null;
		}
		Data boxToReturn = null;
		Data holdingBox = slotsTransform.GetChild (index).GetComponent<DataSlot> ().data;
		if (holdingBox) {
			boxToReturn = Instantiate (holdingBox);
		}
		return boxToReturn;
	}

	public Vector2 getPickupPos(int index){
		if (GetCount () <= index) {
			return Vector2.zero;
		}
		return pickupsPos[index];
	}

	void Start(){
		slotsTransform = GetComponentInChildren<RectTransform> ();
	}
}

