using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Outbox {
	public Vector2 playerPos;
	public List<Vector2> boxesPos; 
	public List<Box> boxesRef;

	public Outbox(Vector2 playerPos, Vector2[] boxesPos, Box[] boxesRef = null) {
		this.playerPos = playerPos;
		this.boxesPos = new List<Vector2> (boxesPos);
		this.boxesRef = new List<Box> ();
		if (boxesRef != null) {
			foreach (Box b in boxesRef) {
				this.boxesRef.Add (new Box(b));
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

	public void ResetOutbox(Box[] boxesRef) {
		EmptyBoxes ();
		if (boxesRef != null) {
			foreach (Box b in boxesRef) {
				this.boxesRef.Add (new Box(b));
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

	public bool acceptBox(Box boxObject){
		if (boxObject.IsDestroyed ()) {
			
			return false;
		}
		if (GetCapacity() == 0) {
			
			boxesRef.RemoveAt (0);
		}
		boxesRef.Add (boxObject);

		for (int i = 0; i < GetCount (); i++) {
			boxesRef [i].boxRef.transform.position = boxesPos [GetCount()-1 - i];

		}
		return true;
	}
}
