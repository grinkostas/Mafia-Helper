using System;
using UnityEngine;

[Serializable]
public class TurnSnapshot : ISnapshot
{
    private Turn _turn;
    private PhaseSnapshot _phaseSnapshot;
    private ActionLogSnapshot _actionLogSnapshot;
    private int _night;

    public TurnSnapshot(Turn turn, Phase currentPhase, ActionLog actionLog)
    {
        _turn = turn;
        _actionLogSnapshot = actionLog.TakeSnapshot();
        _phaseSnapshot = currentPhase.TakeSnapshot();
        _night = Turn.NightCount;
    }
    
    public void Rollback()
    {
        _turn.SetPhase(_phaseSnapshot.TurnPhase);
        _phaseSnapshot.Rollback();
        _actionLogSnapshot.Rollback();
        Turn.NightCount = _night;
    }
}
