using System.Collections.Generic;

public class PlayerSnapshot : ISnapshot
{
    private Player _player;
    private PlayerState _state;
    private VoteData _voteData;
    private List<string> _actionHistory;

    public PlayerSnapshot(Player player)
    {
        _player = player;
        _actionHistory = new List<string>(player.ActionHistory);
        _state = player.State;
        _voteData = new VoteData(player.VoteData);
    }

    public void Rollback()
    {
        _player.ActualizeState(_state);
        _player.RewriteLog(_actionHistory);
        _player.ActualizeVoteData(_voteData);
    }
}
