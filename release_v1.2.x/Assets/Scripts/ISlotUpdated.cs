using UnityEngine.EventSystems;
using UnityEngine;

public interface ISlotUpdated : IEventSystemHandler
{
    void NoticeNetworkURLBoard(Domain d, int id);
}
