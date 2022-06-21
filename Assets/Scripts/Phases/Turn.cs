using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Turn : MonoBehaviour, IMomento<TurnSnapshot>
{
    [SerializeField] private List<Phase> _phases; 
    [SerializeField] private TMP_Text _phaseNameText;
    
    [SerializeField] private ActionLog _actionLog;

    public static int NightCount = 0;
    
    private List<Player> _players;
    [SerializeField] private List<TurnSnapshot> _turnsHistory = new List<TurnSnapshot>();

    private Phase _currentPhase;
    private int _currentPhaseIndex = 0;

    private void OnTurnEnded()
    {
        _turnsHistory.Add(TakeSnapshot());
    }
    
    private Phase GetNextPhase()
    {
        var index = _currentPhaseIndex + 1;
        if (index >= _phases.Count)
        {
            index = 0;
            NightCount++;
        }

        return _phases[index];
    }

    private void OnLog(string note)
    {
        _actionLog.AddNote(note);
    }

    private void OnEnable()
    {
        foreach (var phase in _phases)
        {
            phase.TurnEnded -= OnTurnEnded;
            phase.Log -= OnLog; 
        }
    }

    public void Initialize(List<Player> players)
    {
        NightCount = 1;
        _players = players;
        foreach (var phase in _phases)
        {
            phase.TurnEnded += OnTurnEnded;
            phase.Log += OnLog;
        }
        SetPhase(_phases[0]);
    }

    public void SetPhase(Phase phase)
    {
        if (_currentPhase != null)
        {
            _currentPhase.ExitPhase();
            _currentPhase.PhaseEnded -= NextPhase;
        }

        _currentPhase = phase;
        
        _currentPhaseIndex = _phases.IndexOf(phase);
        _currentPhase.PhaseEnded += NextPhase;
        _currentPhase.EnterPhase(_players);
        _phaseNameText.text = _currentPhase.PhaseName;
    }
    
    private void NextPhase()
    {
        SetPhase(GetNextPhase());
    }

    public void PreviousTurn()
    {
        if (_turnsHistory.Count == 0)
            return;

        int index = _turnsHistory.Count - 1;
        var turnSnapshot = _turnsHistory[index];
        _turnsHistory.RemoveAt(index);
        turnSnapshot.Rollback();
    }
    
    public void NextTurn()
    {
        _currentPhase.NextPlayer();
    }

    
    public TurnSnapshot TakeSnapshot()
    {
        return new TurnSnapshot(this, _currentPhase, _actionLog);
    }
}
