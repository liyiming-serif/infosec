using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum RunningState {
	Inactive, NotReady, Ready
};

public class CodingChallengeTemplate : MonoBehaviour {

	[SerializeField] protected AnimatorController player;
	[SerializeField] protected GameObject playerFeedback;

	[SerializeField] protected Inbox playerInbox;
	[SerializeField] protected Outbox playerOutbox;

	[SerializeField] protected InstructionPanel instructionPan;
	[SerializeField] protected EnumPanel enumPan;
	[SerializeField] protected Button runButton;

	/*Game State*/

	protected int hasSolved;

	protected RunningState playerState;
	protected int playerCMDNo;
	protected int playerOldCounter;

	protected float delaySec = 0.6f;

	protected void PrepareFeedback(string message, GameObject feedback, Color32 colour){
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = colour;
		feedback.SetActive (true);
	}

	protected void InformFeedback(string message, GameObject feedback){
		PrepareFeedback (message, feedback, new Color32(83, 144, 195, 212));
	}

	protected void FailFeedback(string message, GameObject feedback) {
		PrepareFeedback (message, feedback, new Color32(235, 46, 44, 212));
		if (playerCMDNo < instructionPan.GetLength ()) {
			enumPan.SetRunningState (playerCMDNo, EnumPanel.Status.Error);
		} else {
			enumPan.SetRunningState (playerCMDNo - 1, EnumPanel.Status.Error);
		}
		playerCMDNo = -1;
		playerState = RunningState.Inactive;
		SetCodingModeActive (true);
	}

	protected void SucceedFeedback(string message, GameObject feedback) {
		PrepareFeedback (message, feedback, new Color32(90, 174, 122, 212));
		enumPan.ResetRunningState ();
		playerCMDNo = -1;
		playerState = RunningState.Inactive;
		SetCodingModeActive (true);
	}

	protected void SetCodingModeActive(bool setting){
		foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot> ()) {
			s.ActivateEventTrigger (setting);
		}
	}

	virtual protected bool CheckPlayerReady(){
		return false;
	}

	virtual protected bool FinishWithoutSucceed(){
		return true;
	}

	//Run command without checking the bound
	protected void RunPlayerCommand() {
		if (!CheckPlayerReady ()) {
			return;
		}
		if (FinishWithoutSucceed ()) {
			return;
		}
		enumPan.SetRunningState (playerCMDNo, EnumPanel.Status.Executing);

		playerOldCounter = player.counter;
		TopCommand topCommandToRun = instructionPan.GetTopCommandAt (playerCMDNo);
		if (topCommandToRun.myCode == TopCommand.Code.Inbox) {
			player.SetEndPosition (playerInbox.playerPos);
		} else if (topCommandToRun.subCommandRef){
			SetEndPositionBySubCMD (player, topCommandToRun.subCommandRef.myCode);
		}
	}

	 virtual protected void Reset(){
		hasSolved = 0;
		playerState = RunningState.Inactive;
		playerCMDNo = -1;
		playerOldCounter = player.counter;

		enumPan.ResetRunningState ();
		player.ResetAnimator ();
		playerInbox.ResetInbox (InitialInboxGenerator());
		playerOutbox.EmptyAllData();
		playerFeedback.SetActive (false);
	}

	virtual protected void StartRunning() {
		Reset ();
		playerCMDNo = 0;
		SetCodingModeActive (false);
		playerState = RunningState.Ready;
		Invoke ("RunPlayerCommand", delaySec);
	}

	virtual protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode){
	
	}

	virtual protected Data[] InitialInboxGenerator (){
		return null;
	}

}
