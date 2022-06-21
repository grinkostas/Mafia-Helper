using System.Collections.Generic;
using UnityEngine.Events;

public interface ISaveSystem
{
    public UnityAction CardsUpdated { get; set; }
    
    List<PlayingCard> GetCards();
    void SaveCards();
    void AddCard(PlayingCard card);
    void DeleteCard(PlayingCard card);
}
