using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeOne : CodingChallengeTemplate {

	override protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode){
		if (subCode == SubCommand.Code.Boss) {
			character.SetEndPosition (playerOutbox.playerPos);
		} else {
			Debug.Log ("Should not reach here.");
		}
	}

	override protected Data[] InitialInboxGenerator(){
		Data[] initialInboxData = new Data[2];
		initialInboxData [0] = Instantiate (Resources.Load("DataBoard", typeof(Data))) as Data;
		initialInboxData [0].dataStr = "O";
		initialInboxData [1] = Instantiate (Resources.Load("DataBoard", typeof(Data))) as Data;
		initialInboxData [1].dataStr = "K";
		return initialInboxData;
	}

	override protected void Reset ()
	{
		base.Reset ();
	}

	override protected void StartRunning ()
	{
		base.StartRunning ();
	}

	void Start () {
		isSucceeded = false;
		playerInbox.Initialise (new Vector2(-344f, 236f), InitialInboxGenerator());
		playerOutbox.Initialise (new Vector2(-117f, -168f));
		runButton.onClick.AddListener (() => StartRunning());
		Reset ();
	}

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
