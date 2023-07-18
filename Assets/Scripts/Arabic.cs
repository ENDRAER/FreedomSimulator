using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Arabic : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private Rigidbody RB;
    [NonSerialized] public float health = 1000;
    [NonSerialized] public float ImpulseScale = 40f;


    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    void SetRandomDestination()
    {
        navMeshAgent.SetDestination(new Vector3(UnityEngine.Random.Range(-20,20),0, UnityEngine.Random.Range(-20, 20)));
    }


    public void Damage(float damage, Vector3 damagePos)
    {
        health -= damage;
        if (health <= 0)
            Destroy(gameObject);
        else
            RB.AddForce((transform.position - damagePos).normalized * Mathf.Lerp(0f, ImpulseScale, 1f / Vector3.Distance(transform.position, damagePos)), ForceMode.Impulse);
    }
}
