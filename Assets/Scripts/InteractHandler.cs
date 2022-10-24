using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractHandler : MonoBehaviour
{
    private XRGrabInteractable grable;
    private GameLogic game;
    private bool removeItself;
    public bool test = false;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.TryGetComponent<XRGrabInteractable>(out grable))
        {
            grable.onSelectEnter.AddListener(this.EnterSelect);
            grable.onSelectExit.AddListener(this.ExitSelect);
        }
        var temp = GameObject.Find("XR Rig");
        if (temp != null)
        {
            temp.TryGetComponent<GameLogic>(out game);
        }
        removeItself = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(removeItself)
        {
            removeItself = false;
            Destroy(gameObject);
        }

        // just test
        if (test)
        {
            test = false;
            EnterSelect(null);
        }
    }

    public void EnterSelect(XRBaseInteractor iter)
    {
        if (gameObject.CompareTag("Environment"))
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 1000;
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, Mathf.Infinity);
            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("Ground"))
                    {
                        Vector3 bornPos = new Vector3(transform.position.x, hits[i].point.y, transform.position.z);
                        removeItself = game.OnAddBuilding(bornPos);
                        break;
                    }
                }
            }
        }
        else if (gameObject.CompareTag("Buildings"))
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 1000;
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, Mathf.Infinity);
            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("Ground"))
                    {
                        Vector3 bornPos = new Vector3(transform.position.x, hits[i].point.y, transform.position.z);
                        game.OnAddEnvironment(bornPos);
                        removeItself = true;
                        break;
                    }
                }
            }
        }
    }

    public void ExitSelect(XRBaseInteractor iter)
    {

    }
}
