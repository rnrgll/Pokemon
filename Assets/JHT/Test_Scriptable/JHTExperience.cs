using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JHTExperience : MonoBehaviour
{
    [SerializeField] int experiencePoints = 0;

    public void GetExperience(int experience)
    {
        experiencePoints += experience;
    }

    public int GetExp()
    {
        return experiencePoints;
    }
}
