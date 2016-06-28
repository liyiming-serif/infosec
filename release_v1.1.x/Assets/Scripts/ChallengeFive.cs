using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeFive : HackingChallengeTemplate {

	 protected override void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode){
		switch (subCode) {
		case SubCommand.Code.Boss:
			character.SetEndPosition (playerOutbox.playerPos);
			break;
		case SubCommand.Code.Distrust:
			character.SetEndPosition (distrustOutbox.playerPos);
			break;
		case SubCommand.Code.Zero:
			character.SetEndPosition (memoryBar.getPickupPos (0));
			break;
		case SubCommand.Code.One:
			character.SetEndPosition (memoryBar.getPickupPos (1));
			break;
		}
	}
	protected override bool FinishWithoutSucceed ()
	{
		if (distrustCMDNo == enumPan.transform.childCount) {
			if (hasSolved == 0) {
				FailFeedback ("You can load the actual data of \"?\"", playerFeedback);
			} else if(hasSolved == 1){
				FailFeedback ("You are half-way the goal. \"Give\" the value \"Y\" to me.", playerFeedback);
			}

			return true;
		}
		return false;
	}

    protected override void Reset()
    {
        base.Reset();
        Data d = Instantiate(Resources.Load("DataBoard", typeof(Data))) as Data;
        d.dataStr = "?";
        memoryBar.AcceptDataAt(1, d);
    }

    protected Vector2[] InitialMemoryPickupPos() 
	{
		Vector2[] initialPickupPos = new Vector2[2];
		initialPickupPos [0] = new Vector2 (-204f, 43f);
		initialPickupPos [1] = new Vector2 (-116f, 43f);
		return initialPickupPos;
	}

	void Start () {
		distrustTopCode = new TopCommand.Code[2];
		distrustSubCode = new SubCommand.Code[2];
		distrustTopCode [0] = TopCommand.Code.Load;
		distrustSubCode [0] = SubCommand.Code.One;
        distrustTopCode[1] = TopCommand.Code.Outbox;
        distrustSubCode[1] = SubCommand.Code.Distrust;

		playerInbox.Initialise (new Vector2(-344f, 251f), InitialInboxGenerator());
		playerOutbox.Initialise (new Vector2(-83f, -171f));
        distrustInboxPos = new Vector2(-436f, -111f);
        distrustOutbox.Initialise (new Vector2(-83f, 132f));
		memoryBar.Initialise (InitialMemoryPickupPos());
		runButton.onClick.AddListener (() => StartRunning());
		Reset ();
	}

	void Update () {
		Data d;
		if ((playerState != RunningState.Inactive) && (playerOldCounter != player.counter)) {
			playerOldCounter = player.counter;
			TopCommand runTopCommand = instructionPan.GetTopCommandAt (playerCMDNo);
			switch(runTopCommand.myCode){
			case TopCommand.Code.NoAction:
				playerCMDNo += 1;
				playerState = RunningState.Ready;
				Invoke ("RunPlayerCommand", delaySec);
				break;
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
						hasSolved = 2;
						SucceedFeedback ("Well done!", playerFeedback);
					} else {
						FailFeedback ("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
					}
				} else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust){
					if (d) {
						distrustOutbox.AcceptData (d);
						FailFeedback ("I was not expecting to receive \"Y\" twice.", distrustFeedback);
					} else {
						FailFeedback ("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
					}
				}
				break;
			case TopCommand.Code.Load:
				d = memoryBar.CloneDataAt ((int) runTopCommand.subCommandRef.myCode);
				if (d) {
					d.dataStr = "Y";
					player.PickupData (d);
					hasSolved = 1;
					playerCMDNo += 1;
					playerState = RunningState.Ready;
					Invoke ("RunPlayerCommand", delaySec);
				} else {
					FailFeedback ("No data can be loaded from it.", playerFeedback);
				}
				break;
			case TopCommand.Code.Store:
				d = player.CloneData ();
				if (d) {
					memoryBar.AcceptDataAt ((int) runTopCommand.subCommandRef.myCode, d);
					playerCMDNo += 1;
					playerState = RunningState.Ready;
					Invoke ("RunPlayerCommand", delaySec);
				} else {
					FailFeedback ("No data can you store on the ground.", playerFeedback);
				}
				break;
			}
		}

		if ((distrustPlayerState != RunningState.Inactive) && (distrustOldCounter != distrust.counter)) {
			distrustOldCounter = distrust.counter;
			switch (distrustTopCode[distrustCMDNo]) {
			case TopCommand.Code.NoAction:
				break;
			case TopCommand.Code.Inbox:
				d = Instantiate (Resources.Load ("DataBoard", typeof(Data))) as Data;
				d.dataStr = "?";
				distrust.PickupData (d);
				break;
			case TopCommand.Code.Outbox:
				d = distrust.SendData ();
				if (distrustSubCode [distrustCMDNo] == SubCommand.Code.Boss) {
					playerOutbox.AcceptData (d);
				} else if (distrustSubCode [distrustCMDNo] == SubCommand.Code.Distrust) {
					distrustOutbox.AcceptData (d);
				}
				break;
			case TopCommand.Code.Load:
				distrust.PickupData (memoryBar.CloneDataAt ((int)distrustSubCode [distrustCMDNo]));
				break;
			case TopCommand.Code.Store:
				memoryBar.AcceptDataAt ((int)distrustSubCode [distrustCMDNo], distrust.CloneData());
				break;
			}
			distrustCMDNo += 1;
			distrustPlayerState = RunningState.Ready;
			Invoke ("RunPlayerCommand", delaySec);
		}

	}
}
