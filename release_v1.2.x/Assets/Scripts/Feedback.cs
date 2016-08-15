using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Feedback : MonoBehaviour
{

    public static Feedback instance;

    [SerializeField]
    Button response;
    [SerializeField]
    Text feedback;

    public void popUp(bool isSucceded, string newScene)
    {
        if (isSucceded)
        {
            response.GetComponentInChildren<Text>().text = "Next";
            feedback.text = "More Customers";
        }
        else
        {
            response.GetComponentInChildren<Text>().text = "Try Again";
            feedback.text = "Game Over";
        }
        gameObject.SetActive(true);
        response.onClick.RemoveAllListeners();
        response.onClick.AddListener(delegate { SceneManager.LoadScene(newScene); });
    }

    void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

}
