using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GateOpen : MonoBehaviour
{
    private OffMeshLink OffMeshLink;
    private byte Time=0;
    private bool Activate
    {
        get { return OffMeshLink.activated; }
        set
        {
            switch (value)
            {
                case true:
                    StartCoroutine(Open());
                    break;
                case false:
                    StopAllCoroutines();
                    break;
            }
        }
    }
    private void OnEnable()
    {
        OffMeshLink = GetComponent<OffMeshLink>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(Activate);
        Activate = true;
    }
    public IEnumerator Open()
    {
        while (!Activate)
        {
            Time++;
            if (Time == 2)
            {
                Debug.Log("GateOpen!");
                OffMeshLink.activated = true;
                yield return null;
            }
            Debug.Log(Time);
            yield return new WaitForSeconds(1f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Activate = false;
        Time = 0;
    }
}
