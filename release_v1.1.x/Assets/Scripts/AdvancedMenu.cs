using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class AdvancedMenu : MonoBehaviour{

	public Dropdown hasOption;
	public Label noOption;

	void Awake(){
		if (this.GetComponent<Dropdown> () != null) {
			hasOption = this.GetComponent<Dropdown> ();
			noOption = null;
		} else if (this.GetComponent<Label> () != null) {
			noOption = this.GetComponent<Label> ();
			hasOption = null;
		} else {
			throw new InvalidCastException ("This is not an advanced menu.");
		}
	}

	public string GetCaptionText()
	{
		string getTxt;
		if (noOption != null) {
			getTxt = noOption.captionText.text;
		} else {
			getTxt = hasOption.captionText.text;
		}
		return getTxt;
	}

	public bool updateOptionList(List<String> newList)
	{
		bool isUpdated = false;
		if (hasOption == null) {
			return isUpdated;
		}
		if (newList.Count == 1) {
			noOption.captionText.text = newList [0];
			isUpdated = true;
		} else if (newList.Count > 1) {
			hasOption.ClearOptions ();
			hasOption.AddOptions (newList);
			isUpdated = true;
		} 
		return isUpdated;
	}

	public void SetVisible(bool setting)
	{
		if (noOption != null) {
			noOption.gameObject.SetActive (setting);
		} else {
			hasOption.gameObject.SetActive (setting);
		}
	}

	public void SetEnable(bool setting){
		if (noOption != null) {
			noOption.enabled = setting;
		} else {
			hasOption.enabled = setting;
		}
	}

	public void Destroy(){
		if (noOption != null) {
			noOption.Destroy ();
		} else {
			Destroy (hasOption.gameObject);
		}
	}
}
