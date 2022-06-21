using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    [SerializeField] private CardDeal _cardDeal;
    [SerializeField] private Transform _playersParent;
    [SerializeField] private Player _playerPrefab;

    [Header("Menus")] 
    [SerializeField] private ConfirmMenu _confirmMenu;
    [SerializeField] private GameMenu _gameMenu;
    [SerializeField] private PlayersMenu _playersMenu;
    [SerializeField] private StartGameMenu _startGameMenu;

    [SerializeField] private Turn _turn;

    private List<Player> _players = new List<Player>();
    
    private int _currentPlayerNumber = 1;

    private void OnEnable()
    {
        _cardDeal.CardDealt += OnCardDealt;
        _cardDeal.DealEnded += OnDealEnded;
    }
    
    private void OnDisable()
    {
        _cardDeal.CardDealt -= OnCardDealt;
        _cardDeal.DealEnded -= OnDealEnded;
    }

    private void Start()
    {
        _startGameMenu.Show();
        _gameMenu.Hide();
        _playersMenu.Hide();
        
    }

    private void OnCardDealt(PlayingCard card)
    {
        
        Player instance = Instantiate(_playerPrefab, _playersParent);
        instance.Initialize(_currentPlayerNumber, card);
        _currentPlayerNumber++;
        _players.Add(instance);
        instance.Idle();
    }

    private void OnDealEnded()
    {
        _turn.Initialize(_players);
        _playersMenu.Initialize(_players);
        
        _gameMenu.Show();
    }
    
    public void StartGame()
    {
        _currentPlayerNumber = 1;
        if (_players.Count > 0)
        {
            foreach (var player in _players)
            {
                if(player != null)
                    Destroy(player.gameObject);
            }
        }
        _cardDeal.StartDeal();
    }

    public void Reload()
    {
        _confirmMenu.Show(()=>SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
    }
}
