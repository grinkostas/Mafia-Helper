using System.Collections.Generic;
using UnityEngine;

public class DeckBuilder : MonoBehaviour
{
    [SerializeField] private DeckComponent _deckComponentPrefab;
    [SerializeField] private ScrollableLayout _layout;

    [SerializeField] private CardSetting _cardSetting;
    [SerializeField] private SaveSystem _saveSystem;

    [Header("Edit Mode")] 
    [SerializeField] private Menu _editDeckComponentsMenu;
    [SerializeField] private Menu _defaultDeckComponentsMenu;
    
    private List<PlayingCard> _playingCards = new List<PlayingCard>();
    private List<DeckComponent> _deckComponents = new List<DeckComponent>();

    private bool _editMode = false;

    private void OnEnable()
    {
        _saveSystem.CardsUpdated += ShowComponents;
    }

    private void OnDisable()
    {
        _saveSystem.CardsUpdated -= ShowComponents;
    }

    private void RemoveComponents()
    {
        foreach (var deckComponent in _deckComponents)
        {
            RemoveComponent(deckComponent);
        }
        _deckComponents.Clear();
    }

    private void RemoveComponent(DeckComponent deckComponent)
    {
        if(deckComponent == null)
            return;
        deckComponent.Deleted -= OnDeckComponentDeleted;
        Destroy(deckComponent.gameObject);
    }

    private void CreateComponent(PlayingCard card)
    {
        DeckComponent instance = Instantiate(_deckComponentPrefab, _layout.transform);
        instance.Initialize(card, _cardSetting);
        _deckComponents.Add(instance);
        instance.Deleted += OnDeckComponentDeleted;
        if(_editMode)
            instance.EnableEditMode();
    }
    
    private void OnDeckComponentDeleted()
    {
    }
    
    public void ShowComponents()
    {
        RemoveComponents();
        
        _playingCards = _saveSystem.GetCards();
        
        foreach (var card in _playingCards)
        {
            CreateComponent(card);
        }
    }
    
    public List<PlayingCard> GetDeck()
    {
        List<PlayingCard> deck = new List<PlayingCard>();
        foreach (var component in _deckComponents)
        {
            deck.Add(component.PlayingCard, component.Count);
        }
        return deck;
    }

    public void EnableEditMode()
    {
        _editMode = true;
        _editDeckComponentsMenu.Show();
        _defaultDeckComponentsMenu.Hide();
        foreach (var deckComponent in _deckComponents)
        {
            deckComponent.EnableEditMode();
        }
    }

    public void DisableEditMode()
    {
        _editMode = false;
        _editDeckComponentsMenu.Hide();
        _defaultDeckComponentsMenu.Show();
        foreach (var deckComponent in _deckComponents)
        {
            deckComponent.DisableEditMode();
        }
    }

    public void AddCard()
    {
        PlayingCard card = new PlayingCard();
        _saveSystem.AddCard(card);
        
    }

}
