using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DealtCardView : Menu<PlayingCard>
{
    [Header("Texts")]
    [SerializeField] private TMP_Text _roleName;
    [SerializeField] private TMP_Text _description;
    
    [Header("FlipAnimation")] 
    [SerializeField] private float _flipDuration;
    [SerializeField] private Image _cardBackSide;
    [SerializeField] private Button[] _buttonsToDisableDuringAnimation;

    private IEnumerator NextCardAnimation(PlayingCard card)
    {
        yield return FlipAnimation();
        Actualize(card);
    }
    
    private IEnumerator FlipAnimation()
    {
        SetActiveButtons(false);
        var eulers = transform.localRotation.eulerAngles;
        var hash = GetHash(eulers.y + 90, _flipDuration/2);
        var hash2 = GetHash(eulers.y + 180, _flipDuration/2);
        
        iTween.RotateTo(gameObject, hash);
        yield return new WaitForSeconds(_flipDuration/2);
        iTween.RotateTo(gameObject, hash2);
        _cardBackSide.gameObject.SetActive(!_cardBackSide.gameObject.activeSelf);
        yield return new WaitForSeconds(_flipDuration/2);
        SetActiveButtons(true);

    }
    
    private Hashtable GetHash(float angle, float time)
    {   
        return iTween.Hash("y", angle, "time", time, "easetype", iTween.EaseType.linear,
            "looptype", iTween.LoopType.none);
    }

    private void SetActiveButtons(bool activeStatus)
    {
        foreach (var button in _buttonsToDisableDuringAnimation)
        {
            button.interactable = activeStatus;
        }
    }
    
    public void Actualize(PlayingCard card)
    {
        _roleName.text = card.RoleName;
        _description.text = card.RoleDescription;
    }
    
    public override void Show(PlayingCard card)
    {
        Actualize(card);
        base.Show(card);
    }
    
    public void NextCard(PlayingCard card)
    {
        StartCoroutine(NextCardAnimation(card));
    }

    public void Flip()
    {
        StartCoroutine(FlipAnimation());
    }
}
