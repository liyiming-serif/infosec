using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Challenge1 : MonoBehaviour {

	[SerializeField] PlayerController player;
	[SerializeField] PlayerController adversary;

	[SerializeField] GameObject adversaryFeedback;
	[SerializeField] GameObject playerFeedback;

	[SerializeField] PlayerInstructionPanel instructionPan;
	[SerializeField] Button runButton;

	[SerializeField] Inbox playerInbox;
	[SerializeField] Outbox playerOutbox;

	void ActivateCodingMode(bool setting){
		foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot> ()) {
			s.ActivateEventTrigger (false);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
