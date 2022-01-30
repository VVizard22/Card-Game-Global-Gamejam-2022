using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButtonSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] AudioClip buttonClick;
    [SerializeField] AudioClip buttonHover;

    public void PlaySound() {
        AudioSystem.Instance.PlaySound(buttonClick);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        AudioSystem.Instance.PlaySound(buttonHover);
    }
}
