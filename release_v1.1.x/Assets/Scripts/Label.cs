using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Label : MonoBehaviour
{
	public Text captionText;

	public void SetActive(bool setting)
	{
		this.enabled = setting;
		captionText.enabled = setting;
	}

	public bool GetVisible()
	{
		return this.GetVisible ();
	}

	void Awake(){
		captionText = this.gameObject.GetComponentInChildren<Text> ();
		if (captionText == null) {
			throw new InvalidCastException ("This is not a label.");
		}
	}

	public void Destroy(){
		Destroy (captionText);
	}
}

