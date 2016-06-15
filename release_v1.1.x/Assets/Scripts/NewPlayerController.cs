using UnityEngine;

public class NewPlayerController : MonoBehaviour
{
	

	[SerializeField] Animator animator;
	public float speed;
	public int counter;
	public Vector2 initPosition;
	public Vector2 endPosition;

	[SerializeField] Transform slotTrans;

	void Awake()
	{
		this.speed = 100f;
		this.counter = 0;
		animator.enabled = false;
		initPosition = animator.transform.position;
		endPosition = initPosition;
	}

	public void ResetAnimator()
	{
		animator.enabled = false;
		animator.transform.position = initPosition;
		endPosition = initPosition;
		NewBox holdingBox = slotTrans.GetComponent<Slot> ().item;
		if (holdingBox) {
			holdingBox.OnBecameInvisible ();
		}
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

	public void PickupBox(NewBox box){
		DropdownBox ();
		if (box != null) {
			box.transform.SetParent (slotTrans);
			box.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}

	public void DropdownBox(){
		slotTrans.GetComponent<Slot> ().removeBox ();
	}

	public NewBox SendBox(){
		NewBox boxToSend = CloneBox ();
		DropdownBox ();
		return boxToSend;
	}

	public NewBox CloneBox(){
		NewBox boxToReturn = null;
		NewBox holdingBox = slotTrans.GetComponent<Slot> ().item;
		if (holdingBox) {
			boxToReturn = Instantiate (holdingBox);
		}
		return boxToReturn;
	}

	public string BoxData(){
		string dataToReturn = null;
		if (IsCarryingBox ()) {
			dataToReturn = slotTrans.GetComponent<Slot> ().item.data;
		}
		return dataToReturn;
	}

	public bool IsCarryingBox(){
		return (slotTrans.GetComponent<Slot> ().item != null);
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