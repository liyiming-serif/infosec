using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeFiveManager: MonoBehaviour { 

	/*Constants*/
	public const int numCommands = 6;
	public const int numPlayerBoxes = 2;
	public const int numSpyBoxes = 0;
	public const int numMemoryBoxes = 0;

	/*Gameborad interfaces*/
	public Command[] commands = new Command[numCommands];


	public PlayerController player;
	public PlayerController spy;

	public Inbox playerInbox;
	public Outbox bossOutbox;

	public Inbox spyInbox;
	public Outbox spyOutbox;

	public Button runButton;

	public GameObject[] playerBoxes = new GameObject[numPlayerBoxes];
	public GameObject[] spyBoxes = new GameObject[numSpyBoxes];

	public GameObject playerOutboxDialogue;
	public GameObject spyOutboxDialogue;

	/*game logic states*/
	public bool isExecuting;
	public bool isSucceeded;

	public int playerCmd;
	public int playerOldCounter;

	public int spyCmd;
	public int spyOldCounter;

	public int countDown;

	public Vector2[] pickupMemoryBoxPositions = new Vector2[2];
	public Vector2[] memoryBoxPositions = new Vector2[2];
	public GameObject[] memoryBoxesReference = new GameObject[2];
	public char[] memoryBoxesChar = new char[2];
	public GameObject[] spyOutboxesReference = new GameObject[2];

	/*Messy state flags. TODO clean up*/
	public GameObject playerBoxReference = null;
	public char playerChar;

	public GameObject spyBoxReference = null;
	public char spyChar = (char) 0;
	public bool isStolen = false;
	public bool isStolen2 = false;
	public bool isStolen3 = false;
	public int getFrom = -1;

	public GameObject yellowHash;
	public GameObject yellowDollar;

	public bool[] isLocked = new bool[2];
	public GameObject[] blueGard = new GameObject[2];

	void ActivateCodingPanel(bool setting) {
		for (int i = 0; i < numCommands; i++)
		{
			commands [i].SetEnable (setting);
		}
	}
		
	void StartRunning()
	{
		Reset ();
		RunCommand ();
	}

	void RunSpyCode(int memoryIndex)
	{
		getFrom = memoryIndex;	
		spyOldCounter = spy.counter;
		spy.SetEndPosition (pickupMemoryBoxPositions [memoryIndex]);
	}


	void RunCommand()
	{
		if (playerCmd > 6) {
			this.FailRunning ("I haven't got the public yellow boxes in order.", bossOutbox.feedback);
			return;
		}
		if (playerCmd > 0) {
			commands [playerCmd-1].SetStatus (Command.Status.NotExecuting);
		}
		int optionIndex = commands [playerCmd].optionMenu.value;
		if (optionIndex == 0) {
			this.FailRunning ("Command is not selected.", bossOutbox.feedback);
			return;
		} 
		playerOldCounter = player.counter;
		commands [playerCmd].SetStatus (Command.Status.Executing);
		if (optionIndex == 1) {
			player.SetEndPosition (playerInbox.playerPosition);
		} else if (optionIndex == 2) {
			if (commands [playerCmd].actTo.GetCaptionText () == "Boss") {
				player.SetEndPosition (bossOutbox.playerPosition);
			} else if (commands [playerCmd].actTo.GetCaptionText () == "Nikita") {
				player.SetEndPosition (spyOutbox.playerPosition);
			}
		} else if (optionIndex == 3 || optionIndex == 4) {
			if (commands [playerCmd].actTo.GetCaptionText () == "0") {
				player.SetEndPosition (pickupMemoryBoxPositions [0]);
			} else if (commands [playerCmd].actTo.GetCaptionText () == "1") {
				player.SetEndPosition (pickupMemoryBoxPositions [1]);
			}
		} else if (optionIndex == 5) {
			if (commands [playerCmd].actTo.GetCaptionText () == "0") {
				player.SetEndPosition (pickupMemoryBoxPositions [0]);
			} else if (commands [playerCmd].actTo.GetCaptionText () == "1") {
				player.SetEndPosition (pickupMemoryBoxPositions [1]);
			}
		}
	}

	void FailRunning(string message, GameObject feedback)
	{
		commands [playerCmd].SetStatus (Command.Status.Error);
		Text feedbackText = feedback.GetComponentInChildren<Text> ();
		feedbackText.text = "Error @ Line 0" + (this.playerCmd+1) +": " + message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(235,46,44,255);
		feedback.SetActive (true);

		isExecuting = false;
		ActivateCodingPanel (true);
	}

	void succeedRunning(string message, GameObject feedback)
	{
		Text feedbackText = bossOutbox.feedback.GetComponentInChildren<Text> ();
		feedbackText.text = "Great! " + message;
		Renderer changeColor = feedback.GetComponent<Renderer> ();
		changeColor.material.color = new Color32(90,174,122,255);
		feedback.SetActive (true);

		isExecuting = false;
		isSucceeded = true;
	}

	void Reset()
	{
		commands[playerCmd].SetStatus(Command.Status.NotExecuting);

		isExecuting = true;

		ActivateCodingPanel (false);

		playerCmd = 0;
		spyCmd = 0;
		playerOldCounter = player.counter;
		spyOldCounter = spy.counter;
		countDown = 150;

		player.ResetAnimator ();
		spy.ResetAnimator ();

		//TODO clean up
		playerInbox.DestroyRemainBoxes();
		playerInbox.boxesReference.Add(Instantiate (yellowHash));
		playerInbox.boxesReference [0].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
		playerInbox.boxesReference [0].transform.position = playerInbox.boxesPosition [0];
		playerInbox.boxesChar.Add ('Y');
		playerInbox.boxesReference.Add(Instantiate (yellowDollar));
		playerInbox.boxesReference [1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
		playerInbox.boxesReference [1].transform.position = playerInbox.boxesPosition [1];
		playerInbox.boxesChar.Add ('E');
		playerInbox.remainBoxes = 2;

		spyInbox.ResetBoxes ();

		Destroy(memoryBoxesReference [0]);
		Destroy(memoryBoxesReference [1]);
		memoryBoxesChar [0] = (char) 0;
		memoryBoxesChar [1] = (char) 0;
		Destroy (playerBoxReference);

		bossOutbox.DestroyRemainBoxes ();
		spyOutbox.DestroyRemainBoxes ();

		bossOutbox.feedback.SetActive (false);
		spyOutbox.feedback.SetActive (false);

		Destroy (spyBoxReference);
		spyChar = (char) 0;
		isStolen = false;
		isStolen2 = false;
		isStolen3 = false;
		isLocked [0] = false;
		isLocked [1] = false;
		blueGard [0].SetActive (false);
		blueGard [1].SetActive (false);
	}
		
	void Start () {
		isExecuting = false;
		isSucceeded = false;

		playerCmd = 0;
		spyCmd = 0;
		playerOldCounter = player.counter;
		spyOldCounter = spy.counter;

		Vector2[] inboxPositions = new Vector2[numPlayerBoxes];
		inboxPositions [0] = new Vector2 (-297f, -51f);
		inboxPositions [1] = new Vector2 (-337f, -51f);
		Vector2 pickupBoxLoc = new Vector2 (-293f, 42f);
		playerInbox = new Inbox (numPlayerBoxes, pickupBoxLoc, inboxPositions, playerBoxes);

		//TODO clean up
		playerInbox.boxesReference [0] = Instantiate (yellowHash);
		playerInbox.boxesReference [0].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
		playerInbox.boxesReference [0].transform.position = playerInbox.boxesPosition [0];
		playerInbox.boxesChar.Add ('Y');
		playerInbox.boxesReference [1] = Instantiate (yellowDollar);
		playerInbox.boxesReference [1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
		playerInbox.boxesReference [1].transform.position = playerInbox.boxesPosition [1];
		playerInbox.boxesChar.Add ('E');
		playerInbox.remainBoxes = 2;


		Vector2[] outboxPositions = new Vector2[2];
		outboxPositions [0] = new Vector2 (-81f, -50f);
		outboxPositions [1] = new Vector2 (-39f, -50f);
		Vector2 dropBoxLoc = new Vector2 (-147f, -39f);
		bossOutbox = new Outbox (2, dropBoxLoc, outboxPositions, playerOutboxDialogue);

		Vector2[] inboxSPositions = new Vector2[numSpyBoxes];
		Vector2 pickupSBoxLoc = new Vector2 (-293f, -140f);
		spyInbox = new Inbox (numSpyBoxes, pickupSBoxLoc, inboxSPositions, spyBoxes);

		Vector2[] outboxSPositions = new Vector2[2];
		outboxSPositions [0] = new Vector2 (-92f, -227f);
		outboxSPositions [1] = new Vector2 (-50f, -227f);
		Vector2 dropBoxSLoc = new Vector2 (-151f, -211f);
		spyOutbox = new Outbox (1, dropBoxSLoc, outboxSPositions, spyOutboxDialogue);

		pickupMemoryBoxPositions [0] = new Vector2 (-225f, -39f);
		pickupMemoryBoxPositions [1] = new Vector2 (-166f, -39f);

		memoryBoxPositions [0] = new Vector2 (-225f,-115f);
		memoryBoxPositions [1] = new Vector2 (-166f,-115f);

		runButton.onClick.AddListener (new UnityAction (this.StartRunning));

		bossOutbox.feedback.SetActive (false);
		spyOutbox.feedback.SetActive (false);

		memoryBoxesChar [0] = (char) 0;
		memoryBoxesChar [1] = (char) 0;

		isLocked [0] = false;
		isLocked [1] = false;
		blueGard [0].SetActive (false);
		blueGard [1].SetActive (false);

		isStolen3 = false;

	}

	void Update () {

		if (spyOldCounter != spy.counter) { //Spy movement is done.
			spyOldCounter = spy.counter;
			if (!isStolen2 && spyBoxReference == null) {
				//time to copy
				if (memoryBoxesChar [getFrom] == 'Y') {
					spyBoxReference = Instantiate (yellowHash);
				} else if (memoryBoxesChar [getFrom] == 'E') {
					spyBoxReference = Instantiate (yellowDollar);
				}
				spyChar = memoryBoxesChar [getFrom];
				spyBoxReference.transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
				spyBoxReference.transform.SetParent (spy.transform);
				spyBoxReference.transform.localPosition = new Vector2 (0f, -16f);
				spy.SetEndPosition (spyOutbox.playerPosition);
			} else if (spyBoxReference != null) {
				Destroy (spyBoxReference);
				bossOutbox.boxesChar.Add (spyChar);
				for (int i = 0; i < spyOutbox.boxesReference.Count; i++) {
					spyOutbox.boxesReference [i].transform.position = spyOutbox.boxesPosition [spyOutbox.boxesReference.Count - i]; // Maybe out of range
				}
				if (spyChar == 'Y') {
					spyOutbox.boxesReference.Add (Instantiate (yellowHash));

				} else if (spyChar == 'E') {
					spyOutbox.boxesReference.Add (Instantiate (yellowDollar));
				}
				spyOutbox.boxesReference [spyOutbox.boxesReference.Count - 1].transform.position = spyOutbox.boxesPosition [0];
				spyOutbox.boxesReference [spyOutbox.boxesReference.Count - 1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
				spy.SetEndPosition (spy.initPosition);
				this.FailRunning ("Aha! I get a private box " + spyChar.ToString (), spyOutbox.feedback);
				isStolen2 = true;
			} else if (isStolen2) {
				isStolen3 = true;
			}
		}

		if (playerOldCounter != player.counter) { //Player movement is done
			playerOldCounter = player.counter;

			switch (commands [playerCmd].optionMenu.value)
			{
			case 1:
				if (playerInbox.boxesReference.Count > 0) {
					char charValue = playerInbox.boxesChar [0];
					playerInbox.boxesChar.RemoveAt (0);
					Destroy (playerInbox.boxesReference [0]);
					playerInbox.boxesReference.RemoveAt (0);
					for (int i = 0; i < playerInbox.boxesReference.Count; i++) {
						playerInbox.boxesReference [i].transform.position = playerInbox.boxesPosition [i];
					}
					Destroy (playerBoxReference);
					playerChar = charValue;
					if (charValue == 'Y') {
						playerBoxReference = Instantiate (yellowHash);
					} else if (charValue == 'E') {
						playerBoxReference = Instantiate (yellowDollar);
					}
					playerBoxReference.transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
					playerBoxReference.transform.SetParent (player.transform);
					playerBoxReference.transform.localPosition = new Vector2(0f, -16f);
					playerCmd++;
					RunCommand();
				} else {
					this.FailRunning ("There is no box in the inbox belt", bossOutbox.feedback);
				}
				break;
			case 2:
				if (playerBoxReference == null) {
					if (commands [playerCmd].actTo.GetCaptionText () == "Boss") {
						this.FailRunning ("No box in your hands.", bossOutbox.feedback);
					} else if (commands [playerCmd].actTo.GetCaptionText () == "Nikita") {
						this.FailRunning ("No box in your hands.", spyOutbox.feedback);
					}
				} else {
					Destroy (playerBoxReference);
					if (commands [playerCmd].actTo.GetCaptionText () == "Boss") {
						// Accept the box first
						bossOutbox.boxesChar.Add(playerChar);
						for (int i = 0; i < bossOutbox.boxesReference.Count; i++) {
							bossOutbox.boxesReference [i].transform.position = bossOutbox.boxesPosition [bossOutbox.boxesReference.Count-i]; // Maybe out of range
						}

						if (playerChar == 'Y') {
							bossOutbox.boxesReference.Add(Instantiate (yellowHash));
							bossOutbox.boxesReference [bossOutbox.boxesReference.Count - 1].transform.position = bossOutbox.boxesPosition [0];
							bossOutbox.boxesReference [bossOutbox.boxesReference.Count - 1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);

							if (bossOutbox.boxesChar [0] == 'Y') {
								this.FailRunning ("The first box should be 'E'.", bossOutbox.feedback);
							} else if (bossOutbox.boxesChar [0] == 'E') {
								if (!isStolen) {
									this.succeedRunning ("Thanks for your hard working. This is the end challenge.", bossOutbox.feedback);
								}
							}
						} else if (playerChar == 'E') {
							bossOutbox.boxesReference.Add(Instantiate (yellowDollar));
							bossOutbox.boxesReference [bossOutbox.boxesReference.Count - 1].transform.position = bossOutbox.boxesPosition [0];
							bossOutbox.boxesReference [bossOutbox.boxesReference.Count - 1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);

							if (bossOutbox.boxesChar.Count == 1) {
								playerCmd++;
								RunCommand ();
							} else {
								this.FailRunning ("The second box should be 'Y'.", bossOutbox.feedback);
							}
						}
					} else if (commands [playerCmd].actTo.GetCaptionText () == "Nikita") {
						//Accept the box first
						spyOutbox.boxesChar.Add(playerChar);
						for (int i = 0; i < spyOutbox.boxesReference.Count; i++) {
							spyOutbox.boxesReference [i].transform.position = spyOutbox.boxesPosition [bossOutbox.boxesReference.Count-i];
						}
						if (playerChar == 'Y') {
							spyOutbox.boxesReference.Add (Instantiate (yellowHash));
							spyOutbox.boxesReference [spyOutbox.boxesReference.Count - 1].transform.position = spyOutbox.boxesPosition [0];
							spyOutbox.boxesReference [spyOutbox.boxesReference.Count - 1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
						} else if (playerChar == 'E') {
							spyOutbox.boxesReference.Add (Instantiate (yellowDollar));
							spyOutbox.boxesReference [spyOutbox.boxesReference.Count - 1].transform.position = spyOutbox.boxesPosition [0];
							spyOutbox.boxesReference [spyOutbox.boxesReference.Count - 1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
						}
						this.FailRunning ("Thanks, but you don't work for me now.", spyOutbox.feedback);
					}
				}
				break;
			case 3:
				if (commands [playerCmd].actTo.GetCaptionText () == "0") {
					if (memoryBoxesReference [0] == null) {
						this.FailRunning ("No box in this cell.", bossOutbox.feedback);
					} else {
						//copy a box from here
						Destroy(playerBoxReference);
						playerChar = memoryBoxesChar [0];
						if (playerChar == 'Y') {
							playerBoxReference = Instantiate (yellowHash);
						} else if (playerChar == 'E') {
							playerBoxReference = Instantiate (yellowDollar);
						}
						playerBoxReference.transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
						playerBoxReference.transform.SetParent (player.transform);
						playerBoxReference.transform.localPosition = new Vector2 (0f, -16f);
						playerCmd++;
						RunCommand ();
					}
				} else if (commands [playerCmd].actTo.GetCaptionText () == "1") {
					if (memoryBoxesReference [1] == null) {
						this.FailRunning ("No box in this cell.", bossOutbox.feedback);
					} else {
						//copy a box from here
						Destroy(playerBoxReference);
						playerChar = memoryBoxesChar [1];
						if (playerChar == 'Y') {
							playerBoxReference = Instantiate (yellowHash);
						} else if (playerChar == 'E') {
							playerBoxReference = Instantiate (yellowDollar);
						}
						playerBoxReference.transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
						playerBoxReference.transform.SetParent (player.transform);
						playerBoxReference.transform.localPosition = new Vector2 (0f, -16f);
						playerCmd++;
						RunCommand ();
					}
				}
				break;
			case 4:
				if (playerBoxReference == null) {
					this.FailRunning ("No box in your hands.", bossOutbox.feedback);
				} else {
					if (commands [playerCmd].actTo.GetCaptionText () == "0") {
						Destroy (memoryBoxesReference[0]);
						if (playerChar == 'E') {
							memoryBoxesReference [0] = Instantiate (yellowDollar);
							memoryBoxesChar[0] = 'E';
						} else if (playerChar == 'Y') {
							memoryBoxesReference [0] = Instantiate (yellowHash);
							memoryBoxesChar [0] = 'Y';
						}
						memoryBoxesReference [0].transform.position = memoryBoxPositions [0];
						memoryBoxesReference[0].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
						if (!isStolen && !isLocked[0]) {
							RunSpyCode (0);
							isStolen = true;
						}

					} else if (commands [playerCmd].actTo.GetCaptionText () == "1") {
						Destroy (memoryBoxesReference[1]);
						if (playerChar == 'E') {
							memoryBoxesReference [1] = Instantiate (yellowDollar);
							memoryBoxesChar[1] = 'E';
						} else if (playerChar == 'Y') {
							memoryBoxesReference [1] = Instantiate (yellowHash);
							memoryBoxesChar [1] = 'Y';
						}
						memoryBoxesReference [1].transform.position = memoryBoxPositions [1];
						memoryBoxesReference[1].transform.localScale = new Vector3 (0.3485f, 0.3485f, 1f);
						if (!isStolen && !isLocked[1]) {
							RunSpyCode (1);
							isStolen = true;
						}

					}

					playerCmd++;
					RunCommand ();
				}
				break;
			case 5:
				if(commands [playerCmd].actTo.GetCaptionText () == "0"){
					if (!isLocked[0]) {
						isLocked[0] = true;
						blueGard [0].SetActive (true);
					}
				}else if(commands [playerCmd].actTo.GetCaptionText () == "1"){
					if (!isLocked[0]) {
						isLocked[0] = true;
						blueGard [0].SetActive (true);
					}
				}
				playerCmd++;
				RunCommand ();
				break;
			}

		}
	}
}