using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _roleName;
    [SerializeField] private TMP_Text _playerName;


    [SerializeField] private Button _showActionMenuButton;
    [SerializeField] private Menu _actionMenu;
    
    
    [SerializeField] private Button _kickButton;
    [SerializeField] private Button _reviveButton;
    [SerializeField] private Button _logButton;
    [SerializeField] private Button _cardButton;

    private bool _menuHidden = true;
    
    private Player _player;
    private PlayerLogMenu _playerLog;
    private DealtCardView _dealtCardView;

    public UnityAction<PlayerView> ActionShowed;

    private void OnEnable()
    {
        _actionMenu.Hide();
        _menuHidden = true;
        _kickButton.onClick.AddListener(Kick);
        _reviveButton.onClick.AddListener(Revive);
        _logButton.onClick.AddListener(ShowLog);
        _cardButton.onClick.AddListener(ShowCard);
        _showActionMenuButton.onClick.AddListener(ActionMenuButtonClick);
    }
    
    private void OnDisable()
    {
        _kickButton.onClick.RemoveListener(Kick);
        _reviveButton.onClick.RemoveListener(Revive);
        _logButton.onClick.RemoveListener(ShowLog);
        _showActionMenuButton.onClick.RemoveListener(ActionMenuButtonClick);
    }

    private void ShowCard()
    {
        HideActions();
        _dealtCardView.Show(_player.Card);
    }
    private void ActionMenuButtonClick()
    {
        if (_menuHidden)
        {
            ActionShowed?.Invoke(this);
            _actionMenu.Show();
        }
        else
            _actionMenu.Hide();

        Debug.Log("Clicked");
        _menuHidden = !_menuHidden;
    }

    private void Kick()
    {
        HideActions();
        _player.Kick();
        Actualize();
    }

    private void Revive()
    {
        HideActions();
        _player.Revive();
        Actualize();
    }

    private void ShowLog()
    {
        HideActions();
        _playerLog.Show(_player);
        Actualize();
    }
    
    public void Initialize(Player player, PlayerLogMenu playerLog, DealtCardView cardView)
    {
        _player = player;
        _playerLog = playerLog;
        _dealtCardView = cardView;
        Actualize();
    }

    public void Actualize()
    {
        if (_player.IsAlive)
        {
            _kickButton.gameObject.SetActive(true);
            _reviveButton.gameObject.SetActive(false);
        }
        else
        {
            _kickButton.gameObject.SetActive(false);
            _reviveButton.gameObject.SetActive(true);
        }

        _roleName.text = _player.Card.RoleName;
        _playerName.text = _player.Name;

    }
    
    public void HideActions()
    {
        _menuHidden = true;
        _actionMenu.Hide();
    }

    
}
