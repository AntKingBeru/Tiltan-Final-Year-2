using UnityEngine;
using TMPro;

public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TMP_Text stoneText;
    [SerializeField] private TMP_Text woodText;

    private void Update()
    {
        stoneText.text = $"Stone: {ResourceManager.Instance.GetStone()}";
        woodText.text = $"Wood: {ResourceManager.Instance.GetWood()}";
    }
}