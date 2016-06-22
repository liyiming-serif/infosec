using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Data : MonoBehaviour
{
	public string dataStr{
		get{
			return GetComponentInChildren<Text> ().text;
		}
		set{
			GetComponentInChildren<Text> ().text = value;
		}
	}

}