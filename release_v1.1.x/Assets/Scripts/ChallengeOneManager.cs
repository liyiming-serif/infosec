using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeOneManager: MonoBehaviour { 

	public enum SpyingState {Available, Unavailable, Stealing, Stolen};

	/*Constants*/
	public const int numPlayerBoxes = 2;
	public const int numSpyBoxes = 0;
	public const int numMemoryBoxes = 0;

	public const int numBoxPositions = 3;
	public const int maxNumCommands = 10;

	public const string bossName = "Boss";
	public const string spyName = "Nikita";
	public const string LocZero = "0";
	public const string LocOne = "1";
	public const char dataDollar = '$';
	public const char dataBlueE = 'E';

	/*Gameborad interfaces*/
	public GameObject yellowDollar;
	public GameObject blueE;

	public GameObject commandContent;
	public Command commandTemplate;

	public PlayerController player;
	public GameObject bossFeedback;

	public Button addButton;
	public Button discardButton;
	public Button runButton;

	/*game logic states*/
	private List<Command> commands;

	private Box[] playerInboxInit;

	private Inbox playerInbox;

	private Outbox bossOutbox;

	private MemoryBar ground;

	private bool isExecuting;
	private bool isSucceeded;

	private int playerCmd;
	private int playerOldCounter;

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
			this.FailRunning ("There should be one blue 'E' box and one yellow '$' box!", bossFeedback);
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
		feedbackText.text = "Hi, box manager! You work for me to process boxes from the inbox according to my task.";
		Renderer changeColor = bossFeedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(83,144,195,212);
		bossFeedback.SetActive (true);
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

		commands [playerCmd].SetStatus (Command.Status.NotExecuting);
		playerCmd = 0;
		playerOldCounter = player.counter;

		player.ResetAnimator ();

		playerInbox.ResetInbox(playerInboxInit);

		bossOutbox.EmptyBoxes ();

		bossFeedback.SetActive (false);

		ground.ResetBar ();
	}
		
	void Start () {

		commands = new List<Command> ();

		commands.Add (Instantiate(commandTemplate));
		commands [0].UpdateAdvancedMenus ();
		commands [0].cmdNo.text = "1";
		commands [0].transform.position = new Vector2 (313f, 55f);
		commands [0].transform.SetParent (commandContent.transform);
	
		isExecuting = false;
		isSucceeded = false;

		playerCmd = 0;
		playerOldCounter = player.counter;
	
		countDown = 150;

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
		playerInbox = new Inbox (dropdownBoxLoc, boxPositions, playerInboxInit);

		// boss's locations info
		dropdownBoxLoc = new Vector2 (-147f, -39f);
		boxPositions [0] = new Vector2 (-81f, -50f);
		boxPositions [1] = new Vector2 (-39f, -50f);
		boxPositions [2] = new Vector2 (0f, -50f);
		bossOutbox = new Outbox (dropdownBoxLoc, boxPositions);

		// memory's location info
		Vector2[] memoryPickupPoses = new Vector2[2]; //2 slots only in the memory
		memoryPickupPoses [0] = new Vector2 (-225f, -39f);
		memoryPickupPoses [1] = new Vector2 (-166f, -39f);
		Vector2[] memoryBoxPoses = new Vector2[2];
		memoryBoxPoses [0] = new Vector2 (-225f,-115f);
		memoryBoxPoses [1] = new Vector2 (-166f,-115f);
		ground = new MemoryBar (memoryPickupPoses, memoryBoxPoses);

		runButton.onClick.AddListener (new UnityAction (StartRunning));
		addButton.onClick.AddListener (new UnityAction (spawnNewCommand));
		discardButton.onClick.AddListener (new UnityAction (discardLastCommand));

		Introduction ();
	}

	void Update () {

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
					this.FailRunning ("No box in your hands.", bossFeedback);

				} else {
					if (commands [playerCmd].actTo.GetCaptionText () == bossName) {
						bossOutbox.acceptBox (player.SendBox ());
						if (bossOutbox.GetCount () == 2) {
							this.succeedRunning ("Thanks for your hard working!", bossFeedback);
						} else {
							playerCmd++;
							RunCommand ();
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
				SceneManager.LoadScene ("Challenge2");
			}

		}

	}
}