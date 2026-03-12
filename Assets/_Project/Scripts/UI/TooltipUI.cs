using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }
    
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text descriptionText;
    
    private RectTransform _rect;

    private void Awake()
    {
        Instance = this;
        
        _rect = GetComponent<RectTransform>();
        Hide();
    }

    private void Update()
    {
        _rect.position = Input.mousePosition;
    }

    public void Show(string title, string description)
    {
        gameObject.SetActive(true);
        
        titleText.text = title;
        descriptionText.text = description;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}