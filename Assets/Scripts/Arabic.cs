using System;
using UnityEngine;
using UnityEngine.AI;

public class Arabic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent goNavMeshAgent;
    [SerializeField] private Rigidbody goRB;
    [SerializeField] public float health;
    [SerializeField] public float impulseScale;


    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        if (!goNavMeshAgent.pathPending && goNavMeshAgent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        goNavMeshAgent.SetDestination(new Vector3(UnityEngine.Random.Range(-20,20),0, UnityEngine.Random.Range(-20, 20)));
    }

    public void Damage(float damage, Vector3 damagePos)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }
        goRB.AddForce((transform.position - damagePos).normalized * Mathf.Lerp(0f, impulseScale, 1f / Vector3.Distance(transform.position, damagePos)), ForceMode.Impulse);
        SetRandomDestination();
    }
}
