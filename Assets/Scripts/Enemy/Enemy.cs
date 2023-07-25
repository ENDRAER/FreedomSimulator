using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [NonSerialized] protected PlayMode playModeCS;
    [NonSerialized] protected float maxHealth;
    [SerializeField] public float health;
    [SerializeField] protected NavMeshAgent goNavMeshAgent;


    void Start()
    {
        playModeCS = PlayMode.playMode;
        maxHealth = health;
        SetRandomDestination();
    }

    void Update()
    {
        if ( goNavMeshAgent.remainingDistance < 0.5f)
        {
            SetRandomDestination();
        }
    }

    protected void SetRandomDestination()
    {
        goNavMeshAgent.SetDestination(new Vector3(UnityEngine.Random.Range(-20, 20), 0, UnityEngine.Random.Range(-20, 20)));
    }

    public virtual void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        health -= damage;
    }

    protected virtual void Death()
    {
        playModeCS.ScoreUpdater(1);
        playModeCS.EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90 + (18 * playModeCS.EnemiesOnArea.Count - 1), 0);
        playModeCS.EnemiesOnArea.Remove(gameObject);
        Destroy(gameObject);
    }
}
