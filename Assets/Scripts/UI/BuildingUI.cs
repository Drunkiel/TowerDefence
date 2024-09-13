using System.Collections.Generic;
using UnityEngine;

public class BuildingUI : MonoBehaviour
{
    public List<GameObject> allBuildings = new();
    [SerializeField] private Transform parent;
    [SerializeField] private BuildingCard buildingCardPrefab;

    void Start()
    {
        SpawnCards();
    }

    public void SpawnCards()
    {
        foreach (GameObject building in allBuildings)
        {
            BuildingCard _singleCard = Instantiate(buildingCardPrefab, parent);
            _singleCard.AssignData(null, null, () => BuildingSystem.instance.InitializeWithObject(building));
        }
    }
}
