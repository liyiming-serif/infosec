using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeTwoManager: MonoBehaviour { 

	public enum SpyingState {Available, Unavailable, Stealing, Stolen};

	/*Constants*/
	public const int numPlayerBoxes = 3;
	public const int numSpyBoxes = 0;
	public const int numMemoryBoxes = 1;

	public const int numBoxPositions = 3;
	public const int maxNumCommands = 10;

	public const string bossName = "Boss";
	public const string spyName = "Nikita";
	public const string LocZero = "0";
	public const string LocOne = "1";
	public const char dataHash = '#';
	public const char dataDollar = '$';
	public const char dataBlueE = 'E';

	/*Gameborad interfaces*/
	public GameObject yellowHash;
	public GameObject yellowDollar;
	public GameObject blueE;

	public Command commandTemplate;

	public PlayerController player;
	public PlayerController spy;

	public GameObject bossFeedback;
	public GameObject spyFeedback;

	public GameObject commandContent;
	public Button addButton;
	public Button discardButton;
	public Button runButton;

	/*game logic states*/
	private List<Command> commands;

	private Box[] playerInboxInit;

	private Inbox playerInbox;
	private Inbox spyInbox;

	private Outbox bossOutbox;
	private Outbox spyOutbox;

	private MemoryBar ground;
	private SpyingState[] spyingMemory;
	private int stealingIndex; // -1 means spy is occupied. use as a lock.

	private bool isExecuting;
	private bool isSucceeded;

	private int playerCmd;
	private int playerOldCounter;

	private int spyOldCounter;

	private int index;
	private int countDown;

	private int completeSignal;// hack: half-way completed signal

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
			character.SetEndPosition (ground.getPickupPos(0));
			break;
		case LocOne:
			character.SetEndPosition (ground.getPickupPos(1));
			break;
		}
	}

	void spawnNewCommand(){
		if (commands.Count == 10 || commands[commands.Count-1].optionMenu.value == 0) {
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

	void BoxInMemoryReset(){
		ground.ResetBar ();
		ground.acceptBoxAt (0, new Box(yellowHash, dataHash));
	}

	void StartRunning()
	{
		Reset ();
		spyingMemory [0] = SpyingState.Available; //tell spy to copy the box
		RunCommand ();
	}
		

	void RunCommand()
	{
		if (!isExecuting) {
			return;
		}
		if (playerCmd == commands.Count) {
			playerCmd--;
			switch(completeSignal){
			case 0:
				FailRunning ("Try to outbox the first box to me.", spyFeedback);
				break;
			case 1:
				FailRunning ("I haven't got the yellow '$' box.", spyFeedback);
				break;
			case 2:
				FailRunning ("I expect one blue 'E' box and one yellow '$' box.", bossFeedback);
				break;
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

	void Introduction(){
		Text feedbackText = bossFeedback.GetComponentInChildren<Text> ();
		feedbackText.text = "We need to share the box storages on the ground with my colleague Nikita. She can read the public yellow boxes but not my secrete in blue boxes.";
		Renderer changeColor = bossFeedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(83,144,195,212);
	}


	void FailRunning(string message, GameObject feedback)
		{
			if (!isExecuting) {
				return;
			}
			commands [playerCmd].SetStatus (Command.Status.Error);
			Text feedbackText = feedback.GetComponentInChildren<Text> ();
			feedbackText.text = "Error @ Line 0" + (this.playerCmd+1) +": " + message;
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
	}

	void Reset()
	{
		isExecuting = true;
		ActivateCodingPanel (false);

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

		BoxInMemoryReset ();
		ResetSpyingState ();

		index = 0;
		completeSignal = 0;
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

		countDown = 150;

		Vector2 pickupBoxLoc;
		Vector2 dropdownBoxLoc;
		Vector2[] boxPositions = new Vector2[numBoxPositions];

		// player's locations info
		dropdownBoxLoc = new Vector2 (-293f, 42f);
		boxPositions [0] = new Vector2 (-297f, -51f);
		boxPositions [1] = new Vector2 (-337f, -51f);
		boxPositions [2] = new Vector2 (-377f, -51f);

		playerInboxInit = new Box[numPlayerBoxes];
		playerInboxInit [0] = new Box (yellowDollar, dataDollar);
		playerInboxInit [1] = new Box (blueE, dataBlueE);
		playerInboxInit [2] = new Box (yellowDollar, dataDollar);
		playerInbox = new Inbox (dropdownBoxLoc, boxPositions, playerInboxInit);

		// boss's locations info
		dropdownBoxLoc = new Vector2 (-147f, -39f);
		boxPositions [0] = new Vector2 (-81f, -50f);
		boxPositions [1] = new Vector2 (-39f, -50f);
		boxPositions [2] = new Vector2 (0f, -50f);
		bossOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// spy's locations at inbox info
		pickupBoxLoc = new Vector2 (-293f, -140f);
		boxPositions [0] = new Vector2 (-291f, -227f);
		boxPositions [1] = new Vector2 (-331f, -227f);
		boxPositions [2] = new Vector2 (-377f, -227f);
		spyInbox = new Inbox (pickupBoxLoc, boxPositions);

		// spy's location at outbox info
		boxPositions [0] = new Vector2 (-92f, -227f);
		boxPositions [1] = new Vector2 (-50f, -227f);
		boxPositions [2] = new Vector2 (-10f, -227f);
		dropdownBoxLoc = new Vector2 (-151f, -211f);
		spyOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// memory's location info
		Vector2[] memoryPickupPoses = new Vector2[2]; //2 slots only in the memory
		memoryPickupPoses [0] = new Vector2 (-225f, -39f);
		memoryPickupPoses [1] = new Vector2 (-166f, -39f);
		Vector2[] memoryBoxPoses = new Vector2[2];
		memoryBoxPoses [0] = new Vector2 (-225f,-115f);
		memoryBoxPoses [1] = new Vector2 (-166f,-115f);
		ground = new MemoryBar (memoryPickupPoses, memoryBoxPoses);
		ground.acceptBoxAt (0, new Box(yellowHash, dataHash));

		runButton.onClick.AddListener (new UnityAction (StartRunning));
		addButton.onClick.AddListener (new UnityAction (spawnNewCommand));
		discardButton.onClick.AddListener (new UnityAction (discardLastCommand));

		Introduction ();
		spyFeedback.SetActive (false);

		index = 0;
		completeSignal = 0;
	}

	void Update () {

		// Spying memory bar
			for (int i = 0; i < spyingMemory.Length; i++) {
				if (spyingMemory [i] == SpyingState.Available && stealingIndex == -1) { // -1 means spy is not busy
					spyingMemory [i] = SpyingState.Stealing;
					stealingIndex = index;
					spyOldCounter = spy.counter;
					spy.SetEndPosition (ground.getPickupPos (index));
				}
			}

		if (spyOldCounter != spy.counter) { //Spy movement is done.
			if (spyOldCounter + 1 == spy.counter) {
				spy.PickupBox (ground.cloneBoxAt (stealingIndex, MemoryBar.ReadOwner.Public));
				spy.SetEndPosition (spyOutbox.playerPos);
			} else if (spyOldCounter + 2 == spy.counter) {
				spyOutbox.acceptBox (spy.SendBox());
				spyingMemory[stealingIndex] = SpyingState.Stolen;
				spy.SetEndPosition (spy.initPosition);
			} else if (spyOldCounter + 3 == spy.counter) {
				spyOldCounter = spy.counter;
				stealingIndex = -1;
			}
		}

		if (playerOldCounter != player.counter) { //Player movement is done
			playerOldCounter = player.counter;

			switch (commands [playerCmd].optionMenu.value) {
			case 1:
				if (playerInbox.GetCount () > 0) {
					player.PickupBox (playerInbox.sendFirstBox ());
					playerCmd++;
					RunCommand ();
				} else {
					this.FailRunning ("There is no box on the inbox belt.", bossFeedback);
				}
				break;

			case 2:
				if (!player.IsCarryingBox ()) {
					if (commands [playerCmd].actTo.GetCaptionText () == bossName) {
						this.FailRunning ("No box in your hands.", bossFeedback);
					} else if (commands [playerCmd].actTo.GetCaptionText () == spyName) {
						this.FailRunning ("No box in your hands.", spyFeedback);
					}

				} else {
					if (commands [playerCmd].actTo.GetCaptionText () == bossName) {
						bossOutbox.acceptBox (player.SendBox ());
						if (bossOutbox.GetCount () == 2) {
							if (bossOutbox.boxesRef [0].data != dataBlueE && bossOutbox.boxesRef [1].data != dataBlueE) {
								this.FailRunning ("There should be a blue box 'E' on the belt.", bossFeedback);
							} else {
								if (completeSignal == 2) {
									this.succeedRunning ("Thanks for your hard working!", bossFeedback);
								} else {
									completeSignal = 1;
									playerCmd++;
									RunCommand ();
								}
							}
						} else {
							playerCmd++;
							RunCommand ();
						}
					} else if (commands [playerCmd].actTo.GetCaptionText () == spyName) {
						char receivedData = player.carryingRef.data;
						spyOutbox.acceptBox (player.SendBox ());
						if (receivedData == dataBlueE) {
							this.FailRunning ("Aha! Your boss has a private blue box'" + spyOutbox.boxesRef [spyOutbox.GetCount () - 1].data.ToString () + "'!", spyFeedback);
						} else {
							if (completeSignal == 1) {
								this.succeedRunning ("Thanks for your hard working!", spyFeedback);
							} else {
								completeSignal = 2;
								playerCmd++;
								RunCommand ();
							}
						}
					}
				}
				break;
			}
		}

		if (isSucceeded) {
			if (countDown > 0) {
				countDown--;
			} else {
				SceneManager.LoadScene ("Challenge3");
			}

		}

	}
}