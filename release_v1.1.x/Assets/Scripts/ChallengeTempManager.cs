using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ChallengeTempManager: MonoBehaviour { 

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

	public Button addButton;
	public Button discardButton;

	/*game logic states*/
	private List<Command> commands;

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
		
	void Start () {

		commands = new List<Command> ();

		commands.Add (Instantiate(commandTemplate));
		commands [0].UpdateAdvancedMenus ();
		commands [0].cmdNo.text = "1";
		commands [0].transform.position = new Vector2 (313f, 55f);
		commands [0].transform.SetParent (commandContent.transform);

		addButton.onClick.AddListener (new UnityAction (spawnNewCommand));
		discardButton.onClick.AddListener (new UnityAction (discardLastCommand));

	}

	void Update () {

	}
}