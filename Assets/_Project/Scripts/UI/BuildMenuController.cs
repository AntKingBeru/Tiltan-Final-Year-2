using UnityEngine;
using System.Collections.Generic;

public class BuildMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform contentRoot;
    [SerializeField] private BuildButtonUI buttonPrefab;
    
    [Header("Blueprints")]
    [SerializeField] private List<RoomBlueprint> roomBlueprints;
    [SerializeField] private List<TrapBlueprint> trapBlueprints;
    
    [Header("Collapse Seetings")]
    [SerializeField] private RectTransform panel;
    [SerializeField] private float expandedWidth = 360f;
    [SerializeField] private float collapsedWidth = 60f;
    
    private readonly List<BuildButtonUI> _spawnedButtons = new();
    private bool _collapsed;
    
    public void SelectCategoryRooms()
    {
        PopulateRooms();
    }
    
    public void SelectCategoryTraps()
    {
        PopulateTraps();
    }

    private void PopulateRooms()
    {
        ClearButtons();

        foreach (var blueprint in roomBlueprints)
        {
            var btn = Instantiate(buttonPrefab, contentRoot);
            btn.InitializeRoom(blueprint);
            _spawnedButtons.Add(btn);
        }
    }

    private void PopulateTraps()
    {
        ClearButtons();

        foreach (var blueprint in trapBlueprints)
        {
            var btn = Instantiate(buttonPrefab, contentRoot);
            btn.InitializeTrap(blueprint);
            _spawnedButtons.Add(btn);
        }
    }

    private void ClearButtons()
    {
        foreach (var btn in _spawnedButtons) Destroy(btn.gameObject);
        
        _spawnedButtons.Clear();
    }

    public void ToggleMenu()
    {
        _collapsed = !_collapsed;

        panel.SetSizeWithCurrentAnchors(
            RectTransform.Axis.Horizontal,
            _collapsed ? collapsedWidth : expandedWidth
        );
    }
}