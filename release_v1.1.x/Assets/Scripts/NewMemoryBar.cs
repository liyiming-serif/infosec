using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewMemoryBar : MonoBehaviour
{
	public enum ReadOwner {Public, Boss};

	public List<Vector2> pickupsPos;
	public List<ReadOwner> accessControl;

	[SerializeField] Transform slotsTransform;

	public void Initialise(Vector2[] pickupsPos){
		this.pickupsPos = new List<Vector2> (pickupsPos);
		this.accessControl = new List<ReadOwner> ();
		for (int i = 0; i < GetCount(); i++) {
			this.accessControl.Add (ReadOwner.Public);
		}
	}

	int GetCount(){
		return slotsTransform.childCount;
	}

	public void EmptyMemoryBar(){
		int count = 0;
		foreach (Transform slot in slotsTransform) {
			slot.GetComponent<Slot> ().removeBox ();
			accessControl[count++] = ReadOwner.Public;
		}
	}

	public bool HasBoxAt(int index){
		if (GetCount () <= index) {
			return false;
		}
		return (slotsTransform.GetChild (index).GetComponent<Slot> ().item != null);
	}

	public void AcceptBoxAt(int index, NewBox box){
		if (GetCount () <= index || box == null) {
			return;
		}
		NewBox boxBeReplaced = slotsTransform.GetChild (index).GetComponent<Slot> ().item;
		if (boxBeReplaced != null) {
			boxBeReplaced.OnBecameInvisible ();
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
		case ReadOwner.Public:
			return true;

		case ReadOwner.Boss:
			return (id == ReadOwner.Boss);

		default:
			return false;
		}
	}

	public NewBox CloneBoxAt(int index){
		if (GetCount () <= index) {
			return null;
		}
		NewBox boxToReturn = null;
		NewBox holdingBox = slotsTransform.GetChild (index).GetComponent<Slot> ().item;
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

}

