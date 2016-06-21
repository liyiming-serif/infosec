using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NumberPanel : MonoBehaviour, IUpdateNumbers {

	public enum Status
	{
		NotExecuting, Executing, Error
	};

	private Text numberLabel = null;
	private int atLabelNo = -1;

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

	public void SetRunningState(int runningCMDNo, Status status){
		if (atLabelNo > -1) {
			transform.GetChild(atLabelNo).GetComponent<Text>().color = Color.white;
		}
		if (runningCMDNo > -1) {
			if (status == Status.Executing) {
				transform.GetChild(runningCMDNo).GetComponent<Text> ().color = Color.green;
			} else if (status == Status.Error) {
				transform.GetChild(runningCMDNo).GetComponent<Text> ().color = Color.red;
			}
		}
		atLabelNo = runningCMDNo;
	}

	public void ResetRunningState(){
		if (atLabelNo > -1) {
			SetRunningState (-1, Status.NotExecuting);
		}
	}
}

namespace UnityEngine.EventSystems {
	public interface IUpdateNumbers : IEventSystemHandler {
		void UpdateNumbers (bool increaseByOne);
	}
}