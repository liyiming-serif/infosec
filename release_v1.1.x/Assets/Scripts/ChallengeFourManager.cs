using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeFourManager: MonoBehaviour { 

	public enum SpyingState {Available, Unavailable, Stealing, Stolen};

	/*Constants*/
	public const int numPlayerBoxes = 2;
	public const int numSpyBoxes = 0;
	public const int numMemoryBoxes = 0;

	public const int numBoxPositions = 2;
	public const int maxNumCommands = 10;

	public const string bossName = "Boss";
	public const string spyName = "Nikita";
	public const string LocZero = "0";
	public const string LocOne = "1";
	public const char dataHash = '#';
	public const char dataDollar = '$';

	/*Gameborad interfaces*/
	private List<Command> commands;

	public GameObject yellowHash;
	public GameObject yellowDollar;
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
	private int countDown;


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

	void StartRunning()
	{
		Reset ();
		RunCommand ();
	}
		

	void RunCommand()
	{
		if (!isExecuting) {
			return;
		}
		if (playerCmd == commands.Count) {
			playerCmd--;
			this.FailRunning ("I haven't got the yellow boxes sorted in order!", bossFeedback);
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

		memory.ResetBar ();
		ResetSpyingState ();
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

		playerInboxInit = new Box[numPlayerBoxes];
		playerInboxInit [0] = new Box (yellowHash, dataHash);
		playerInboxInit [1] = new Box (yellowDollar, dataDollar);

		// player's locations info
		dropdownBoxLoc = new Vector2 (-293f, 42f);
		boxPositions [0] = new Vector2 (-297f, -51f);
		boxPositions [1] = new Vector2 (-337f, -51f);

		playerInbox = new Inbox (dropdownBoxLoc, boxPositions, playerInboxInit);

		// boss's locations info
		dropdownBoxLoc = new Vector2 (-147f, -39f);
		boxPositions [0] = new Vector2 (-81f, -50f);
		boxPositions [1] = new Vector2 (-39f, -50f);
		bossOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// spy's locations at inbox info
		pickupBoxLoc = new Vector2 (-293f, -140f);
		boxPositions [0] = new Vector2 (-291f, -227f);
		boxPositions [0] = new Vector2 (-331f, -227f);
		spyInbox = new Inbox (pickupBoxLoc, boxPositions);

		// spy's location at outbox info
		boxPositions [0] = new Vector2 (-92f, -227f);
		boxPositions [1] = new Vector2 (-50f, -227f);
		dropdownBoxLoc = new Vector2 (-151f, -211f);
		spyOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// memory's location info
		Vector2[] memoryPickupPoses = new Vector2[numBoxPositions];
		memoryPickupPoses [0] = new Vector2 (-225f, -39f);
		memoryPickupPoses [1] = new Vector2 (-166f, -39f);
		boxPositions [0] = new Vector2 (-225f,-115f);
		boxPositions [1] = new Vector2 (-166f,-115f);
		memory = new MemoryBar (memoryPickupPoses, boxPositions);

		runButton.onClick.AddListener (new UnityAction (StartRunning));
		addButton.onClick.AddListener (new UnityAction (spawnNewCommand));
		discardButton.onClick.AddListener (new UnityAction (discardLastCommand));

		bossFeedback.SetActive (false);
		spyFeedback.SetActive (false);

		ResetSpyingState ();

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
						char receivedData = player.carryingRef.data;
						bossOutbox.acceptBox (player.SendBox ());
						if (receivedData == '#') {
							if (bossOutbox.boxesRef[0].data == dataDollar) {
								this.succeedRunning ("Thanks for your hard working!", bossFeedback);

							} else {
								this.FailRunning ("The first box should be '$'.", bossFeedback);
							}
						} else if (receivedData == '$') {
							if (bossOutbox.GetCount() == 1) {
								playerCmd++;
								RunCommand ();
							} else {
								this.FailRunning ("The second box should be '#'.", bossFeedback);
							}
						}
					} else if (commands [playerCmd].actTo.GetCaptionText () == spyName) {
						spyOutbox.acceptBox (player.SendBox ());
						this.FailRunning ("Thanks, but you don't work for me now.", spyFeedback);
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
					RunCommand ();
				}
				break;

			case 4:
				if (!player.IsCarryingBox()) {
					this.FailRunning ("No box in your hands.", bossFeedback);
				} else {
					int.TryParse (commands [playerCmd].actTo.GetCaptionText (), out index);
					bool isAccepted = memory.acceptBoxAt (index, player.CloneBox());
					if (isAccepted && memory.CanAccessAt (index, MemoryBar.ReadOwner.Public)) {
						spyingMemory [index] = SpyingState.Available;
					}
					playerCmd++;
					RunCommand ();
				}
				break;

			}
		}

		if (isSucceeded) {
			if (countDown > 0) {
				countDown--;
			} else {
				SceneManager.LoadScene ("Challenge5");
			}

		}

	}
}