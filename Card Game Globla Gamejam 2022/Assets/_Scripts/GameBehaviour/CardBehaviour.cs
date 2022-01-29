using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardBehaviour : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler,
    IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardBase _cardData { get; private set; }
    public RectTransform _slotAssignedTo { get; private set; } = null;
    
    public Vector3 originalScale { get; private set; }

    public bool _fusionOption { get; private set; } = false;

    public bool _interactuable { get; private set; } = true;

    public bool _onCombatSlot { get; private set; }

    public Canvas _canvas { get; private set; }
    public RectTransform rectTransform { get; private set; }
    private CanvasGroup canvasGroup;

    void Start()
    {
        _cardData = gameObject.GetComponent<CardCreation>()._cardData;
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;    
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetCanvas(Canvas canvas) => _canvas = canvas;

    public void SetSlotAssignedTo(RectTransform slot){
        _slotAssignedTo = slot;
        _onCombatSlot = slot.GetComponent<SlotBehaviour>().CombatSlot;
    }
    
    public void SetInteractuable(bool interactuable){
        _interactuable = interactuable;
    }
    
    public void SetFusionOption(bool fusionOption){
        _fusionOption = fusionOption;
    }
    public void OnPointerEnter(PointerEventData eventData){
        if(!eventData.dragging && _interactuable){
            rectTransform.localScale = new Vector3(rectTransform.localScale.x + .03f, rectTransform.localScale.y + .03f, 1);
        }
    }

    public void OnPointerExit(PointerEventData eventData){
        if(!eventData.dragging && _interactuable){
            rectTransform.localScale = originalScale;
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        if(!_onCombatSlot && _interactuable){
            gameObject.transform.SetAsLastSibling();
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData){
        if(!_onCombatSlot && _interactuable){
            rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData){
        rectTransform.localScale = originalScale;
        canvasGroup.alpha = 1f;
        if(!_onCombatSlot && _interactuable){
            canvasGroup.blocksRaycasts = true;
            ReturnPosition();
        }
    }

    public void OnPointerDown(PointerEventData eventData){
    }

    public void RestoreVisuals(){
        rectTransform.localScale = originalScale;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
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
