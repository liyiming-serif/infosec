using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DataSlot : MonoBehaviour {

	public Data data {
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).GetComponent<Data> ();
			}
			return null;
		}
	}

	public void RemoveData(){
		if (data) {
			Destroy (data.gameObject);
		}
	}
}
