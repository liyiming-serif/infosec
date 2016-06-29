using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ClickHandler : MonoBehaviour, IPointerClickHandler
{

    public static TopCommand subCommandToBeChanged;

    public SubCommand.Code myCode;

    public Animator animator;

    public static int checkUpdate, isUpdated;

    #region IPointerClickHandler implementation
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (subCommandToBeChanged && animator.enabled)
        {
            isUpdated = 1;
            ExecuteEvents.Execute<IUpdateSubCMDChoice>(subCommandToBeChanged.gameObject, null, (x, y) => x.FinaliseSubCMDChoice(myCode));
            subCommandToBeChanged = null;
        }
    }
    #endregion

    void Update()
    {
       
        if (subCommandToBeChanged && !animator.enabled)
        {
            if (subCommandToBeChanged.myCode == TopCommand.Code.Outbox && (myCode == SubCommand.Code.Boss || myCode == SubCommand.Code.Distrust))
            {
                animator.enabled = true;
            }
            else if ((subCommandToBeChanged.myCode == TopCommand.Code.Store || subCommandToBeChanged.myCode == TopCommand.Code.Load) && (myCode == SubCommand.Code.Zero || myCode == SubCommand.Code.One))
            {
                animator.enabled = true;
            
            }
        }else if(subCommandToBeChanged == null && animator.enabled)
        {
            animator.enabled = false;
        }
    }


    void Start()
    {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
}
