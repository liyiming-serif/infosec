using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class AlienC : MonoBehaviour
{

    protected Animator animator;

    [SerializeField]
    float speed = 200.0f;
    protected Vector2 initPosition;
    public Vector2 endPosition;
    public Vector2 startPosition;
	bool isMoving;

    void Awake()
    {
        animator = GetComponent<Animator>();
		animator.SetTrigger("stopwalk");
        initPosition = animator.transform.position;
        endPosition = initPosition;
        startPosition = endPosition;
		isMoving = false;
    }

    public void ResetAnimator()
    {
		animator.SetTrigger("stopwalk");
        animator.transform.position = initPosition;
        endPosition = initPosition;
    }

    public void SetInitPosition(Vector2 newPosition)
    {
        initPosition = newPosition;
    }

    public void SetEndPosition(Vector2 destination)
    {
		animator.SetTrigger("startwalk");
        if (Vector2.Distance(endPosition, destination) > .1f)
        {
            startPosition = endPosition;
            endPosition = destination;
			isMoving = true;
        }
    }

    public void RebasePosition(Vector2 oldPosition)
    {
        animator.transform.position = oldPosition;
        endPosition = oldPosition;
    }

	IEnumerator Done()
	{
		yield return new WaitForSeconds(2f);
		gameObject.SetActive(false);
	}

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (Vector2.Distance(animator.transform.position, endPosition) > .1f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, endPosition, step);
        }
		else if (isMoving)
        {
            //TODO Tell TaskManager that it's there.
            //GetComponentInParent<NetworkW>().SendMessage("Result");
			animator.SetTrigger("triggervirus");
			StartCoroutine(Done());
			isMoving = false;

        }

    }
}