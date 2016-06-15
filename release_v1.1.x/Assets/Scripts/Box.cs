using UnityEngine;
using System.Collections;


public class Box 
{
	public GameObject boxRef;
	public char data;
	public Vector3 scaleInfo;

	public Box(){
		this.boxRef = null;
		this.data = (char) 0;
		this.scaleInfo = new Vector3 (0.3485f, 0.3485f, 1f);
	}

	public Box (Box boxObject){
		this.scaleInfo = new Vector3 (0.3485f, 0.3485f, 1f);
		if (boxObject.IsDestroyed()) {
			this.boxRef = null;
			this.data = (char) 0;
		} else {
			this.boxRef = MonoBehaviour.Instantiate (boxObject.boxRef);
			this.boxRef.transform.parent = null;
			this.boxRef.transform.localScale = scaleInfo;
			this.data = boxObject.data;
		}
	}

	public Box(GameObject boxPrefab, char data){
		this.scaleInfo = new Vector3 (0.3485f, 0.3485f, 1f);
		this.boxRef = MonoBehaviour.Instantiate (boxPrefab);
		this.boxRef.transform.parent = null;
		this.boxRef.transform.localScale = scaleInfo;
		this.data = data;
	}

	public Box(GameObject boxPrefab, char data, Vector2 relativePos, GameObject parent = null){
		this.scaleInfo = new Vector3 (0.3485f, 0.3485f, 1f);
		this.boxRef = MonoBehaviour.Instantiate (boxPrefab);
		this.boxRef.transform.parent = null;
		this.boxRef.transform.localScale = this.scaleInfo;
		this.data = data;
		if (parent == null) {
			this.boxRef.transform.position = relativePos;
		} else {
			this.boxRef.transform.SetParent (parent.transform);
			this.boxRef.transform.localPosition = relativePos;
		}
	}
		
	public void Destroy(){
		MonoBehaviour.Destroy (boxRef);
		data = (char) 0;
	}

	public bool IsDestroyed(){
		return (boxRef == null);
	}
}