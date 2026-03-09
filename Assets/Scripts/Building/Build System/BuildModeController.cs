using UnityEngine;
using System;

public class BuildModeController : MonoBehaviour
{
    public static event Action<bool> OnBuildModeChanged;

	public bool IsBuildMode {get; private set;}

	public void ToggleBuildMode()
	{
		IsBuildMode = !IsBuildMode;
		OnBuildModeChanged?.Invoke(IsBuildMode);
	}
}