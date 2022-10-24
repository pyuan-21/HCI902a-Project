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
    public ProgressBar envBar;
    public ProgressBar ecoBar;
    private bool needUpdate;

    // Start is called before the first frame update
    void Start()
    {
        buildingNames = new List<string>();
        buildingNames.Add("food_factory");
        buildingNames.Add("industrial_factory");
        buildingRoot = new GameObject("Buildings");

        environmentValue = 100;
        economyValue = 0;
        needUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
    }

    public bool OnAddBuilding(Vector3 pos)
    {
        if (environmentValue <= 0)
            return false;

        int randomIndex = UnityEngine.Random.Range(0, buildingNames.Count - 1);
        string buildingName = buildingNames[randomIndex];
        GameObject prefab = Resources.Load<GameObject>("BuildingPrefabs/" + buildingName);
        GameObject prefabInstance = Instantiate<GameObject>(prefab);
        prefabInstance.transform.SetParent(buildingRoot.transform);
        prefabInstance.transform.position = pos;
        prefabInstance.transform.rotation = Quaternion.identity;
        prefabInstance.transform.localScale = Vector3.one;

        economyValue += 5;
        environmentValue -= 5;
        needUpdate = true;

        return true;
    }

    public bool OnAddEnvironment(Vector3 pos)
    {
        // TODO
        return false;
    }

    private void UpdateBar()
    {
        if (!needUpdate)
            return;

        envBar.BarValue = environmentValue;
        ecoBar.BarValue = economyValue;

        needUpdate = false;
    }
}
