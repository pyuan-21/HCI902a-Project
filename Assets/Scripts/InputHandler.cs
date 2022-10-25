using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class InputHandler : MonoBehaviour
{
    public GameObject xrRig;
    public float heightOffset = 0.2f;
    public float moveSpeed = 0.1f;
    public bool isFrenchKeyboard = false;

    private float m_AxisToPressThreshold = 0.01f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckTouchPadInput();
    }

    void CheckTouchPadInput()
    {
        bool touchPadClick = false;
        Vector2 clickValue = Vector2.zero;

        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);

        foreach (var device in inputDevices)
        {
            if (device.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxis, out clickValue) && clickValue.magnitude >= m_AxisToPressThreshold)
            {
                //StartMove(position);
                touchPadClick = true;
                break;
            }
        }

        // keyboard test
        List<string> keys = new List<string>();
        if(isFrenchKeyboard)
        {
            keys.Add("z"); keys.Add("s"); keys.Add("q"); keys.Add("d");
        }
        else
        {
            keys.Add("w"); keys.Add("s"); keys.Add("a"); keys.Add("d");
        }
        Vector2[] delta = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        for(int i = 0; i < keys.Count; i++)
        {
            if (Input.GetKey(keys[i]))
            {
                clickValue += delta[i];
                touchPadClick = true;
            }
        }


        if (touchPadClick)
        {
            // try move xrRig
            Vector3 rayOrigin = xrRig.transform.position + Vector3.up * 1000;
            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, Vector3.down, Mathf.Infinity);
            if (hits!=null&&hits.Length>0)
            {
                for(int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.CompareTag("Ground"))
                    {
                        //Debug.Log("Hit at " + hits[i].point.y);
                        // clickValue x:[-1,1] from left to right; y:[-1,1] from bottom to top
                        // try to move xrRig
                        Quaternion rot = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
                        Vector3 temp1 = new Vector3(clickValue.x, 0, clickValue.y); // rot only works with 3D
                        Vector3 temp2 = rot * temp1; // rotate it
                        temp2 = temp2.normalized; // direction
                        Vector2 moveDir = new Vector2(temp2.x, temp2.z); // project on x-z plane
                        //Debug.Log("moveDir: " + moveDir);
                        Vector3 temp3 = Vector3.ProjectOnPlane(xrRig.transform.position, Vector3.up);
                        Vector2 posGround = new Vector2(temp3.x, temp3.z);
                        Vector2 newPosGround = posGround + moveDir * moveSpeed;
                        Vector3 newPos = new Vector3(newPosGround.x, hits[i].point.y + heightOffset, newPosGround.y);
                        //Debug.Log("---------------------------");
                        //Debug.Log("oldPos: " + xrRig.transform.position);
                        //Debug.Log("newPos: " + newPos);
                        //Debug.Log("---------------------------");
                        xrRig.transform.position = newPos;
                        break;
                    }
                }
            }
        }
    }
}