using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI sliderText;

    private Health health;

    private void SetHealth(Health health)
    {
        slider.maxValue = health.MaxHealth;
        slider.value = health.CurrentHealth >= 0 ? health.CurrentHealth : 0;
        sliderText.text = health.CurrentHealth >= 0 ? health.CurrentHealth.ToString() : "0";
    }

    public void Bind(Health health)
    {
        if (this.health != null)
        {
            health.OnHealthChange -= SetHealth;
        }

        this.health = health;

        if (this.health != null)
        {
            health.OnHealthChange += SetHealth;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
        {
            health.OnHealthChange -= SetHealth;
        }
    }

}
