using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeOne : MonoBehaviour {

	public enum RunningState {
		Inactive, NotReady, Ready
	};

	[SerializeField] PlayerController player;

	[SerializeField] GameObject playerFeedback;

	[SerializeField] InstructionPanel instructionPan;
	[SerializeField] EnumPanel enumPan;
	[SerializeField] Button runButton;

	[SerializeField] Inbox playerInbox;
	[SerializeField] Outbox playerOutbox;


	/*Game State*/
	RunningState playerState;
	bool isSucceeded;

	int playerCMDNo;
	int playerOldCounter;

	float delaySec;

	void PrepareFeedback(string message, GameObject feedback, Color32 colour){
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = colour;
		feedback.SetActive (true);
	}

	void InformFeedback(string message, GameObject feedback){
		PrepareFeedback (message, feedback, new Color32(83, 144, 195, 212));
	}

	void FailFeedback(string message, GameObject feedback) {
		PrepareFeedback (message, feedback, new Color32(235, 46, 44, 212));
	}

	void SucceedFeedback(string message, GameObject feedback) {
		PrepareFeedback (message, feedback, new Color32(90, 174, 122, 212));		
	}

	void SetCodingModeActive(bool setting){
		foreach (TopCommandSlot s in GameObject.FindObjectsOfType<TopCommandSlot> ()) {
			s.ActivateEventTrigger (false);
		}
	}

	void SetEndPositionBySubCMD(PlayerController character, SubCommand.Code subCode){
		switch(subCode){
		case SubCommand.Code.Boss:
			character.SetEndPosition (playerOutbox.playerPos);
			break;
		default:
			Debug.Log ("Should not reach here.");
			break;
		}
	}

	Data[] InitialInboxGenerator(){
		Data[] initialInboxData = new Data[2];
		initialInboxData [0] = Instantiate (Resources.Load("DataBoard", typeof(Data))) as Data;
		initialInboxData [0].dataStr = "O";
		initialInboxData [1] = Instantiate (Resources.Load("DataBoard", typeof(Data))) as Data;
		initialInboxData [1].dataStr = "K";
		return initialInboxData;
	}

	void StartRunning() {
		Reset ();
		playerState = RunningState.Ready;
		Invoke ("RunPlayerCommand", delaySec);
	}

	void RunPlayerCommand() {
		if (playerState != RunningState.Ready) {
			return;
		}
		playerState = RunningState.NotReady;

		if (playerCMDNo == instructionPan.GetLength ()) {
			enumPan.ResetRunningState ();
			playerCMDNo = 0;
			playerState = RunningState.Inactive;
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

	void Reset () {
		playerState = RunningState.Inactive;
		playerCMDNo = 0;
		playerOldCounter = player.counter;

		enumPan.ResetRunningState ();
		player.ResetAnimator ();
		playerInbox.ResetInbox (InitialInboxGenerator());
		playerOutbox.EmptyAllData();
		playerFeedback.SetActive (false);
	}

	void Start () {
		delaySec = 0.6f;
		isSucceeded = false;
		playerInbox.Initialise (new Vector2 (-293f, 42f), InitialInboxGenerator());
		playerOutbox.Initialise (new Vector2(447f, 149f));
		runButton.onClick.AddListener (() => StartRunning());
		Reset ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playerOldCounter != player.counter) {
			playerOldCounter = player.counter;
			TopCommand runTopCommand = instructionPan.GetTopCommandAt (playerCMDNo);
			switch(runTopCommand.myCode){
			case TopCommand.Code.Inbox:
				player.PickupData (playerInbox.sendFirstData ());
				break;
			case TopCommand.Code.Outbox:
				if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Boss) {
					playerOutbox.AcceptData (player.SendData ());
				} else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust) {
					Debug.Log ("Should not reach here @ 1 Update, Challenge One.");
				}
				break;
			}
			playerCMDNo += 1;
			playerState = RunningState.Ready;
			Invoke ("RunPlayerCommand", delaySec);
		}

	}
}
