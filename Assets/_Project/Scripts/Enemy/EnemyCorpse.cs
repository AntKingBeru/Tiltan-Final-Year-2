using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    public void ReviveIntoMinion()
    {
        MinionManager.Instance.SpawnMinion(transform.position);
        Destroy(gameObject);
    }
}