using System;
using System.Linq;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardSetting : Menu<PlayingCard>
{
    [SerializeField] private TMP_InputField _roleName;
    [SerializeField] private TMP_InputField _roleDescription;
    [SerializeField] private TMP_InputField _actionText;
    [SerializeField] private TMP_Dropdown _cardType;
    [SerializeField] private Counter _counter;
    [SerializeField] private SaveSystem _saveSystem;
    
    private PlayingCard _playingCard;

    private void Initialize(PlayingCard card)
    {
        _playingCard = card;
        _roleName.text = _playingCard.RoleName;
        _roleDescription.text = card.RoleDescription;
        _actionText.text = card.ActionText;

        _cardType.options = GetOptionData();
        _cardType.value = (int)card.CardType;

    }

    private List<TMP_Dropdown.OptionData> GetOptionData()
    {
        return Enum.GetValues(typeof(CardType)).Cast<CardType>().Select(x=> 
            new TMP_Dropdown.OptionData(x.GetTypeName())).ToList();
    }
    
    public override void Show(PlayingCard card)
    {
        Initialize(card);
        base.Show(card);
    }
    
    public void Save()
    {
        _playingCard.RoleName = _roleName.text;
        _playingCard.RoleDescription = _roleDescription.text;
        _playingCard.ActionText = _actionText.text;
        _playingCard.CardType = (CardType)_cardType.value;
        _playingCard.TurnOrder = _counter.Count;
        
        _saveSystem.SaveCards();
        base.Hide();
    }
}
