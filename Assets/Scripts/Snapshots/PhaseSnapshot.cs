using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class PhaseSnapshot : ISnapshot
{
    private List<PlayerSnapshot> _playerSnapshots;
    [SerializeField] private int _turnNumber;
    
    public Phase TurnPhase { get; private set; }
    
    public PhaseSnapshot(Phase phase, List<Player> players)
    {
        TurnPhase = phase;
        _turnNumber = phase.CurrentTurn-1;
        _playerSnapshots = players.Select(x=> x.TakeSnapshot()).ToList();
    }
    
    public void Rollback()
    {
        
        foreach (var playerSnapshot in _playerSnapshots)
        {
            playerSnapshot.Rollback();
        }
        TurnPhase.RollbackToTurn(_turnNumber);
        
        Debug.Log($"Turn back to {_turnNumber}");
    }
}
