using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class AttackCommand : MonoBehaviour{

	/*components interface*/
	public Text cmdNo;
	public Dropdown optionMenu;
	public AdvancedMenu actTo;
	public Command.Status executeStatus;
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

	public bool SetStatus(Command.Status s) {
		switch (s) {
		case Command.Status.Executing:
			cmdNo.color = Color.green;
			break;

		case Command.Status.Error:
			cmdNo.color = Color.red;
			break;

		case Command.Status.NotExecuting:
			cmdNo.color = Color.black;
			break;

		}
		executeStatus = s;
		return true;
	}

	public Command.Status GetStatus()
	{
		return executeStatus;
	}

	public void Destroy(){
		Destroy (cmdNo);
		actTo.Destroy ();
		Destroy(optionMenu);
	}

	public void UpdateAdvancedMenus () {
		if (optionMenu.options [optionMenu.value].text == "Outbox To") {
			actTo.updateOptionList (new List<String>(owners));
			actTo.SetVisible (true);
		} else if (optionMenu.options [optionMenu.value].text == "Copy From" || optionMenu.options [optionMenu.value].text == "Copy To") {
			actTo.updateOptionList (new List<String>(memoryIndices));
			actTo.SetVisible (true);
		}
		else {
			actTo.SetVisible (false);
		}
	}

	// Run after Advanced Menus are initialised
	void Awake () {
		executeStatus = Command.Status.NotExecuting;
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
