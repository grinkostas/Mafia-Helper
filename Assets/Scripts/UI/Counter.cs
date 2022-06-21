using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : Menu
{
    [SerializeField] private Button _plusButton;
    [SerializeField] private Button _minusButton;
    [SerializeField] private TMP_Text _countText;
    [SerializeField] private int _startValue = 1;

    private int _count;
   
    public int Count
    {
        get => _count;
        set
        {
            if(value < 0)
                return;
            
            _count = value;
            _countText.text = _count.ToString();
        }
    }

    private void Awake()
    {
        Zeroing();
    }

    private void OnEnable()
    {
        _plusButton.onClick.AddListener(Plus);
        _minusButton.onClick.AddListener(Minus);
    }
    
    private void OnDisable()
    {
        _plusButton.onClick.RemoveListener(Plus);
        _minusButton.onClick.RemoveListener(Minus);
    }
    
    private void ChangeCount(int delta)
    {
        Count += delta;
    }
    
    private void Plus()
    {
        ChangeCount(1);
    }

    private void Minus()
    {
        ChangeCount(-1);
    }
    
    private void Zeroing()
    {
        Count = _startValue;
    }
}
