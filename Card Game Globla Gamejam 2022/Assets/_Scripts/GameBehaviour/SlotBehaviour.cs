using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotBehaviour : MonoBehaviour, IDropHandler
{
    public static event Action<GameObject> StartFusing;
    public static event Action<SlotBehaviour> SlotAssigned;
    public static event Action<SlotBehaviour> EmptyingSlot;

    [SerializeField] CardType SlotType;

    public bool CombatSlot = false;

    public GameObject _objectAttatched { get; private set; } = null;
    public Vector2 anchoredPos { get; private set; }

    public CardType GetSlotType(){
        return SlotType;
    }

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

            if(current.GetComponent<CardBehaviour>() != null && current.GetComponent<CardBehaviour>()._slotAssignedTo != null && !CombatSlot && current.GetComponent<CardBehaviour>()._interactuable){
                current.GetComponent<CardBehaviour>()._slotAssignedTo.gameObject.GetComponent<SlotBehaviour>().EmptySlot();
                FillSlot(eventData.pointerDrag.gameObject);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
            }
        }
    }

    public void EmptySlot(){
        SlotAssigned?.Invoke(this);
        EmptyingSlot?.Invoke(this);
        _objectAttatched = null;
    }

    public void FillSlot(GameObject gameObject){
        _objectAttatched = gameObject;
        if (gameObject.GetComponent<CardBehaviour>() != null)
            gameObject.GetComponent<CardBehaviour>().SetSlotAssignedTo(GetComponent<RectTransform>());
        SlotAssigned?.Invoke(this);
    }

    private void ManageFusion(GameObject cardToFuse){
        StartFusing?.Invoke(cardToFuse);
    }

}
