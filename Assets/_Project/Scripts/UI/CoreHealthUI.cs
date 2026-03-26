using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CoreHealthUI : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private Core _core;

    public void Bind(Core core)
    {
        _core = core;

        slider.value = core.HealthPercent;

        core.OnHealthChanged += UpdateHealth;
    }

    private void OnDestroy()
    {
        if (_core)
            _core.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(float value)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothUpdate(value));
    }

    private IEnumerator SmoothUpdate(float target)
    {
        var start = slider.value;
        var time = 0f;

        while (time < 0.2f)
        {
            time += Time.deltaTime;
            slider.value = Mathf.Lerp(start, target, time / 0.2f);
            yield return null;
        }
        
        slider.value = target;
    }
}