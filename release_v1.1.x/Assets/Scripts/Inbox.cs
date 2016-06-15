using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inbox {
	
	public Vector2 playerPos;
	public List<Vector2> boxesPos; 
	public List<Box> boxesRef;

	public Inbox(Vector2 playerPos, Vector2[] boxesPos, Box[] boxesRef = null) {
		this.playerPos = playerPos;
		this.boxesPos = new List<Vector2> (boxesPos);
		this.boxesRef = new List<Box> ();
		if (boxesRef != null) {
			for (int i = 0; i < boxesRef.Length; i++) {
				this.boxesRef.Add (new Box(boxesRef[i].boxRef,boxesRef[i].data,boxesPos[i]));
			}
		}
	}

	public int GetMaxCapacity(){
		return boxesPos.Count;
	}

	public int GetCapacity(){
		return boxesPos.Count - boxesRef.Count;
	}

	public int GetCount(){
		return boxesRef.Count;
	}


	public void ResetInbox(Box[] boxesRef = null) {
		EmptyBoxes ();
		if (boxesRef != null) {
			for (int i = 0; i < boxesRef.Length; i++) {
				this.boxesRef.Add (new Box(boxesRef[i].boxRef,boxesRef[i].data,boxesPos[i]));
			}
		}
	}

	public void EmptyBoxes()
	{
		for (int i = 0; i < boxesRef.Count; i++) {
			boxesRef [i].Destroy ();
		}
		boxesRef.Clear ();
	}
		
	public Box sendBox(int index){
		if (GetCount () <= index) {
			return new Box();
		}
		Box tobeDestroy = boxesRef [index];
		Box cloneBox = new Box (boxesRef [index]);
		tobeDestroy.Destroy ();
		boxesRef.RemoveAt (index);
		for (int i = 0; i < GetCount (); i++) {
			boxesRef [i].boxRef.transform.position = boxesPos [i];
		}
		return cloneBox;
	}


	public Box sendFirstBox(){
		return sendBox (0);
	}

}
