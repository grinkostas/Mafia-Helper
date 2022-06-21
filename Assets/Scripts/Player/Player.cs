using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Player : MonoBehaviour, IMomento<PlayerSnapshot>
{
    [Header("UI")]
    [SerializeField] private TMP_Text _numberText;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Button _button;
    
    [Header("Menus")]
    [SerializeField] private Menu _aliveMenu;
    [SerializeField] private Menu _kickedMenu;
    
    [Header("Colors")]
    [SerializeField] private Color _defaultColor;
    [SerializeField] private Color _activeColor;
    [SerializeField] private Color _availableColor;
    [SerializeField] private Color _selectedColor;
   
    
    private PlayingCard _playingCard;
    private int _orderNumber;
    private List<string> _actionHistory = new List<string>();
    public List<string> ActionHistory => _actionHistory;


    public UnityAction<Player> Click;
    public PlayingCard Card => _playingCard;
    public PlayerState State { get; private set; }
    public VoteData VoteData { get; private set; }
    public bool IsAlive => State != PlayerState.Kicked;
    public int OrderNumber => _orderNumber;
    public string Name => $"№{_orderNumber}";
    
    
    private void OnEnable()
    {
        _button.onClick.AddListener(()=>Click?.Invoke(this));
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(()=>Click?.Invoke(this));
    }

    public void Initialize(int orderNumber, PlayingCard playingCard)
    {
        State = PlayerState.Default;
        _orderNumber = orderNumber;
        _playingCard = playingCard;
        _backgroundImage.color = _defaultColor;
        _numberText.text = orderNumber.ToString();
        _kickedMenu.Hide();
        _aliveMenu.Show();
        VoteData = new VoteData();
    }

    public void Kick()
    {
        _aliveMenu.Hide();
        _kickedMenu.Show();
        State = PlayerState.Kicked;
        VoteData.RemoveFromVote();
    }

    public void Revive()
    {
        _aliveMenu.Show();
        _kickedMenu.Hide();
        State = PlayerState.Default;
    }
    public void Available()
    {
        if(State == PlayerState.Kicked)
            return;
        ActualizeState(PlayerState.Available);
    }

    public void Active()
    {
        if(State == PlayerState.Kicked)
            return;
        ActualizeState(PlayerState.Active);
    }

    public void Idle()
    {
        if(State == PlayerState.Kicked)
            return;
        ActualizeState(PlayerState.Default);
    }

    public void Select()
    {
        if(State == PlayerState.Kicked)
            return;
        if(State == PlayerState.Selected)
            ActualizeState(PlayerState.Default);
        else
            ActualizeState(PlayerState.Selected);
    }

    public void ActualizeState(PlayerState state)
    {
        State = state;
        if(_aliveMenu == null || _backgroundImage == null)
            return;
        
        if (state != PlayerState.Kicked)
        { 
            _aliveMenu.Show();
            _kickedMenu.Hide();     
        }
        switch (state)
        {
            case PlayerState.Active:
                _backgroundImage.color = _activeColor;
                break;
            case PlayerState.Available:
                _backgroundImage.color = _availableColor;
                break;
            case PlayerState.Default:
                _backgroundImage.color = _defaultColor;
                break;
            case PlayerState.Selected:
                _backgroundImage.color = _selectedColor;
                break;
            case PlayerState.Kicked:
                _aliveMenu.Hide();
                _kickedMenu.Show();
                break;
        }
    }

    public void ActualizeVoteData(VoteData data)
    {
        VoteData = new VoteData(data);
    }
    
    public void RewriteLog(List<string> newLog)
    {
        _actionHistory = new List<string>(newLog);
    }

    public void AddAction(int playerNumber)
    {
        string action = $"{Card.ActionText} игрока {playerNumber} в ночь №{Turn.NightCount}";
        _actionHistory.Add(action);
    }

    public void PutToTheVote(int turn)
    {
        if(VoteData.PresentTurn > turn)
            VoteData.PutOnVote(turn);
    }

    public void ResetVotes()
    {
        VoteData = new VoteData();
    }
    
    public PlayerSnapshot TakeSnapshot()
    {
        return new PlayerSnapshot(this);
    }
}
