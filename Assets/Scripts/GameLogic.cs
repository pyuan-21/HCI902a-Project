using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private GameObject buildingRoot;
    private List<string> buildingNames;

    public float environmentValue;
    public float economyValue;

    // TODO: change these two progressbar
    //public ProgressB

    // Start is called before the first frame update
    void Start()
    {
        buildingNames = new List<string>();
        buildingNames.Add("food_factory");
        buildingNames.Add("industrial_factory");
        buildingRoot = new GameObject("Buildings");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnAddBuilding(Vector3 pos)
    {
        int randomIndex = UnityEngine.Random.Range(0, buildingNames.Count - 1);
        string buildingName = buildingNames[randomIndex];
        GameObject prefab = Resources.Load<GameObject>("BuildingPrefabs/" + buildingName);
        GameObject prefabInstance = Instantiate<GameObject>(prefab);
        prefabInstance.transform.SetParent(buildingRoot.transform);
        prefabInstance.transform.position = pos;
        prefabInstance.transform.rotation = Quaternion.identity;
        prefabInstance.transform.localScale = Vector3.one;

        economyValue += 5;
        economyValue = Mathf.Clamp(economyValue, 0, 100);

        environmentValue -= 5;
        environmentValue = Mathf.Clamp(environmentValue, 0, 100);
    }

    public void OnAddEnvironment(Vector3 pos)
    {

    }
}
