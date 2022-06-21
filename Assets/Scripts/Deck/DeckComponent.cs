using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(RectTransform))]
public class DeckComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _roleName;
    [SerializeField] private Counter _counter;

    [SerializeField] private Menu _editMode;
    [SerializeField] private Menu _defaultMode;

    [SerializeField] private SaveSystem _saveSystem;
    
    private CardSetting _cardSettingMenu;
    public PlayingCard PlayingCard { get; private set; }
    public int Count => _counter.Count;

    public UnityAction Deleted;
    
    public void Initialize(PlayingCard card, CardSetting cardSetting)
    {
        PlayingCard = card;
        _roleName.text = card.RoleName;
        _cardSettingMenu = cardSetting;
    }

    public void EnableEditMode()
    {
        _defaultMode.Hide();
        _editMode.Show();
    }

    public void DisableEditMode()
    {
        _defaultMode.Show();
        _editMode.Hide();
    }

    public void Edit()
    {
        _cardSettingMenu.Show(PlayingCard);
    }

    public void Delete()
    {
        _saveSystem.DeleteCard(PlayingCard);
        Deleted?.Invoke();
    }

}
