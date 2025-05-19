using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public int level = 1;
    public int experience = 0;
    public int expToNextLevel = 4;

    public Slider expSlider;
    public UpgradeManager upgradeManager;

    void Start()
    {
        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = experience;
        }
    }

    public void AddExperience(int amount)
    {
        experience += amount;

        while (experience >= expToNextLevel)
        {
            experience -= expToNextLevel;
            LevelUp();
            upgradeManager.MostrarOpcionesMejora(level);
            Time.timeScale = 0;
        }

        if (expSlider != null)
        {
            expSlider.maxValue = expToNextLevel;
            expSlider.value = experience;
        }
    }

    private void LevelUp()
    {
        level++;
        expToNextLevel += 2;
        Debug.Log("¡Subiste a nivel " + level + "!");
    }
}
