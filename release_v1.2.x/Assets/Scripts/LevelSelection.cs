using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour {

    [SerializeField]
    Button canvasSwitch;

    void Start() {
        List<Button> toLevel = new List<Button>();
        toLevel.AddRange(GetComponentsInChildren<Button>(true));
        toLevel[0].onClick.AddListener(delegate { SceneManager.LoadScene("Challenge1"); });
        toLevel[1].onClick.AddListener(delegate { SceneManager.LoadScene("Challenge2"); });
        toLevel[2].onClick.AddListener(delegate { SceneManager.LoadScene("Challenge3"); });
        toLevel[3].onClick.AddListener(delegate { SceneManager.LoadScene("Challenge5"); });
        canvasSwitch.onClick.AddListener(delegate { gameObject.SetActive(!gameObject.activeSelf); });
        gameObject.SetActive(false);
    }

}
