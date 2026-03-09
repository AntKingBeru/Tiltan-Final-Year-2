using UnityEngine;
using System.Collections.Generic;

public class DungeonGrid : MonoBehaviour
{
    private Dictionary<Vector2Int, GridCell> _cells;

	public bool IsAreaFree(Vector2Int start, Vector2Int size)
	{
		for (var x = 0; x < size.x; x++)
		{
			for (var y = 0; y < size.y; y++)
			{
				var pos = start + new Vector2Int(x, y);

				if (!_cells.ContainsKey(pos) || _cells[pos].occupied) return false;
			}
		}

		return true;
	}
}