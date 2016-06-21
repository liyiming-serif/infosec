using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour {

	public Data data {
		get{
			if (transform.childCount > 0) {
				return transform.GetChild (0).GetComponent<Data> ();
			}
			return null;
		}
	}
}
