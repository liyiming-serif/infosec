using UnityEngine;
using System.Collections;


public class NewBox : MonoBehaviour
{
	public string data;

	public void OnBecameInvisible()
	{
		Destroy(gameObject);
	}

}