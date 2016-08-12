using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ServersGraphC : MonoBehaviour {

    [SerializeField]
    List<Image> dnames;
    [SerializeField]
    List<Animator> servers;
    [SerializeField]
    List<Animator> outpaths;

    int nowServer;
    int nextServer;

    //IEnumerator StartWalking()
    //{
    //    yield return new WaitForSeconds(1);
    //    GetComponentInParent<NetworkW>().alienC.SetEndPosition(new Vector2(-283, 62));
    //}

    //IEnumerator Explode()
    //{
    //    yield return new WaitForSeconds(1);
    //    GetComponentInParent<NetworkW>().Explode();
    //}

    //public void ToNextServer(int id)
    //{
    //    animationState = 1;
    //    outpaths[currentID].GetComponent<SpriteRenderer>().enabled = true;
    //    SetActive(outpaths[currentID], true);
    //    nextID = id;
    //    StartCoroutine(StartWalking());
    //}

    //public void ArriveNextServer()
    //{
    //    animationState = 0;
    //    SetActive(outpaths[currentID], false);
    //    SetActive(servers[nextID], true);
    //    currentID = nextID;
    //    StartCoroutine(Explode());
    //}

    // Use this for initialization
    void Start () {
	}

}
