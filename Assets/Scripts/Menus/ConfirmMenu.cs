using System;
using TMPro;
using UnityEngine;

public class ConfirmMenu : Menu<Action>
{
    [SerializeField] private TMP_Text _questionText;

    private const string DefaultConfirmText = "Are you sure?";
    
    private Action _currentAction;

    private void Start()
    {
        _questionText.text = DefaultConfirmText;
    }

    public override void Show(Action action)
    {
        _currentAction = action;
        base.Show(action);
    }

    public void Yes()
    {
        if(_currentAction != null)
            _currentAction();
        Hide();
    }

    public void No()
    {
        Hide();
    }
}
