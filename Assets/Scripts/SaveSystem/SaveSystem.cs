using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class SaveSystem : ScriptableObject, ISaveSystem
{
    public static string Path => Application.persistentDataPath + "/Cards.json";
    public static string DefaultPath => Application.persistentDataPath + "/DefaultCards.json";

    private List<PlayingCard> _currentCards = new List<PlayingCard>();
    private FileStream _fileStream;
    public UnityAction CardsUpdated { get; set; }
    
    private void GetDefaultCards()
    {
        ReadCardsFromFile(DefaultPath);
    } 

    private void ReloadCards()
    {
        if (File.Exists(Path) == false || new FileInfo(Path).Length == 0)
        {
            GetDefaultCards();
            return;
        }

        ReadCardsFromFile(Path);
        CardsUpdated?.Invoke();
    }

    private void ReadCardsFromFile(string path)
    {
        _currentCards.Clear();
        var cards = new Wrapper<PlayingCard>();
        string json;
        using (var streamReader = new StreamReader(path))
        {
            json = streamReader.ReadToEnd();
        }
        var tempCards = JsonUtility.FromJson<Wrapper<PlayingCard>>(json);
        if (tempCards != null && tempCards.Items.Length > 0)
            cards = tempCards;
        
        var result = cards.ToList();
        if (result.Count <= 0)
        {
            GetDefaultCards();
        }
        _currentCards = result;
    }

    public List<PlayingCard> GetCards()
    {
        if(_currentCards.Count == 0)
            ReloadCards();
        return _currentCards;
    }
    
    public void SaveCards()
    {
        Wrapper<PlayingCard> wrappedCards =new  Wrapper<PlayingCard>(_currentCards.ToArray());
        string json = JsonUtility.ToJson(wrappedCards);
        using (var streamWriter = new StreamWriter(Path, false))
        {
            streamWriter.WriteLine(json);
        }
        ReloadCards();
    }

    public void AddCard(PlayingCard card)
    {
        card.Id = _currentCards.Max(x => x.Id) + 1;
        _currentCards.Add(card);
        SaveCards();
    }

    public void DeleteCard(PlayingCard card)
    {
        if (_currentCards.Contains(card) == false)
            return;
        
        _currentCards.Remove(card);
       SaveCards();
       CardsUpdated?.Invoke();
        
    }
}
