using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBehaviour : MonoBehaviour, IDropHandler
{
    public static event Action<GameObject> StartFusing;

    public bool CombatSlot = false;

    public GameObject _objectAttatched { get; private set; } = null;
    public Vector2 anchoredPos { get; private set; }

    void Awake()
    {
        anchoredPos = GetComponent<RectTransform>().anchoredPosition;        
    }

    public void OnDrop(PointerEventData eventData){
        GameObject current = eventData.pointerDrag.gameObject;


        if (eventData.pointerDrag != null && _objectAttatched == null) {
            if(CombatSlot){
                ManageFusion(current);
            }

            if(current.GetComponent<CardBehaviour>() != null && current.GetComponent<CardBehaviour>()._slotAssignedTo != null)
                current.GetComponent<CardBehaviour>()._slotAssignedTo.gameObject.GetComponent<SlotBehaviour>().EmptySlot();
            FillSlot(eventData.pointerDrag.gameObject);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        }
    }

    public void EmptySlot(){
        _objectAttatched = null;
    }

    public void FillSlot(GameObject gameObject){
        _objectAttatched = gameObject;
        if (gameObject.GetComponent<CardBehaviour>() != null)
            gameObject.GetComponent<CardBehaviour>().SetSlotAssignedTo(GetComponent<RectTransform>());
    }

    private void ManageFusion(GameObject cardToFuse){
        StartFusing?.Invoke(cardToFuse);
    }

}
