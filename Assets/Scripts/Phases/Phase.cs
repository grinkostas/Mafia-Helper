using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Phase : MonoBehaviour, IMomento<PhaseSnapshot>
{
    [SerializeField] private string _phaseName;
    public string PhaseName => _phaseName;
    public UnityAction TurnEnded;
    public UnityAction PhaseEnded;
    public UnityAction<string> Log;

    public int CurrentTurn { get; protected set; }
    protected List<Player> Players;

    private void Awake()
    {
        CurrentTurn = 0;
    }

    public virtual void EnterPhase(List<Player> players)
    {
        Log?.Invoke($"\n==={PhaseName} №{Turn.NightCount}===");
        Players = players;
    }

    public abstract void NextPlayer();

    public abstract void RollbackToTurn(int turn);
    public abstract void JumpToTurn(int turnNumber);

    public virtual void ExitPhase()
    {
        CurrentTurn = 0;
    }

    public PhaseSnapshot TakeSnapshot()
    {
        return new PhaseSnapshot(this, Players);
    }
}
