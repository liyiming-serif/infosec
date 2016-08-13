using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AlienC : MonoBehaviour
{

    
    [SerializeField]
    float speed = 150.0f;

    Animator animator;
    Vector2 initPosition;
    Vector2 endPosition;
    bool startMoving;

    void Awake()
    {
        animator = GetComponent<Animator>();
		animator.SetTrigger("stopwalk");
        initPosition = animator.transform.position;
        endPosition = initPosition;
        startMoving = false;
    }

    public void SetInitPosition(Vector2 newPosition)
    {
        initPosition = newPosition;
    }

    public void SetEndPosition(Vector2 destination)
    {
        if (Vector2.Distance(endPosition, destination) > .1f)
        {
            startMoving = true;
            animator.SetTrigger("startwalk");
            endPosition = destination;
        }
    }

    public void RebasePosition(Vector2 oldPosition)
    {
        animator.transform.position = oldPosition;
        endPosition = oldPosition;
    }

    public void ResetAnimator()
    {
        SetEndPosition(initPosition);
    }

    IEnumerator DeactiveItself()
	{
		yield return new WaitForSeconds(1.5f);
		gameObject.SetActive(false);
	}

    public void GetExploded()
    {
        animator.SetTrigger("triggervirus");
        StartCoroutine(DeactiveItself());
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (Vector2.Distance(animator.transform.position, endPosition) > .1f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, endPosition, step);
        }
        else if(startMoving)
        {
            startMoving = false;
            ExecuteEvents.ExecuteHierarchy<NetworkWindows>(this.gameObject, null, (x, y) => x.AlienGo());
        }

    }
}