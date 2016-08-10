using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServerAnimatorsController : MonoBehaviour {

    [SerializeField]
    List<Image> dnames;
    [SerializeField]
    List<Animator> servers;
    [SerializeField]
    List<Animator> outpaths;
    [SerializeField]
    List<Animator> inpaths;

    int counter;
    List<bool> isBlinking;
    int currentID;
    int nextID;
    int animationState;

    void SetActive(Animator a, bool b)
    {
        a.enabled = b;
    }

    void SetBlink(int index, bool b)
    {
        isBlinking[index] = b;
    }


    public void ToNextServer(int id)
    {
        animationState = 1;
        SetBlink(currentID, false);
        outpaths[currentID].GetComponent<SpriteRenderer>().enabled = true;
        SetActive(outpaths[currentID], true);
        SetActive(inpaths[id-1], true);
        nextID = id;
    }

    public void ArriveNextServer()
    {
        animationState = 0;
        SetActive(outpaths[currentID], false);
        SetActive(inpaths[nextID - 1], false);
        SetActive(servers[nextID], true);
        currentID = nextID;
    }

    // Use this for initialization
    void Start () {
	    foreach (Animator a in servers)
        {
            SetActive(a, false);
        }
        foreach (Animator a in inpaths)
        {
            SetActive(a, false);
        }
        foreach(Animator a in outpaths)
        {
            SetActive(a, false);
        }
        SetActive(servers[0], true);
        isBlinking = new List<bool>();
        isBlinking.Add(true);
        for(int i = 1; i < outpaths.Count; i++)
        {
            isBlinking.Add(false);
        }
        counter = 0;
        currentID = 0;
        animationState = 0;
	}
	
	// Update is called once per frame
	void Update () {
	    if(counter % 50 == 0)
        {
            for(int i = 0; i < isBlinking.Count; i++)
            {
                if(isBlinking[i])
                {
                    SpriteRenderer o = outpaths[i].GetComponent<SpriteRenderer>();
                    o.enabled = !o.enabled;
                }
            }
            counter = 1;
        }
        else
        {
            counter++;
        }
	}


}
