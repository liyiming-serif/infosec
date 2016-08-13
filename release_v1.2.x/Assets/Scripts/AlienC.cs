﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class AlienC : MonoBehaviour
{

    protected Animator animator;

    [SerializeField]
    float speed = 200.0f;
    protected Vector2 initPosition;
    public Vector2 endPosition;
    public Vector2 startPosition;
    public bool startMoving;

    void Awake()
    {
        animator = GetComponent<Animator>();
		animator.SetTrigger("stopwalk");
        initPosition = animator.transform.position;
        endPosition = initPosition;
        startPosition = endPosition;
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
            startPosition = endPosition;
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