using UnityEngine;

public class EnemyCorpse : MonoBehaviour, IInteractable
{
    private PlayerController _player;
    private bool _isCarried;

    public void SetPlayer(PlayerController player)
    {
        _player = player;
    }
    
    public bool CanInteract(PlayerController player)
    {
        return !_isCarried && player && !player.HasCorpse;
    }

    public void Interact(PlayerController player)
    {
        if (!CanInteract(player))
            return;

        if (_player.TryPickUpCorpse(this))
        {
            _isCarried = true;
        }
    }

    public void AttachTo(Transform anchor)
    {
        transform.SetParent(anchor);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        
        if (TryGetComponent(out Rigidbody rb))
            rb.isKinematic = true;

        if (TryGetComponent(out Collider col))
            col.enabled = false;
    }

    public void Detach(Vector3 worldPosition)
    {
        transform.SetParent(null);
        transform.position = worldPosition;
        
        if (TryGetComponent(out Rigidbody rb))
            rb.isKinematic = false;

        if (TryGetComponent(out Collider col))
            col.enabled = true;
    }
}