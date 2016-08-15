using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AlienC : MonoBehaviour
{

    public delegate void CallbackFunct();

    [SerializeField]
    float speed = 150.0f;

    Animator animator;
    Vector2 initPosition;
    Vector2 endPosition;
    bool startMoving;
    bool _isForward;

    public bool isForward
    {
        get
        {
            return _isForward;
        }
        set
        {
            _isForward = value;
        }
    }
    void Awake()
    {
        animator = GetComponent<Animator>();
        initPosition = animator.transform.position;
        endPosition = initPosition;
        startMoving = false;
        _isForward = true;
    }

    public void SetInitPosition(Vector2 newPosition)
    {
        initPosition = newPosition;
    }

    public void ReturnToInitPosition()
    {
        SetEndPosition(initPosition);
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

    IEnumerator AfterAnimation(float sec, CallbackFunct func)
	{
		yield return new WaitForSeconds(sec);
        func();
	}

    public void GetExploded(CallbackFunct func)
    {
        animator.SetTrigger("triggervirus");
        func = delegate { gameObject.SetActive(false); } + func;
        StartCoroutine(AfterAnimation(2f, func));
    }


    public void GetRevenge(CallbackFunct func)
    {
        animator.SetTrigger("alienwins");
        func = func;
        StartCoroutine(AfterAnimation(1f, func));
    }
    public void GetConfused()
    {
        animator.SetTrigger("throwerror");
        _isForward = false; //Return to the launchpad
        StartCoroutine(AfterAnimation(1f, delegate { ExecuteEvents.ExecuteHierarchy<NetworkWindows>(this.gameObject, null, (x, y) => x.AlienGo(_isForward)); }));
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
            animator.SetTrigger("stopwalk");
            ExecuteEvents.ExecuteHierarchy<NetworkWindows>(this.gameObject, null, (x, y) => x.AlienGo(_isForward));
        }

    }
}