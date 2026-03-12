using UnityEngine;

[ExecuteAlways]
public class BlockedCellSpawner : MonoBehaviour
{
    public BlockedCell stonePrefab;
    public BlockedCell woodPrefab;
    public DungeonGrid grid;

    [ContextMenu("Generate")]
    private void Generate()
    {
        var size = grid.CellSize;

        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var prefab = Random.value > 0.25f ? stonePrefab : woodPrefab;
                
                var pos = new Vector3(x * size + size * 0.5f, 0, y * size + size * 0.5f);
                
                Instantiate(prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}