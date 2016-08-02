using UnityEngine;

public class AnimatorController : MonoBehaviour
{

    protected Animator animator;

    [SerializeField]
    float speed = 200.0f;
    protected Vector2 initPosition;
    public Vector2 endPosition;
    public Vector2 startPosition;

    void Awake()
    {
        animator = GetComponent<Animator>();
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
    }

    public void SetInitPosition(Vector2 newPosition)
    {
        initPosition = newPosition;
    }

    public void SetEndPosition(Vector2 destination)
    {
        animator.enabled = true;
        if (Vector2.Distance(endPosition, destination) > .1f)
        {
            startPosition = endPosition;
            endPosition = destination;
        }
    }

    public void RebasePosition(Vector2 oldPosition)
    {
        animator.transform.position = oldPosition;
        endPosition = oldPosition;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (Vector2.Distance(animator.transform.position, endPosition) > .1f)
        {
            animator.transform.position = Vector2.MoveTowards(animator.transform.position, endPosition, step);
        }
        else if (animator.enabled)
        {
            //TODO Tell TaskManager that it's there.
            GetComponentInParent<Network>().SendMessage("Result");
            animator.enabled = false;
        }

    }
}