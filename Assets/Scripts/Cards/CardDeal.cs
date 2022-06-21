using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardDeal : MonoBehaviour
{
    [SerializeField] private DeckBuilder _deckBuilder;
    [SerializeField] private DealtCardView _dealtCardView;
    [SerializeField] private Menu _cardDealMenu;
    
    private List<PlayingCard> _deck;
    private PlayingCard _currentCard;

    public UnityAction<PlayingCard> CardDealt;
    public UnityAction DealEnded;

    private void ConfirmCard()
    {
        CardDealt?.Invoke(_currentCard);
        _deck.Remove(_currentCard);
        _currentCard = null;
        _deck.Shuffle();
        DealNextCard();
    }
    
    private void DealNextCard()
    {
        if (_deck.Count == 0)
        {
            EndDeal();
            return;
        }

        _currentCard = _deck[0];
        ShowDealtCard();
    }
    
    private void ShowDealtCard()
    {
        _dealtCardView.NextCard(_currentCard);
    }
    
    private void EndDeal()
    {
        DealEnded?.Invoke();
        _cardDealMenu.Hide();
    }
    
    public void OnConfirmCardButtonClick()
    {
        ConfirmCard();
    }
    
    public void ReRollCard()
    {
        _currentCard = _deck[Random.Range(1, _deck.Count)];
        _dealtCardView.Actualize(_currentCard);
    }
    public void StartDeal()
    {
        var deck = _deckBuilder.GetDeck();
        deck.Shuffle();
        _deck = deck;
        
        _cardDealMenu.Show();
        _currentCard = _deck[0];
        _dealtCardView.Show(_currentCard);
    }
}
