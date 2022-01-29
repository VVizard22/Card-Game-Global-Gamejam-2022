using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardToFuse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    public static event Action<GameObject> SelectCard;
    public static event Action<GameObject> ActualSelection;

    public bool mainFusion { get; private set; } = false;

    public bool currentlySelected { get; private set; } = false;

    public GameObject _originalCardReference { get; private set; }
    public CardBase _cardData { get; private set; }    
    private Vector3 originalScale;
    private RectTransform rectTransform;
    public bool _mouseOver { get; private set; } = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = originalScale;
        ActualSelection += CheckSelection;
    }

    void OnDestroy()
    {
        ActualSelection -= CheckSelection;
    }

    public void SetOriginalReference(GameObject cardReference) {
        _originalCardReference = cardReference;
        originalScale = _originalCardReference.transform.localScale;
    } 
    public void SetCardData(CardBase data) => _cardData = data;
    public void SetMainFusion() => mainFusion = true;

    public void OnPointerEnter(PointerEventData eventData){
        if(!mainFusion && !currentlySelected){
            _mouseOver = true;
            rectTransform.localScale = new Vector3(rectTransform.localScale.x + .03f, rectTransform.localScale.y + .03f, 1);
        }
    } 

    public void OnPointerExit(PointerEventData eventData) {
        if(!mainFusion && !currentlySelected){
            _mouseOver = false;
            rectTransform.localScale = originalScale;
        }
    }

    public void OnPointerDown(PointerEventData eventData){
        if(_mouseOver){
            SelectCard?.Invoke(_originalCardReference);
            ActualSelection?.Invoke(gameObject);
        }
    }

    public void CheckSelection(GameObject currentSelection){
        if(currentSelection == gameObject)
            currentlySelected = true;
        else
            currentlySelected = false;
        CheckSizeChanges();
    }

    void CheckSizeChanges(){
        if(currentlySelected)
            rectTransform.localScale = new Vector3(rectTransform.localScale.x + .03f, rectTransform.localScale.y + .03f, 1);
        else
            rectTransform.localScale = originalScale;
    }
}
