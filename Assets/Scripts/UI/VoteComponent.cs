using TMPro;
using UnityEngine;

public class VoteComponent : MonoBehaviour
{
    [SerializeField] private string _playerDefaultName;
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private Counter _counter;

    public Counter Counter => _counter;
    public int Count => _counter.Count;

    public void Initialize(Player player)
    {
        _counter.Hide();
        _playerName.text = _playerDefaultName + " " + player.OrderNumber.ToString();
    }
    
    public void ShowVote()
    {
        _counter.Show();
    }

    public void HideVote()
    {
        _counter.Hide();
    }
}
