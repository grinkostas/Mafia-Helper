using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLogSnapshot : ISnapshot
{
    private ActionLog _actionLog;
    private List<string> _log;

    public ActionLogSnapshot(ActionLog actionLog)
    {
        _actionLog = actionLog;
        _log = new List<string>(actionLog.Log);
    }
    
    public void Rollback()
    {
        _actionLog.RewriteLog(_log);
    }
}
