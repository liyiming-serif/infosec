using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public Vector2 relativePos;
	public Vector3 scaleInfo;

	public Animator animator;

	public Vector2 initPosition;
	public Vector2 endPosition;
	public int counter;

	public Box carryingRef;

	PlayerController(){
		this.counter = 0;
		this.speed = 100f;
		this.relativePos = new Vector2 (0f, -16f);
		this.carryingRef = new Box ();
	}

	public void ResetAnimator()
	{
		animator.enabled = false;
		animator.transform.position = initPosition;
		endPosition = initPosition;

		if (!carryingRef.IsDestroyed()) {
			carryingRef.Destroy ();
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

	public void PickupBox(Box boxObject){
		DropdownBox ();
		if (!boxObject.IsDestroyed ()) {
			carryingRef = boxObject;
			carryingRef.boxRef.transform.SetParent (this.transform);
			carryingRef.boxRef.transform.localPosition = relativePos;
		}
	}

	public void DropdownBox(){
		if (!carryingRef.IsDestroyed()) {
			carryingRef.Destroy ();
		}
	}

	public Box SendBox(){
		Box sendBox = CloneBox ();
		DropdownBox ();
		return sendBox;
	}

	public Box CloneBox(){
		Box cloneBox;
		if (!carryingRef.IsDestroyed ()) {
			cloneBox = new Box (carryingRef);
		} else {
			cloneBox = new Box ();
		}
		return cloneBox;
	}

	public bool IsCarryingBox(){
		return (!carryingRef.IsDestroyed ());
	}

	void SetDirection()
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


	void Awake()
	{
		animator = gameObject.GetComponent<Animator>();
		animator.enabled = false;
		initPosition = animator.transform.position;
		endPosition = initPosition;
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