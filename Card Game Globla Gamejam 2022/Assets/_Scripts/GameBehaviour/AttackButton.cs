using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    void Awake()
    {
        SlotBehaviour.SlotAssigned += CheckActivation;
    }

    void Start(){
        gameObject.GetComponent<Button>().interactable = false;
    }

    void OnDestroy()
    {
        SlotBehaviour.SlotAssigned -= CheckActivation;
    }

    public void CheckActivation(SlotBehaviour slot){
        if(slot.CombatSlot){
            if(slot._objectAttatched != null)
                gameObject.GetComponent<Button>().interactable = true;
            else
                gameObject.GetComponent<Button>().interactable = false;
        }
    }
}
