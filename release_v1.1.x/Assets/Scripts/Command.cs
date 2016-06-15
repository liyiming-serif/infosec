using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class Command : MonoBehaviour{

	public enum Status {Error, NotExecuting, Executing};

	/*components interface*/
	public Text cmdNo;
	public Dropdown optionMenu;
	public AdvancedMenu actTo;
	public Status executeStatus;
	public String[] options;
	public String[] owners;
	public String[] memoryIndices;

	public void SetEnable(bool setting) {
		optionMenu.enabled = setting;
		if (actTo != null) {
			actTo.SetEnable (setting);
		}
	}

	public bool IsEnable() {
		return optionMenu.enabled;
	}

	public bool SetStatus(Status s) {
		switch (s) {
		case Status.Executing:
			cmdNo.color = Color.green;
			break;

		case Status.Error:
			cmdNo.color = Color.red;
			break;

		case Status.NotExecuting:
			cmdNo.color = Color.black;
			break;

		}
		executeStatus = s;
		return true;
	}

	public Status GetStatus()
	{
		return executeStatus;
	}

	public void Destroy(){
		Destroy (cmdNo);
		actTo.Destroy ();
		Destroy(optionMenu.gameObject);
	}

	public void UpdateAdvancedMenus () {
		if (optionMenu.options [optionMenu.value].text == "outbox->") {
			actTo.updateOptionList (new List<String>(owners));
			actTo.SetVisible (true);
		} else if (optionMenu.options [optionMenu.value].text == "->copy from" || optionMenu.options [optionMenu.value].text == "copy to->" || optionMenu.options [optionMenu.value].text == "Boss owns") {
			actTo.updateOptionList (new List<String>(memoryIndices));
			actTo.SetVisible (true);
		}
		else {
			actTo.SetVisible (false);
		}
	}

	// Run after Advanced Menus are initialised
	void Awake () {
		executeStatus = Status.NotExecuting;
		cmdNo = this.GetComponentInChildren<Text> ();
		optionMenu = this.GetComponent<Dropdown> ();
		optionMenu.ClearOptions ();
		optionMenu.AddOptions (new List<String> (options));
		actTo = this.GetComponentInChildren<AdvancedMenu> ();
		if (cmdNo == null) {
			throw new InvalidCastException ("Command No. label is not found.");
		}
		if (optionMenu == null) {
			throw new InvalidCastException ("Dropdown is not found.");
		}
		if (actTo == null) {
			throw new InvalidCastException ("Act To Menu not found.");
		}
		optionMenu.onValueChanged.AddListener (delegate {
			this.UpdateAdvancedMenus();	
		});
	}
		
}
