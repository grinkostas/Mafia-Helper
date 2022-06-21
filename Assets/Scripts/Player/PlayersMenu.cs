using System.Collections.Generic;
using UnityEngine;

public class PlayersMenu : Menu
{
    [SerializeField] private PlayerView _playerViewPrefab;
    [SerializeField] private Transform _playerViewsParent;
    [SerializeField] private PlayerLogMenu _playerLog;
    [SerializeField] private DealtCardView _dealtCardView;
    private List<Player> _players = new List<Player>();
    private List<PlayerView> _playerViews = new List<PlayerView>();

    private void OnActionShowed(PlayerView view)
    {
        foreach (var playerView in _playerViews)      
        {
            if(playerView != view)
                playerView.HideActions();
        }
    }

    private void OnEnable()
    {
        foreach (var playerView in _playerViews)
        {
            playerView.ActionShowed += OnActionShowed;
        }
    }

    private void OnDisable()
    {
        foreach (var playerView in _playerViews)
        {
            playerView.ActionShowed -= OnActionShowed;
        }
        
    }
    public void Initialize(List<Player> players)
    {
        foreach (var playerView in _playerViews)
        {
            playerView.ActionShowed -= OnActionShowed;
            Destroy(playerView.gameObject);
        }
        _playerViews.Clear();
        _players.Clear();
        
        _players = players;
        foreach (var player in players)
        {
            PlayerView instance = Instantiate(_playerViewPrefab, _playerViewsParent);
            instance.Initialize(player, _playerLog, _dealtCardView);
            instance.Actualize();
            _playerViews.Add(instance);
            instance.ActionShowed += OnActionShowed;
        }
    }

    
    public override void Show()
    {
        foreach(var view in _playerViews)
            view.Actualize();
        base.Show();
    }


    
}
