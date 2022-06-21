using UnityEngine;

public class GameMenu : Menu
{
    [SerializeField] private ActionLog _actionLog;
    public override void Show()
    {
        base.Show();
        _actionLog.Clear();
    }
}
