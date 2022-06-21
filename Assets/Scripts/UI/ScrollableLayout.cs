using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public class ScrollableLayout : MonoBehaviour
{
    private VerticalLayoutGroup _verticalLayout;

    private VerticalLayoutGroup VerticalLayout  
    {
        get
        {
            if (_verticalLayout == null)
                _verticalLayout = GetComponent<VerticalLayoutGroup>();
            return _verticalLayout;
        }
    }
    
    private void UpdateSize()
    {
        float size = 0;
        foreach (Transform children in transform)
        {
            var rect = children.GetComponent<RectTransform>();
            size += rect.sizeDelta.y + VerticalLayout.spacing;
        }
        
        var rectTransform = VerticalLayout.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, size);
        
    }

    private void OnTransformChildrenChanged()
    {
        UpdateSize();
    }
}
