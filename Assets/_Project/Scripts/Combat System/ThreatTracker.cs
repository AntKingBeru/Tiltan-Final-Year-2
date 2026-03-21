using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ThreatTracker
{
    private readonly Dictionary<IDamageable, float> _threat = new();

	public void AddThreat(IDamageable target, float amount)
	{
		if (target == null)
			return;

		if (!_threat.TryAdd(target, amount))
			_threat[target] += amount;
    }

	public IDamageable GetHighestThreat()
	{
		var max = 0f;
		IDamageable best = null;

		foreach (var kvp in _threat.Where(
                     kvp => kvp.Key != null).Where(
                     kvp => kvp.Value > max))
        {
            max = kvp.Value;
            best = kvp.Key;
        }

		return best;
	}

	public void DecayThreat(float amountPerSecond)
	{
		var keys = new List<IDamageable>(_threat.Keys);

		foreach (var key in keys)
		{
			_threat[key] -= amountPerSecond * Time.deltaTime;

			if (_threat[key] <= 0)
				_threat.Remove(key);
		}
	}
}