using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ActionLog : Menu, IMomento<ActionLogSnapshot>
{
    [SerializeField] private TMP_Text _logText;
    [SerializeField] private int _actionLimit;
    private List<string> _log = new List<string>();
    public List<string> Log => _log;
    
    private void UpdateLog()
    {
        List<string> tempLog = _log.Skip(Mathf.Max(0, _log.Count - _actionLimit)).ToList();
        string log = string.Join("\n", tempLog.ToArray());
        _logText.text = log;
    }
    
    public void AddNote(string commit)
    {
        _log.Add(commit);
        UpdateLog();
    }

    public void RewriteLog(List<string> previousLog)
    {
        _log = new List<string>(previousLog);
        UpdateLog();
    }
    
    public void Clear()
    {
        _log.Clear();
        UpdateLog();
    }

    public ActionLogSnapshot TakeSnapshot()
    {
        return new ActionLogSnapshot(this);
    }
}
