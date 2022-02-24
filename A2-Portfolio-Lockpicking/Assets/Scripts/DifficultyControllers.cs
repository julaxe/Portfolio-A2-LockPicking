using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyControllers : MonoBehaviour
{
    public PickController pickController;

    public void OnEasyPressed()
    {
        pickController.SetDifficulty(PickController.GameDifficulty.Easy);
    }

    public void OnNormalPressed()
    {
        pickController.SetDifficulty(PickController.GameDifficulty.Normal);
    }

    public void OnHardPressed()
    {
        pickController.SetDifficulty(PickController.GameDifficulty.Hard);
        
    }
}
