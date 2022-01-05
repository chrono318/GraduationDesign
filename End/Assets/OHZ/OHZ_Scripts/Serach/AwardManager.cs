using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardManager : MonoBehaviour
{
    public static AwardManager instance;

    public List<GameObject> awardLevelThree;
    public List<GameObject> awardLevelTwo;
    public List<GameObject> awardLevelOne;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance);
        instance = this;
    }
}
