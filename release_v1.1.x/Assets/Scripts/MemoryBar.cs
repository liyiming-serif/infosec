using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MemoryBar
{
	public enum ReadOwner {Public, Boss};

	public List<Vector2> pickupsPos;
	public List<Vector2> boxesPos; 
	public List<Box> boxesRef;
	public List<ReadOwner> accessControl;

	public MemoryBar(Vector2[] pickupsPos, Vector2[] boxesPos){
		this.pickupsPos = new List<Vector2> (pickupsPos);
		this.boxesPos = new List<Vector2> (boxesPos);
		this.boxesRef = new List<Box> ();
		this.accessControl = new List<ReadOwner> ();
		for (int i = 0; i < GetCount(); i++) {
			this.accessControl.Add (ReadOwner.Public);
			this.boxesRef.Add (new Box());
		}
	}


	int GetCount(){
		return boxesPos.Count;
	}

	public void ResetBar(){
		for (int i = 0; i < GetCount(); i++) {
			accessControl[i] = ReadOwner.Public;
			boxesRef [i].Destroy ();
		}
	}

	public bool hasBoxAt(int index){
//		if (GetCount () <= index) {
//			return false;
//		}
		return (!boxesRef[index].IsDestroyed());
	}

	public bool acceptBoxAt(int index, Box boxRef){
//		if (GetCount () <= index) {
//			return false;
//		}
		if (boxRef.IsDestroyed() || boxRef == null) {
			return false;
		}
		if (boxRef.data == boxesRef [index].data) {
			boxRef.Destroy ();
			return false;
		}
		boxesRef [index].Destroy();
		boxesRef [index] = boxRef;
		boxesRef [index].boxRef.transform.position = boxesPos [index];
		return true;
	}

	public void setOwnershipAt(int index, ReadOwner id){
//		if (GetCount () <= index) {
//			return;
//		}
		accessControl [index] = id;
	}

	public bool CanAccessAt(int index, ReadOwner id){
//		if (GetCount () <= index) {
//			return false;
//		}
		switch (accessControl [index]) {
		case ReadOwner.Public:
			return true;

		case ReadOwner.Boss:
			return (id == ReadOwner.Boss);

		default:
			return false;
		}
	}

	public Box cloneBoxAt(int index, ReadOwner id){
//		if (GetCount () <= index) {
//			return null;
//		}
		Box cloneBox;
		if (!boxesRef[index].IsDestroyed()) {
			
			cloneBox = new Box (boxesRef [index]);
		} else {
			
			cloneBox = new Box();
		}
		return cloneBox;
	}

	public Vector2 getPickupPos(int index){
//		if (GetCount () <= index) {
//			return Vector2.zero;
//		}
		return pickupsPos[index];
	}

}

