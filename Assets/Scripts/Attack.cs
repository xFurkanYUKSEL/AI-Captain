using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private GameObject[] Tanks;
    private void OnEnable()
    {
        Tanks = GameObject.FindGameObjectsWithTag("Tank");
        foreach (GameObject tank in Tanks)
        {
            tank.GetComponent<EnemyNav>().CurrentState = EnemyNav.AISTATE.CHASE;
        }
    }
}
