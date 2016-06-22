using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeTwo : CodingChallengeTemplate {

	[SerializeField] protected GameObject distrustFeedback;
	[SerializeField] protected Outbox distrustOutbox;

	override protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode){
		if (subCode == SubCommand.Code.Boss) {
			character.SetEndPosition (playerOutbox.playerPos);
		} else if (subCode == SubCommand.Code.Distrust){
			character.SetEndPosition (distrustOutbox.playerPos);
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

	protected override bool CheckPlayerReady ()
	{
		if (playerState != RunningState.Ready) {
			return false;
		}
		playerState = RunningState.NotReady;
		return true;
	}

	protected override bool FinishWithoutSucceed ()
	{
		if (playerCMDNo == instructionPan.GetLength ()) {
			switch (hasSolved) {
			case 0:
				FailFeedback ("You need to \"Take\" and \"Give\" for every letter. There are 2 sets to be completed.", playerFeedback);
				break;
			case 1:
				FailFeedback ("You are half-way the goal. Maybe redo \"Take\" and \"Give\" again?", playerFeedback);
				break;
			}
			return true;
		}
		return false;
	}

	protected override void Reset ()
	{
		base.Reset ();
		distrustOutbox.EmptyAllData ();
		distrustFeedback.SetActive (false);
	}

	void Start () {
		playerInbox.Initialise (new Vector2(-344f, 251f), InitialInboxGenerator());
		playerOutbox.Initialise (new Vector2(-83f, -171f));
		distrustOutbox.Initialise (new Vector2(-83f, 132f));
		runButton.onClick.AddListener (() => StartRunning());
		Reset ();
	}

	void Update () {
		Data d;
		if (playerOldCounter != player.counter) {
			playerOldCounter = player.counter;
			TopCommand runTopCommand = instructionPan.GetTopCommandAt (playerCMDNo);
			switch(runTopCommand.myCode){
			case TopCommand.Code.Inbox:
				d = playerInbox.sendFirstData ();
				if (d) {
					player.PickupData (d);
					playerCMDNo += 1;
					playerState = RunningState.Ready;
					Invoke ("RunPlayerCommand", delaySec);
				} else {
					FailFeedback ("There is no data on the line. Try \"Give\" what you had to me.", playerFeedback);
				}
				break;
			case TopCommand.Code.Outbox:
				d = player.SendData ();
				if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Boss) {
					if (d) {
						playerOutbox.AcceptData (d);
						if (hasSolved == 0 && d.dataStr == "O") {
							FailFeedback ("I was not expecting to receive \"O\".", playerFeedback);
						} else if (hasSolved == 0 && d.dataStr == "K") {
							FailFeedback ("You have dropped letter \"O\".", distrustFeedback);
						} else if (hasSolved == 1 && d.dataStr == "K") {
							hasSolved += 1;
							SucceedFeedback ("Thanks for giving me \"K\".", playerFeedback);
						}
					} else {
						FailFeedback ("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
					}
				} else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust){
					if (d) {
						distrustOutbox.AcceptData (d);
						if (hasSolved == 0 && d.dataStr == "O") {
							hasSolved += 1;
							InformFeedback ("Thanks, I got what I need.", distrustFeedback);
							StartCoroutine (DiminishAfterSec (distrustFeedback, 1f));
							playerCMDNo += 1;
							playerState = RunningState.Ready;
							Invoke ("RunPlayerCommand", delaySec);
						} else if (hasSolved == 0 && d.dataStr == "K") {
							FailFeedback ("You have dropped letter\"O\". I was not expecting to receive \"K\".", distrustFeedback);
						} else if (hasSolved == 1 && d.dataStr == "K") {
							FailFeedback ("I hwas not expecting to receive all data.", distrustFeedback);
						}
					}else {
						FailFeedback ("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
					}
				}
				break;
			}
		}

	}

	IEnumerator DiminishAfterSec(GameObject feedback, float time)
	{
		yield return new WaitForSeconds(time);
		feedback.SetActive (false);
	}

}
