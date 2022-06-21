using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NightPhase : Phase
{
    [SerializeField] private TMP_Text _roleName;
    [Header("Hint")]
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private string _hint;
    
    private Player _currentPlayer;

    private Player _selectedPlayer;
    
    private List<Player> _sortedPlayers = new List<Player>();

    private void OnPlayerClick(Player player)
    {
        if (player.IsAlive == false)
        {
            return;
        }

        ClearSelect();
        if (player.State != PlayerState.Selected)
        {
            _selectedPlayer = player;
            _selectedPlayer.Select();
        }
        else
        {
            player.ActualizeState(PlayerState.Default);
            _selectedPlayer = null;
        }
        
    }

    private void LogAction()
    {
        
        if (_selectedPlayer == null)
            return;
        
        string note = $"{_currentPlayer.Card.RoleName} {_currentPlayer.Card.ActionText} {_selectedPlayer.Name}";
        Log?.Invoke(note);
        _currentPlayer.AddAction(_selectedPlayer.OrderNumber);
        TurnEnded?.Invoke();
        _selectedPlayer = null;
    }
    
    private void MafiaTurn()
    {
        foreach (var player in Players)
        {
            player.Available();
            if(player.Card.CardType is CardType.Mafia or CardType.Don)
                player.Active();
            _currentPlayer = Players.First(x => x.Card.CardType == CardType.Mafia);
            _roleName.text = _currentPlayer.Card.RoleName;
            
        }
    }
    
    private void ClearSelect()
    {
        foreach (var player in Players)
        {
            if (player.State == PlayerState.Selected)
            {
                var previousState = PlayerState.Available;
                if (_currentPlayer != null)
                {
                    if (player.Card.RoleName == _currentPlayer.Card.RoleName)
                        previousState = PlayerState.Active;
                    else if (_currentPlayer.Card.CardType == CardType.Mafia && player.Card.CardType == CardType.Don)
                        previousState = PlayerState.Active;
                }
                
                player.ActualizeState(previousState);
            }
        }
    }

    public override void EnterPhase(List<Player> players)
    {
        base.EnterPhase(players);
        _hintText.text = _hint;
        _hintText.gameObject.SetActive(true);
        CurrentTurn = -1;
        _sortedPlayers = Players.OrderBy(x => x.Card.TurnOrder).ToList();
        _sortedPlayers = _sortedPlayers.Where(
            x => x.Card.CardType is CardType.Active or CardType.Don && x.IsAlive).ToList();
        
        foreach (var player in Players)
        {
            player.Click += OnPlayerClick;
        }
        foreach(var player in _sortedPlayers)
            Debug.Log($"{player.Name}");
        
        JumpToTurn(CurrentTurn);
    }
    
    public override void NextPlayer()
    {
        LogAction();
        
        if (_currentPlayer != null && 
            ((_currentPlayer.Card.CardType == CardType.Inactive || _currentPlayer.Card.CardType == CardType.Mafia) 
             && CurrentTurn < _sortedPlayers.Count - 1))
        {
            CurrentTurn++;
            _currentPlayer = _sortedPlayers[CurrentTurn];
            NextPlayer();
            return;
        }
        
        JumpToTurn(CurrentTurn);
        CurrentTurn++;

    }

    public override void RollbackToTurn(int turn)
    {
        CurrentTurn = turn;
        int index = Mathf.Clamp(turn, 0, _sortedPlayers.Count-1);
        _currentPlayer = _sortedPlayers[index];
        _roleName.text = _sortedPlayers[index].Card.RoleName;
        ClearSelect();
    }

    public override void JumpToTurn(int turn)
    {
        if (turn >= _sortedPlayers.Count)
        {
            PhaseEnded?.Invoke();
            return;
        }

        if (turn == -1)
        {
            MafiaTurn();
            return;
        }

        if (turn < 0) turn = 0;
        _currentPlayer = _sortedPlayers[turn];

        foreach (var player in Players)
        {
            player.Available();
        }
        
        _currentPlayer.Active();
        _roleName.text = _currentPlayer.Card.RoleName;
    }

    public override void ExitPhase()
    {
        _hintText.gameObject.SetActive(false);
        foreach (var player in Players)
        {
            player.Click -= OnPlayerClick;
        }
        base.ExitPhase();
    }
}
