using UnityEngine;
using System.Collections;

public class WalkerCentraliser : MonoBehaviour
{
    public delegate void UpdateValue(int value);
    public static event UpdateValue OnValueUpdated;
    public static int crossRoads;
    public static int dungeonSize;

    public static void AddCrossRoads()
    {
        crossRoads++;
        if(OnValueUpdated != null)
        {
            OnValueUpdated(CalculateFitness());
        }
    }

    public static void AddDungeonSize()
    {
        dungeonSize++;
        if (OnValueUpdated != null)
        {
            OnValueUpdated(CalculateFitness());
        }
    }

    public static int CalculateFitness()
    {
        return crossRoads + dungeonSize;
    }
}
