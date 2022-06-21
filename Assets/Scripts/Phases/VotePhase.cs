using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class VotePhase : Phase
{
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private TMP_Text _phaseNameText;

    [SerializeField] private Menu _voteMenu;
    [SerializeField] private VoteComponent _voteComponentPrefab;
    [SerializeField] private Transform _voteComponentsParent;
    
    private List<Player> _playersOnVote;
    private List<VoteComponent> _voteComponents = new List<VoteComponent>();
    
    
    private VoteComponent _currentVoteComponent;
    private void DeleteComponents()
    {
        foreach (var voteComponent in _voteComponents)
        {
            Destroy(voteComponent.gameObject);
        }
        _voteComponents.Clear();
    }

    private void CreateComponents()
    {
        foreach (var player in _playersOnVote)
        {
            var instance = Instantiate(_voteComponentPrefab, _voteComponentsParent);
            _voteComponents.Add(instance);
            instance.Initialize(player);
        }
    }

    private void FinishVote()
    {
        int maxVotes = _playersOnVote.Max(x => x.VoteData.VoteCount);
        var playersWithMaxVotes = _playersOnVote.Where(x => x.VoteData.VoteCount == maxVotes).ToList();
        
        foreach (var player in _playersOnVote)
            player.VoteData.VoteCount = 0;
        
        
        if (playersWithMaxVotes.Count > 1 && maxVotes > 0)
        {
            _playersOnVote = playersWithMaxVotes;
            CurrentTurn = 0;
            DeleteComponents();
            CreateComponents();
            JumpToTurn(0);
            return;
        }

        if (maxVotes > 0)
        {
            var votedPlayer = playersWithMaxVotes.Single();
            votedPlayer.Kick();
            Log?.Invoke($"{votedPlayer.Name} повешен");
        }
        PhaseEnded?.Invoke();
    }

    public override void EnterPhase(List<Player> players)
    {
        base.EnterPhase(players);
        DeleteComponents();
        CurrentTurn = 0;
        _playersOnVote = players.Where(x => x.VoteData.IsOnVote).OrderBy(x=>x.VoteData.PresentTurn).ToList();
        if (_playersOnVote.Count == 0 || _playersOnVote == null)
        {
            PhaseEnded?.Invoke();
            return;
        }

        _playerNameText.gameObject.SetActive(false);
        _phaseNameText.text = PhaseName;
        CreateComponents();
        _voteMenu.Show();
        _currentVoteComponent = null;
        JumpToTurn(CurrentTurn);
        CurrentTurn++;

    }

    public override void ExitPhase()
    {
        base.ExitPhase();
        _voteMenu.Hide();
        _playerNameText.gameObject.SetActive(true);
        _playersOnVote.Clear();
        foreach (var player in Players)
        {
            player.ResetVotes();
        }

        DeleteComponents();

    }

    public override void NextPlayer()
    {
        if (_currentVoteComponent != null && CurrentTurn > 0 && CurrentTurn <= _playersOnVote.Count)
        {
            _playersOnVote[CurrentTurn-1].VoteData.VoteCount = _currentVoteComponent.Count;
            
            Log?.Invoke(
                $"За {_playersOnVote[CurrentTurn-1].Name} " +
                $"проголосовало {_currentVoteComponent.Count}");
            
            TurnEnded?.Invoke();
        }

        JumpToTurn(CurrentTurn);
        CurrentTurn++;
    }

    public override void RollbackToTurn(int turn)
    {
        JumpToTurn(turn);
        CurrentTurn++;
    }

    public override void JumpToTurn(int turn)
    {   
        CurrentTurn = turn;
        
        Debug.Log($"Current turn {CurrentTurn}");
        if(_currentVoteComponent != null)
            _currentVoteComponent.HideVote();
        
        if (turn < 0 || turn > _playersOnVote.Count - 1)
        {
            FinishVote();
            return;
        }

        _currentVoteComponent = _voteComponents[turn];
        _currentVoteComponent.Counter.Count = _playersOnVote[turn].VoteData.VoteCount;
        _currentVoteComponent.ShowVote();
    }
}
