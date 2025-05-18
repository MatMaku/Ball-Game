using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public int level = 1;
    public int experience = 0;
    public int expToNextLevel = 5;

    public Slider expSlider;

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
        expToNextLevel += 5;
        Debug.Log("¡Subiste a nivel " + level + "!");
    }
}
