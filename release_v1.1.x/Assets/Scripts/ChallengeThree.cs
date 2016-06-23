using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChallengeThree : CodingChallengeTemplate {

	[SerializeField] protected GameObject distrustFeedback;
	[SerializeField] protected Outbox distrustOutbox;
	[SerializeField] protected MemoryBar memoryBar;

	override protected void SetEndPositionBySubCMD(AnimatorController character, SubCommand.Code subCode){
		if (subCode == SubCommand.Code.Boss) {
			character.SetEndPosition (playerOutbox.playerPos);
		} else if (subCode == SubCommand.Code.Distrust) {
			character.SetEndPosition (distrustOutbox.playerPos);
		} else if (subCode == SubCommand.Code.Zero) {
			character.SetEndPosition (new Vector2 (-202f, 52f));
		} else if (subCode == SubCommand.Code.One) {
			character.SetEndPosition (new Vector2 (-116f, 52f));
		}
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
			if (hasSolved == 0 || hasSolved == 2) {
				FailFeedback ("You need to \"Load\" from \"0\" and \"Give\" it to me.", playerFeedback);
			} else if (hasSolved == 1) {
				FailFeedback ("You need to \"Load\" from \"0\" and \"Give\" it to me.", distrustFeedback);
			}
			return true;
		}
		return false;
	}

	protected Vector2[] InitialMemoryPickupPos() 
	{
		Vector2[] initialPickupPos = new Vector2[2];
		initialPickupPos [0] = new Vector2 (-204f, -43f);
		initialPickupPos [1] = new Vector2 (-116f, -43f);
		return initialPickupPos;
	}
		
	protected override void Reset ()
	{
		base.Reset ();
		memoryBar.EmptyMemoryBar ();
		Data initialData = Instantiate (Resources.Load("DataBoard", typeof(Data))) as Data;
		initialData.dataStr = "O";
		memoryBar.AcceptDataAt (0, initialData);
		distrustOutbox.EmptyAllData ();
		distrustFeedback.SetActive (false);
	}

	void Start () {
		playerInbox.Initialise (new Vector2(-344f, 251f), InitialInboxGenerator());
		playerOutbox.Initialise (new Vector2(-83f, -171f));
		distrustOutbox.Initialise (new Vector2(-83f, 132f));
		memoryBar.Initialise (InitialMemoryPickupPos());
		Data initialData = Instantiate (Resources.Load("DataBoard", typeof(Data))) as Data;
		initialData.dataStr = "O";
		memoryBar.AcceptDataAt (0, initialData);
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
							hasSolved = 1;
							InformFeedback ("Thanks, I got what I need.", playerFeedback);
							StartCoroutine (DiminishAfterSec (playerFeedback, 1f));
							playerCMDNo += 1;
							playerState = RunningState.Ready;
							Invoke ("RunPlayerCommand", delaySec);
						} else if (hasSolved == 2 && d.dataStr == "O") {
							hasSolved = 3;
							SucceedFeedback ("Well done!", playerFeedback);
						} else if (hasSolved == 1 && d.dataStr == "O") {
							FailFeedback ("I was not expecting to receive \"O\" twice.", playerFeedback);
						}
					} else {
						FailFeedback ("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
					}
				} else if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Distrust){
					if (d) {
						distrustOutbox.AcceptData (d);
						if (hasSolved == 0 && d.dataStr == "O") {
							hasSolved = 2;
							InformFeedback ("Thanks, I got what I need.", distrustFeedback);
							StartCoroutine (DiminishAfterSec (distrustFeedback, 1f));
							playerCMDNo += 1;
							playerState = RunningState.Ready;
							Invoke ("RunPlayerCommand", delaySec);
						} else if (hasSolved == 2 && d.dataStr == "O") {
							FailFeedback ("I was not expecting to receive \"O\" twice.", distrustFeedback);
						} else if (hasSolved == 1 && d.dataStr == "O") {
							hasSolved = 3;
							SucceedFeedback ("Well done!", distrustFeedback);
						}
					} else {
						FailFeedback ("No data can you give to me. Please \"Take\" before \"Giving\" me data.", playerFeedback);
					}
				}
				break;
			case TopCommand.Code.Load:
				if (runTopCommand.subCommandRef.myCode == SubCommand.Code.Zero) {
					d = memoryBar.CloneDataAt (0);
				} else {
					d = memoryBar.CloneDataAt (1);
				}
				if (d) {
					player.PickupData (d);
					playerCMDNo += 1;
					playerState = RunningState.Ready;
					Invoke ("RunPlayerCommand", delaySec);
				} else {
					FailFeedback ("No data can be loaded from it.", playerFeedback);
				}
				break;
			}
		}

	}

}
