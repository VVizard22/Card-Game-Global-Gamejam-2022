using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardBase _cardData;
    public RectTransform _slotAssignedTo { get; private set; } = null;
    
    public bool _onCombatSlot { get; private set; }

    public Canvas _canvas { get; private set; }
    public RectTransform rectTransform { get; private set; }
    private CanvasGroup canvasGroup;

    void Awake()
    {
        _cardData = GetComponent<CardCreation>()._cardData;
        rectTransform = GetComponent<RectTransform>();        
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetCanvas(Canvas canvas) => _canvas = canvas;

    public void SetSlotAssignedTo(RectTransform slot){
        _slotAssignedTo = slot;
        _onCombatSlot = slot.GetComponent<SlotBehaviour>().CombatSlot;
    }
    
    public void OnPointerEnter(PointerEventData eventData){
        if(!eventData.dragging){
            rectTransform.localScale = new Vector3(rectTransform.localScale.x + .03f, rectTransform.localScale.y + .03f, 1);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(!eventData.dragging){
            rectTransform.localScale = new Vector3(rectTransform.localScale.x - .03f, rectTransform.localScale.y - .03f, 1);
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(!_onCombatSlot){
            gameObject.transform.SetAsLastSibling();
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData){
        if(!_onCombatSlot){
            rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        if(!_onCombatSlot){
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            ReturnPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData){
    }
    
    public void ReturnPosition(){
        if(_slotAssignedTo != null)
            rectTransform.anchoredPosition = _slotAssignedTo.anchoredPosition;
    }

    public void DestroyCard(){
        Destroy(this.gameObject);
    }

    public GameObject CloneCard(){
        GameObject newCard = Instantiate(gameObject, transform.position, Quaternion.identity);;
        return newCard;
    }
}
