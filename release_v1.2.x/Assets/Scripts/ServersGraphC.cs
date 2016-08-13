using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ServersGraphC : MonoBehaviour {

    [SerializeField]
    List<Animator> inpaths;
    [SerializeField]
    List<Animator> servers;
    [SerializeField]
    List<Image> dnames;

    public void LightupDomainName(int id)
    {
        //The dname id starts from 1, 0 is reserved for the launch pad.
        dnames[id].color = Color.green;
    }

    public void ActivatePath(int id, bool isActive)
    {
        inpaths[id].enabled = isActive;
    }

    public Vector2 GetLandingPos(int id)
    {
        Vector2 result = servers[id].transform.position;
        result.x += 30;
        result.y -= 30;
        return result;
    }

    void Start()
    {
        foreach(Animator a in inpaths)
        {
            a.enabled = false;
        }
    }
}
