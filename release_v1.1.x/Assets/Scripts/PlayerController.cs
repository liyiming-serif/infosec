using UnityEngine;

public class PlayerController : MonoBehaviour
{
	

	[SerializeField] Animator animator;
	[SerializeField] Transform slotTrans;

	public float speed = 100.0f;
	public int counter = 0;
	public Vector2 initPosition;
	public Vector2 endPosition;

	void Start()
	{
		animator.enabled = false;
		initPosition = animator.transform.position;
		endPosition = initPosition;
	}

	public void ResetAnimator()
	{
		animator.enabled = false;
		animator.transform.position = initPosition;
		endPosition = initPosition;
		slotTrans.GetComponent<DataSlot> ().RemoveData ();
	}

	public void SetInitPosition(Vector2 newPosition)
	{
		initPosition = newPosition;
	}

	public void SetEndPosition(Vector2 destination)
	{
		animator.enabled = true;
		if (Vector2.Distance (endPosition, destination) > .1f) {
			endPosition = destination;
		}
	}

	public void PickupData(Data data){
		DropdownData ();
		if (data != null) {
			data.transform.SetParent (slotTrans);
			data.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void DropdownData(){
		slotTrans.GetComponent<DataSlot> ().RemoveData ();
	}

	public Data SendData(){
		Data dataToSend = CloneData ();
		DropdownData ();
		return dataToSend;
	}

	public Data CloneData(){
		Data dataToReturn = null;
		Data holdingData = slotTrans.GetComponent<DataSlot> ().data;
		if (holdingData) {
			dataToReturn = Instantiate (holdingData);
		}
		return dataToReturn;
	}

	public string GetData(){
		string dataToReturn = null;
		if (IsCarryingData ()) {
			dataToReturn = slotTrans.GetComponent<DataSlot> ().data.dataStr;
		}
		return dataToReturn;
	}

	public bool IsCarryingData(){
		return (slotTrans.GetComponent<DataSlot> ().data != null);
	}

	private void SetDirection()
	{
		float horizontal = animator.transform.position.x - endPosition.x;
		float vertical = animator.transform.position.y - endPosition.y;
		if (vertical < 0) {
			animator.SetInteger ("Direction", 2); // towards north
		} else if (vertical > 0) {
			animator.SetInteger ("Direction", 0); // towards south
		} else if (horizontal > 0) {
			animator.SetInteger ("Direction", 1); // towards west
		} else if (horizontal < 0){
			animator.SetInteger ("Direction", 3); // towards east
		}
	}
		
	void Update()
	{
		float step = speed * Time.deltaTime;

		if (Vector2.Distance (animator.transform.position, endPosition) > .1f) {
			animator.transform.position = Vector2.MoveTowards (animator.transform.position, endPosition, step);
		} else if (animator.enabled) {
			counter++;
			animator.enabled = false;
		}

	}
}