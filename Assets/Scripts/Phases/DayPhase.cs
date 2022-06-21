using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DayPhase : Phase
{
    [SerializeField] private TMP_Text _roleName;
    
    [Header("Hint")]
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private string _hint;
    
    private Player _currentPlayer;
    private Player _selectedPlayer;
    
    private void ClearSelect()
    {
        foreach (var player in Players)
        {
            if (player.State != PlayerState.Selected)
                continue;
            
            var previousState = PlayerState.Default;
            if (_currentPlayer != null && _currentPlayer == _selectedPlayer)
                previousState = PlayerState.Active;
            
            player.ActualizeState(previousState);
            
        }
    }
    
    private void OnPlayerClick(Player player)
    {
        if (player.IsAlive == false)
            return;

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

    public override void EnterPhase(List<Player> players)
    {
        base.EnterPhase(players);
        _hintText.text = _hint;
        _hintText.gameObject.SetActive(true);
        Players = new List<Player>(players).Where(x=>x.IsAlive).ToList();
        Players.FirstToBack(Turn.NightCount-1);
        CurrentTurn = 0;

        foreach (var player in players)
            player.Click += OnPlayerClick;
        
        JumpToTurn(CurrentTurn);
        CurrentTurn++;
    }
    public override void ExitPhase()
    {
        base.ExitPhase();
        _hintText.gameObject.SetActive(false);
        foreach (var player in Players)
        {
            player.Click -= OnPlayerClick;
        }

    }
    public override void NextPlayer()
    {
        
        TurnEnded?.Invoke();
        if (_selectedPlayer != null && _currentPlayer != null)
        {
            _selectedPlayer.PutToTheVote(CurrentTurn);
            Log?.Invoke($"{_currentPlayer.Name} выставил {_selectedPlayer.Name}");
            _selectedPlayer = null;
        }
        else if(_currentPlayer != null)
        {
            Log?.Invoke($"{_currentPlayer.Name} никого не выставил");
        }
        JumpToTurn(CurrentTurn);
        CurrentTurn++;
    }

    public override void RollbackToTurn(int turn)
    {
        CurrentTurn = turn;
        _roleName.text = Players[turn].Name;
        JumpToTurn(turn);
        CurrentTurn++;
    }

    public override void JumpToTurn(int turn)
    {
        if (turn >= Players.Count)
        {
            PhaseEnded?.Invoke();
            return;
        }

        if (turn < 0) turn = 0;
        
        CurrentTurn = turn;
        _currentPlayer = Players[turn];
        
        if (_currentPlayer.IsAlive == false)
        {
            CurrentTurn++;
            NextPlayer();
            return;
        }
        
        foreach (var player in Players)
            player.Idle();
        _roleName.text = "Игрок " + _currentPlayer.Name;
        _currentPlayer.Active();
    }
}
