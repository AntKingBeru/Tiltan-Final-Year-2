using UnityEngine;

public class Room : MonoBehaviour
{
    public Vector2Int Origin { get; private set; }
    public Vector2Int Size { get; private set; }

    public bool BlocksEnemies { get; private set; }

    public void Initialize(Vector2Int origin, Vector2Int size, bool blocksEnemies)
    {
        Origin = origin;
        Size = size;
        BlocksEnemies = blocksEnemies;
    }
}