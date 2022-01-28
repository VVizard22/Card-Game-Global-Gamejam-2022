using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardCreation : MonoBehaviour
{
    public CardBase _cardData { get; private set; }
    [SerializeField] Image _artwork;
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI DMG;

    public void SetCardData(CardBase data){
        _cardData = data;
        _artwork.sprite = _cardData._cardSprite;
        UpdateCardData();
    }

    public void UpdateCardData(){
        HP.text = _cardData._baseStats.Health.ToString();
        DMG.text = _cardData._baseStats.AttackPower.ToString();
    }
}
