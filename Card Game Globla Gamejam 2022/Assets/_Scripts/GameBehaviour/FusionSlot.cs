using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionSlot : MonoBehaviour
{
    
    public GameObject _objectAttatched { get; private set; } = null;
    public Vector2 anchoredPos { get; private set; }
    
    void Awake()
    {
        anchoredPos = GetComponent<RectTransform>().anchoredPosition;        
    }

    public void EmptySlot(){
        _objectAttatched = null;
    }

    public void FillSlot(GameObject gameObject){
        _objectAttatched = gameObject;
        /*
        if (gameObject.GetComponent<CardToFuse>() != null)
            gameObject.GetComponent<CardToFuse>().SetSlotAssignedTo(GetComponent<RectTransform>());*/
    }

}
