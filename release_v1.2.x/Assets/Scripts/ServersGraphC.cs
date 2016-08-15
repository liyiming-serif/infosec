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
    [SerializeField]
    List<string> keys;

    Dictionary<string, int> mapID;

    public void LightupDomainName(string key, Color c)
    {
        dnames[mapID[key]].color = c;
    }

    public void ActivatePath(string key, bool isActive)
    {
        inpaths[mapID[key]].enabled = isActive;
    }

    public Vector2 GetLandingPos(string key)
    {
        Vector2 result = servers[mapID[key]].transform.position;
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
        mapID = new Dictionary<string, int>();
        for(int i = 0; i < keys.Count; i++)
        {
            mapID.Add(keys[i], i);
        }
    }
}
