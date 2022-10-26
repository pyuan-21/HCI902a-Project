using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GameLogic : MonoBehaviour
{
    private GameObject buildingRoot;
    private List<string> buildingNames;

    private GameObject environmentRoot;
    private List<string> environmentNames;

    public float environmentValue;
    public float economyValue;

    public XRInteractionManager xrMgr;

    public ProgressBar envBar;
    public ProgressBar ecoBar;
    private bool needUpdate;


    //water height 
    public float wHeightMin = -0.77f;
    public float wHeightMax = -1.24f;
    public Transform waterTrans;

    // each time create/destroy a building will use this value to change the economy/environment value
    public float changeValue = 5;

    
    void Start()
    {
        buildingNames = new List<string>();
        buildingNames.Add("food_factory");
        buildingNames.Add("industrial_factory");
        buildingNames.Add("electronic_store");
        buildingNames.Add("house_1");
        buildingNames.Add("mine");
        buildingNames.Add("pharmacy");
        buildingRoot = new GameObject("Buildings");

        environmentNames = new List<string>();
        environmentNames.Add("Tree1");
        environmentNames.Add("Tree2");
        environmentNames.Add("Tree3");
        environmentRoot = new GameObject("Environment");

        environmentValue = 100;
        economyValue = 0;
        needUpdate = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnvironment();
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
        prefabInstance.transform.localScale = Vector3.one * 6;
        prefabInstance.tag = "Buildings";
        prefabInstance.GetComponent<XRSimpleInteractable>().interactionManager = xrMgr;

        economyValue += changeValue;
        environmentValue -= changeValue;
        needUpdate = true;

        return true;
    }

    public bool OnAddEnvironment(Vector3 pos)
    {
        if (environmentValue >= 100)
            return false;

        int randomIndex = UnityEngine.Random.Range(0, environmentNames.Count - 1);
        string environmentName = environmentNames[randomIndex];
        GameObject prefab = Resources.Load<GameObject>("EnvironmentPrefabs/" + environmentName);
        GameObject prefabInstance = Instantiate<GameObject>(prefab);
        prefabInstance.transform.SetParent(environmentRoot.transform);
        prefabInstance.transform.position = pos;
        prefabInstance.transform.rotation = Quaternion.identity;
        prefabInstance.transform.localScale = Vector3.one;
        prefabInstance.tag = "Environment";
        prefabInstance.GetComponent<XRSimpleInteractable>().interactionManager = xrMgr;

        economyValue -= changeValue;
        environmentValue += changeValue;
        needUpdate = true;

        return true;
    }

    private void UpdateEnvironment()
    {
        if (!needUpdate)
            return;

        // update progress bar
        envBar.BarValue = environmentValue;
        ecoBar.BarValue = economyValue;

        // update water height
        float newHeight = (1.0f-envBar.BarValue/100f) * (wHeightMax - wHeightMin) + wHeightMin; // get newHeight from range [min, max]
        waterTrans.position = new Vector3(0, newHeight, 0);

        needUpdate = false;
    }
}
