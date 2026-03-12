using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MinimapClickHandler : MonoBehaviour
{
    [SerializeField] private Camera minimapCam;
    [SerializeField] private Camera mainCamera;
    
    [SerializeField] private RawImage minimapImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        var rect = minimapImage.rectTransform;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect, 
            eventData.position,
            eventData.pressEventCamera,
            out var localPoint
        );

        var r = rect.rect;
        
        var normalizedX = (localPoint.x - r.x) / r.width;
        var normalizedY = (localPoint.y - r.y) / r.height;

        var ray = minimapCam.ViewportPointToRay(
            new Vector3(normalizedX, normalizedY, 0)
        );
        
        if (Physics.Raycast(ray, out var hit)) MoveMainCamera(hit.point);
    }

    private void MoveMainCamera(Vector3 worldPosition)
    {
        var pos = mainCamera.transform.position;

        var target = new Vector3(
            worldPosition.x,
            pos.y,
            worldPosition.z
        );
        
        mainCamera.transform.position = Vector3.Lerp(pos, target, 0.25f);
    }
}
