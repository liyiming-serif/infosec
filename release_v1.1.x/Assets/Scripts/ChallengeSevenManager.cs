using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeSevenManager: MonoBehaviour { 

	public enum SpyingState {Available, Unavailable, Stealing, Stolen};

	/*Constants*/
	public const int numPlayerBoxes = 4;
	public const int numSpyBoxes = 0;
	public const int numMemoryBoxes = 0;

	public const int numBoxPositions = 4;
	public const int maxNumCommands = 15;

	public const string bossName = "Boss";
	public const string spyName = "Nikita";
	public const string LocZero = "0";
	public const string LocOne = "1";
	public const char dataE = 'E';
	public const char dataY = 'Y';
	public const char dataHash = '#';
	public const char dataDollar = '$';
	public const char dataBossHash = '<';
	public const char dataBossDollar = '>';

	/*Gameborad interfaces*/
	private List<Command> commands;

	public GameObject blueE;
	public GameObject blueY;
	public GameObject blueHash;
	public GameObject blueDollar;
	public GameObject yellowHash;
	public GameObject yellowDollar;
	public Command commandTemplate;

	public PlayerController player;
	public PlayerController spy;

	public GameObject bossFeedback;
	public GameObject spyFeedback;

	public GameObject[] blueLighted = new GameObject[2];

	public GameObject commandContent;
	public Button addButton;
	public Button discardButton;
	public Button runButton;

	/*game logic states*/
	private Box[] playerInboxInit;

	private Inbox playerInbox;
	private Inbox spyInbox;

	private Outbox bossOutbox;
	private Outbox spyOutbox;

	private MemoryBar memory;
	private SpyingState[] spyingMemory;
	private int stealingIndex; // -1 means spy is occupied. use as a lock.

	private bool isExecuting;
	private bool isSucceeded;

	private int playerCmd;
	private int playerOldCounter;

	private int spyOldCounter;

	private int index;
	private int[] outboxStates;
	private char receivedData;


	void ActivateCodingPanel(bool setting) {
		foreach (Command c in commands)
		{
			c.SetEnable (setting);
		}
		addButton.enabled = setting;
		discardButton.enabled = setting;
	}

	void setEndPosition(PlayerController character, string captionTxt){
		switch (captionTxt) {
		case bossName:
			character.SetEndPosition (bossOutbox.playerPos);
			break;
		case spyName:
			character.SetEndPosition (spyOutbox.playerPos);
			break;
		case LocZero:
			character.SetEndPosition (memory.getPickupPos(0));
			break;
		case LocOne:
			character.SetEndPosition (memory.getPickupPos(1));
			break;
		}
	}

	void spawnNewCommand(){
		if (commands.Count == maxNumCommands || commands[commands.Count-1].optionMenu.value == 0) {
			return;
		}
		addButton.transform.position = new Vector2 (addButton.transform.position.x, addButton.transform.position.y - 38);
		discardButton.transform.position = new Vector2 (discardButton.transform.position.x, discardButton.transform.position.y - 38);
		Command newCmd = Instantiate (commandTemplate);
		newCmd.UpdateAdvancedMenus ();
		newCmd.cmdNo.text = (1 + commands.Count).ToString();
		newCmd.transform.position = new Vector2 (commands [commands.Count - 1].transform.position.x, commands [commands.Count - 1].transform.position.y - 38);
		newCmd.transform.SetParent (commandContent.transform);
		commands.Add (newCmd);
	}

	void discardLastCommand(){
		if (commands.Count == 1) {
			return;
		}
		addButton.transform.position = new Vector2 (addButton.transform.position.x, addButton.transform.position.y + 38);
		discardButton.transform.position = new Vector2 (discardButton.transform.position.x, discardButton.transform.position.y + 38);
		Command tobeDestroy = commands [commands.Count - 1];
		tobeDestroy.Destroy ();
		commands.RemoveAt (commands.Count -1);
	}

	void ResetSpyingState(){
		for (int i = 0; i < numBoxPositions; i++) {
			spyingMemory [i] = SpyingState.Unavailable;
		}
	}

	void StartRunning()
	{
		Reset ();
		Invoke ("RunCommand", 0.6f);
	}
		

	void RunCommand()
	{
		if (!isExecuting) {
			return;
		}
		if (playerCmd == commands.Count) {
			playerCmd--;
			if (outboxStates [0] < 2) {
				FailRunning ("I haven't got two blue boxes in reverse order", bossFeedback);
			}
			if (outboxStates [1] < 2) {
				FailRunning ("I haven't got two yellow boxes in reverse order", spyFeedback);
			}
			return;
		}

		if (playerCmd > 0) {
			commands [playerCmd-1].SetStatus (Command.Status.NotExecuting);
		}
		int optionIndex = commands [playerCmd].optionMenu.value;
		if (optionIndex == 0) {
			this.FailRunning ("Command is not selected.", bossFeedback);
			return;
		}

		commands [playerCmd].SetStatus (Command.Status.Executing);
		playerOldCounter = player.counter;
		if (optionIndex == 1) {
			player.SetEndPosition (playerInbox.playerPos);
		} else {
			setEndPosition (player, commands [playerCmd].actTo.GetCaptionText ());
		}
	}

	void FailRunning(string message, GameObject feedback)
	{
		commands [playerCmd].SetStatus (Command.Status.Error);
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = "Error @ Line " + (this.playerCmd+1) +": " + message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(235,46,44,212);
		feedback.SetActive (true);

		isExecuting = false;
		ActivateCodingPanel (true);
	}

	void succeedRunning(string message, GameObject feedback)
	{
		if (!isExecuting) {
			return;
		}
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = "Great! " + message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(90,174,122,212);
		feedback.SetActive (true);

		isExecuting = false;
		isSucceeded = true;
		ActivateCodingPanel (true);
	}

	void Reset()
	{
		isExecuting = true;
		ActivateCodingPanel (false);

		commands [playerCmd].SetStatus (Command.Status.NotExecuting);
		playerCmd = 0;
		playerOldCounter = player.counter;
		stealingIndex = -1;
		spyOldCounter = spy.counter;

		player.ResetAnimator ();
		spy.ResetAnimator ();

		playerInbox.ResetInbox(playerInboxInit);
		spyInbox.ResetInbox (null);

		bossOutbox.EmptyBoxes ();
		spyOutbox.EmptyBoxes ();

		bossFeedback.SetActive (false);
		spyFeedback.SetActive (false);

		memory.ResetBar ();
		ResetSpyingState ();

		blueLighted [0].SetActive (false);
		blueLighted [1].SetActive (false);

		for (int i = 0; i < 2; i++) {
			outboxStates [i] = 0;
		}
	}
		
	void Start () {

		commands = new List<Command> ();

		commands.Add (Instantiate(commandTemplate));
		commands [0].UpdateAdvancedMenus ();
		commands [0].cmdNo.text = "1";
		commands [0].transform.position = new Vector2 (313f, 55f);
		commands [0].transform.SetParent (commandContent.transform);
	
		spyingMemory = new SpyingState[numBoxPositions];
		ResetSpyingState ();

		isExecuting = false;
		isSucceeded = false;

		playerCmd = 0;
		playerOldCounter = player.counter;
		stealingIndex = -1;
		spyOldCounter = spy.counter;

		outboxStates = new int[2];
		for (int i = 0; i < 2; i++) {
			outboxStates [i] = 0;
		}

		Vector2 pickupBoxLoc;
		Vector2 dropdownBoxLoc;
		Vector2[] boxPositions = new Vector2[numBoxPositions];

		playerInboxInit = new Box[numPlayerBoxes];
		playerInboxInit [0] = new Box (blueE, dataE);
		playerInboxInit [1] = new Box (blueY, dataY);
		playerInboxInit [2] = new Box (yellowHash, dataHash);
		playerInboxInit [3] = new Box (yellowDollar, dataDollar);

		// player's locations info
		dropdownBoxLoc = new Vector2 (-293f, 42f);
		boxPositions [0] = new Vector2 (-290f, -51f);
		boxPositions [1] = new Vector2 (-330f, -51f);
		boxPositions [2] = new Vector2 (-370f, -51f);
		boxPositions [3] = new Vector2 (-410f, -51f);
		playerInbox = new Inbox (dropdownBoxLoc, boxPositions, playerInboxInit);

		// boss's locations info
		dropdownBoxLoc = new Vector2 (-147f, -39f);
		boxPositions [0] = new Vector2 (-81f, -50f);
		boxPositions [1] = new Vector2 (-39f, -50f);
		boxPositions [2] = new Vector2 (3f, -50f);
		boxPositions [3] = new Vector2 (45f, -50f);
		bossOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// spy's locations at inbox info
		pickupBoxLoc = new Vector2 (-293f, -140f);
		boxPositions [0] = new Vector2 (-291f, -227f);
		boxPositions [1] = new Vector2 (-331f, -227f);
		boxPositions [2] = new Vector2 (-371f, -227f);
		boxPositions [3] = new Vector2 (-411f, -227f);
		spyInbox = new Inbox (pickupBoxLoc, boxPositions);

		// spy's location at outbox info
		boxPositions [0] = new Vector2 (-92f, -227f);
		boxPositions [1] = new Vector2 (-50f, -227f);
		boxPositions [2] = new Vector2 (-10f, -227f);
		boxPositions [3] = new Vector2 (30f, -227f);
		dropdownBoxLoc = new Vector2 (-151f, -211f);
		spyOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// memory's location info
		Vector2[] memoryPickupPoses = new Vector2[2];
		memoryPickupPoses [0] = new Vector2 (-225f, -39f);
		memoryPickupPoses [1] = new Vector2 (-166f, -39f);
		Vector2[] memoryBoxPoses = new Vector2[2];
		memoryBoxPoses [0] = new Vector2 (-225f,-115f);
		memoryBoxPoses [1] = new Vector2 (-166f,-115f);
		memory = new MemoryBar (memoryPickupPoses, memoryBoxPoses);

		runButton.onClick.AddListener (new UnityAction (StartRunning));
		addButton.onClick.AddListener (new UnityAction (spawnNewCommand));
		discardButton.onClick.AddListener (new UnityAction (discardLastCommand));

		bossFeedback.SetActive (false);
		spyFeedback.SetActive (false);

		ResetSpyingState ();
		blueLighted [0].SetActive (false);
		blueLighted [1].SetActive (false);
	}

	void Update () {

		// Spying memory bar
		for (int i = 0; i < spyingMemory.Length; i++) {
			if (spyingMemory[i] == SpyingState.Available && stealingIndex == -1) { // -1 means spy is not busy
				spyingMemory[i] = SpyingState.Stealing;
				stealingIndex = index;
				spyOldCounter = spy.counter;
				spy.SetEndPosition (memory.getPickupPos(index));
			}
		}

		if (spyOldCounter != spy.counter) { //Spy movement is done.
			if (spyOldCounter + 1 == spy.counter) {
				spy.PickupBox (memory.cloneBoxAt (stealingIndex, MemoryBar.ReadOwner.Public));
				spy.SetEndPosition (spyOutbox.playerPos);
			} else if (spyOldCounter + 2 == spy.counter) {
				receivedData = spy.carryingRef.data;
				spyOutbox.acceptBox (spy.SendBox());
				if (receivedData == dataE || receivedData == dataY) {
					FailRunning ("Aha! Your boss has a private box'" + receivedData.ToString () + "'!", spyFeedback);
				} else if (receivedData == dataBossDollar) {
					FailRunning ("Aha! Your boss is processing box'$'!", spyFeedback);
				} else if(receivedData == dataBossHash) {
					FailRunning ("Aha! Your boss is processing box'#'!", spyFeedback);
				}
				spyingMemory[stealingIndex] = SpyingState.Stolen;
				spy.SetEndPosition (spy.initPosition);
			} else if (spyOldCounter + 3 == spy.counter) {
				spyOldCounter = spy.counter;
				stealingIndex = -1;
			}
		}

		if (playerOldCounter != player.counter) { //Player movement is done
			playerOldCounter = player.counter;

			switch (commands [playerCmd].optionMenu.value)
			{
			case 1:
				if (playerInbox.GetCount() > 0) {
					player.PickupBox(playerInbox.sendFirstBox ());
					playerCmd++;
					RunCommand();
				} else {
					this.FailRunning ("There is no box on the inbox belt.", bossFeedback);
				}
				break;

			case 2:
				if (!player.IsCarryingBox()) {
					if (commands [playerCmd].actTo.GetCaptionText () == bossName) {
						this.FailRunning ("No box in your hands.", bossFeedback);
					} else if (commands [playerCmd].actTo.GetCaptionText () == spyName) {
						this.FailRunning ("No box in your hands.", spyFeedback);
					}

				} else {
					if (commands [playerCmd].actTo.GetCaptionText () == bossName) {
						receivedData = player.carryingRef.data;
						bossOutbox.acceptBox (player.SendBox ());
						if (receivedData == dataE) {
							switch (outboxStates [0]) {
							case 0:
								this.FailRunning ("The first box should be '" + dataY.ToString () + "'.", bossFeedback);
								break;
							case 1:
								if (outboxStates [1] == 2) {
									this.succeedRunning ("Thanks for your hard working! End of the game.", bossFeedback);
								} else {
									outboxStates [0]++;
									playerCmd++;
									Invoke ("RunCommand", 0.6f);
								}
								break;
							case 2:
								this.FailRunning ("You sent me more than two blue boxes.", bossFeedback);
								break;
							}
						} else if (receivedData == dataY) {
							switch (outboxStates [0]) {
							case 0:
								outboxStates [0]++;
								playerCmd++;
								Invoke ("RunCommand", 0.6f);
								break;
							case 1:
								this.FailRunning ("The second box should be '" + dataE.ToString () + "'.", bossFeedback);
								break;
							case 2:
								this.FailRunning ("You sent me more than two blue boxes.", bossFeedback);
								break;
							}
						} else {
							this.FailRunning ("I expect 'Y' box then 'E' box.", bossFeedback);
						}
					} else if (commands [playerCmd].actTo.GetCaptionText () == spyName) {
						receivedData = player.carryingRef.data;
						spyOutbox.acceptBox (player.SendBox ());
						if (receivedData == dataHash) {
							switch (outboxStates [1]) {
							case 0:
								this.FailRunning ("The first box should be '" + dataDollar.ToString () + "'.", spyFeedback);
								break;
							case 1:
								if (outboxStates [0] == 2) {
									this.succeedRunning ("Thanks for your hard working! End of the game.", spyFeedback);
								} else {
									outboxStates [1]++;
									playerCmd++;
									Invoke ("RunCommand", 0.6f);
								}
								break;
							case 2:
								this.FailRunning ("You sent me more than two yellow boxes.", spyFeedback);
								break;
							}
						} else if (receivedData == dataDollar) {
							switch (outboxStates [1]) {
							case 0:
								outboxStates [1]++;
								playerCmd++;
								Invoke ("RunCommand", 0.6f);
								break;
							case 1:
								this.FailRunning ("The second box should be '" + dataHash.ToString () + "'.", spyFeedback);
								break;
							case 2:
								this.FailRunning ("You sent me more than two yellow boxes.", spyFeedback);
								break;
							}
						} else if (receivedData == dataE || receivedData == dataY) {
							FailRunning ("Aha! Your boss has a private box'" + receivedData.ToString () + "'!", spyFeedback);
						} else if (receivedData == dataBossDollar) {
							FailRunning ("Aha! Your boss is processing box'$'!", spyFeedback);
						} else if(receivedData == dataBossHash) {
							FailRunning ("Aha! Your boss is processing box'#'!", spyFeedback);
						}
					}
				}
				break;

			case 3:
				int.TryParse (commands [playerCmd].actTo.GetCaptionText (), out index);
				if (!memory.hasBoxAt(index)) {
					this.FailRunning ("No box in this cell.", bossFeedback);
				} else {
					player.PickupBox (memory.cloneBoxAt(index, MemoryBar.ReadOwner.Boss));
					playerCmd++;
					Invoke ("RunCommand", 0.6f);
				}
				break;

			case 4:
				if (!player.IsCarryingBox()) {
					this.FailRunning ("No box in your hands.", bossFeedback);
				} else {
					int.TryParse (commands [playerCmd].actTo.GetCaptionText (), out index);
					bool isAccepted = memory.CanAccessAt (index, MemoryBar.ReadOwner.Public);
					receivedData = player.carryingRef.data;
					if (isAccepted) {
						memory.acceptBoxAt (index, player.CloneBox ());
					} else {
						if (receivedData == dataHash) {// L -> H
							memory.acceptBoxAt (index, new Box (blueHash, dataBossHash));
						} else if (receivedData == dataDollar) {// L -> H
							memory.acceptBoxAt (index, new Box (blueDollar, dataBossDollar));
						} else { // H -> H
							memory.acceptBoxAt (index, player.CloneBox ());
						}
					}
					if (isAccepted) {
						spyingMemory [index] = SpyingState.Available;
					}
					playerCmd++;
					Invoke ("RunCommand", 0.6f);
				}
				break;

			case 5:
				int.TryParse (commands [playerCmd].actTo.GetCaptionText (), out index);
				if (memory.hasBoxAt (index)) {
					FailRunning ("Declare its ownership before using it.", bossFeedback);
				} else {
					memory.setOwnershipAt (index, MemoryBar.ReadOwner.Boss);
					blueLighted [index].SetActive (true);
					playerCmd++;
					Invoke ("RunCommand", 0.6f);
				}
				break;

			}
		}

	}
}