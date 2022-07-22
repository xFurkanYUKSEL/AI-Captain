// AI Script
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public enum AISTATE { PATROL, CHASE, ATTACK };

    private NavMeshAgent ThisAgent;
    private Transform Target;
    private byte AttackTime;
    internal AISTATE CurrentState
    {
        get { return _CurrentState; }
        set
        {
            StopAllCoroutines();
            _CurrentState = value;

            switch (CurrentState)
            {
                case AISTATE.PATROL:
                    StartCoroutine(StatePatrol());
                    break;

                case AISTATE.CHASE:
                    StartCoroutine(StateChase());
                    break;

                case AISTATE.ATTACK:
                    StartCoroutine(StateAttack());
                    break;
            }
        }
    }
    private AISTATE _CurrentState;

    private void Awake()
    {
        ThisAgent = GetComponent<NavMeshAgent>();
        Target = GameObject.Find(gameObject.name + "Target").GetComponent<Transform>();
    }

    private void Start()
    {
        CurrentState = AISTATE.PATROL;
    }

    public IEnumerator StateChase()
    {
        float AttackDistance = 2f;

        while (CurrentState == AISTATE.CHASE)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackDistance)
            {
                CurrentState = AISTATE.ATTACK;
                yield return null;
            }


            ThisAgent.SetDestination(Target.transform.position);
            yield return null;
        }
    }

    public IEnumerator StateAttack()
    {
        float AttackDistance = 2f;

        while (CurrentState == AISTATE.ATTACK)
        {
            print("Attack!" + Target.gameObject.name);
            AttackTime++;
            if (AttackTime == 5)
            {
                Destroy(Target.gameObject);
                CurrentState = AISTATE.PATROL;
                yield return null;
            }
            if (Vector3.Distance(transform.position, Target.transform.position) > AttackDistance)
            {
                CurrentState = AISTATE.CHASE;
                AttackTime = 0;
                yield return null;
            }
            ThisAgent.SetDestination(Target.transform.position);
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator StatePatrol()
    {
        GameObject[] Waypoints = GameObject.FindGameObjectsWithTag(gameObject.name + "Waypoint");
        GameObject CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
        float TargetDistance = 2f;

        while (CurrentState == AISTATE.PATROL)
        {
            ThisAgent.SetDestination(CurrentWaypoint.transform.position);
            Debug.Log(gameObject.name + " " + CurrentWaypoint.transform.position);
            if (Vector3.Distance(transform.position, CurrentWaypoint.transform.position) < TargetDistance)
            {
                CurrentWaypoint = Waypoints[Random.Range(0, Waypoints.Length)];
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Target == null) return;
        if (other.gameObject == Target.gameObject)
        {
            CurrentState = AISTATE.CHASE;
            Debug.Log(gameObject.name + " Trigger Enter!");
            Attack attack = GetComponent<Attack>();
            if (attack != null)
            {
                attack.enabled = true;
            }
        }

    }
}