public class VoteData
{
    public bool IsOnVote { get; private set; }
    public int PresentTurn { get; private set; }
    public int VoteCount { get; set; }

    private void ResetData()
    {
        VoteCount = 0;
        PresentTurn = int.MaxValue;
    }
    
    public VoteData()
    {
        RemoveFromVote();
    }

    public VoteData(VoteData data)
    {
        IsOnVote = data.IsOnVote;
        PresentTurn = data.PresentTurn;
        VoteCount = data.VoteCount;
    }

    public void PutOnVote(int turn)
    {
        IsOnVote = true;
        PresentTurn = turn;
    }

    public void RemoveFromVote()
    {
        IsOnVote = false;
        ResetData();
    }
}