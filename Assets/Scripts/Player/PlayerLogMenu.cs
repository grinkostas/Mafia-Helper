using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerLogMenu : Menu
{
    [SerializeField] private TMP_Text _logText;
    [SerializeField] private int _logNotesLimit;
    [SerializeField] private TMP_Text _playerName;
    private Player _currentPlayer;
    
    public void Show(Player player)
    {
        if(player == null)
            return;
        _currentPlayer = player; 
        _playerName.text = player.Name; 
        
        var tempLog = _currentPlayer.ActionHistory
            .Skip(Mathf.Max(0, _currentPlayer.ActionHistory.Count - _logNotesLimit)).ToList();
        _logText.text = string.Join("\n", tempLog.ToArray());
        base.Show();
    }
    
    public override void Show()
    {
        Show(_currentPlayer);
        
    }
}
