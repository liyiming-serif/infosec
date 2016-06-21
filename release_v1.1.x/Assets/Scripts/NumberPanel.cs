using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NumberPanel : MonoBehaviour, IUpdateNumbers {

	private Text numberLabel;

	#region IUpdateNumbers implementation
	void IUpdateNumbers.UpdateNumbers (bool increaseByOne)
	{
		if (increaseByOne) {
			numberLabel = Instantiate (Resources.Load("NumLabelPrefab", typeof(Text))) as Text;
			numberLabel.text = (GetComponentsInChildren<Text> ().Length + 1).ToString();
			numberLabel.transform.SetParent (transform);
		} else { //decrease by 1
			if (numberLabel) {
				Destroy (numberLabel.gameObject);  //Didn't destroy its Text component yet.
				Text[] numberLabels = GetComponentsInChildren<Text> ();
				if (numberLabels.Length > 1) {
					numberLabel = numberLabels [numberLabels.Length - 2];
				}
			}
		}
	}
	#endregion

	// Use this for initialization
	void Start () {
		numberLabel = null;
	}
}

namespace UnityEngine.EventSystems {
	public interface IUpdateNumbers : IEventSystemHandler {
		void UpdateNumbers (bool increaseByOne);
	}
}