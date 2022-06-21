using UnityEngine;

public class StartGameMenu : Menu
{
    [SerializeField] private DeckBuilder _deckBuilder;
    
    public override void Show()
    {
        _deckBuilder.ShowComponents();
        base.Show();
    }
}
