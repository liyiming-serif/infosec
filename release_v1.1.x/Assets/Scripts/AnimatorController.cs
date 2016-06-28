using UnityEngine;

public class AnimatorController : MonoBehaviour
{
	
	Animator animator;
	DataSlot dslot;

	float speed = 200.0f;
	public int counter = 0;
	public Vector2 initPosition;
	public Vector2 endPosition;
    public Vector2 startPosition;

	void Start()
	{
		animator = GetComponent<Animator> ();
		dslot = GetComponentInChildren<DataSlot> ();
		animator.enabled = false;
		initPosition = animator.transform.position;
		endPosition = initPosition;
        startPosition = endPosition;
	}

	public void ResetAnimator()
	{
		animator.enabled = false;
		animator.transform.position = initPosition;
		endPosition = initPosition;
		DropdownData ();
	}

	public void SetInitPosition(Vector2 newPosition)
	{
		initPosition = newPosition;
	}

	public void SetEndPosition(Vector2 destination)
	{
		animator.enabled = true;
		if (Vector2.Distance (endPosition, destination) > .1f) {
            startPosition = endPosition;
            endPosition = destination;
       }
	}

	public void PickupData(Data data){
		DropdownData ();
		if (data != null) {
			data.transform.SetParent (dslot.transform);
			data.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void DropdownData(){
		dslot.RemoveData ();
	}

	public Data SendData(){
		Data dataToSend = CloneData ();
		DropdownData ();
		return dataToSend;
	}

	public Data CloneData(){
		Data dataToReturn = null;
		Data holdingData = dslot.data;
		if (holdingData) {
			dataToReturn = Instantiate (holdingData);
		}
		return dataToReturn;
	}

	public string GetData(){
		string dataToReturn = null;
		if (IsCarryingData ()) {
			dataToReturn = dslot.data.dataStr;
		}
		return dataToReturn;
	}

	public bool IsCarryingData(){
		return (dslot.data != null);
	}

    public void RebasePosition(Vector2 oldPosition)
    {
        animator.transform.position = oldPosition;
        endPosition = oldPosition;
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