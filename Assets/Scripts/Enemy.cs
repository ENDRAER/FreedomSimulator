using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] public enum _EnemyType { Arabic, NuclearDonkey };
    [SerializeField] public _EnemyType EnemyType;
    [NonSerialized] public PlayMode playModeCS;
    [SerializeField] private NavMeshAgent goNavMeshAgent;
    [SerializeField] private Rigidbody goRB;
    [SerializeField] private MeshRenderer HatMeshRenderer;
    [NonSerialized] public float maxHealth;
    [SerializeField] public float health;
    [SerializeField] public float impulseScale;


    void Start()
    {
        playModeCS = PlayMode.MainBridge;
        maxHealth = health;
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
        switch (EnemyType)
        {
            case _EnemyType.Arabic:
                health -= damage;
                if (health <= 0)
                {
                    playModeCS.EnemiesOnArea.Remove(gameObject);
                    Destroy(gameObject);
                    return;
                }
                HatMeshRenderer.material.color = Color.Lerp(Color.black, Color.red, 1f / maxHealth * health);
                goRB.AddForce((transform.position - damagePos).normalized * Mathf.Lerp(0f, impulseScale, 1f / Vector3.Distance(transform.position, damagePos)), ForceMode.Impulse);
                SetRandomDestination();
                break;
            case _EnemyType.NuclearDonkey:
                playModeCS.EnemiesOnArea.ForEach(enemies => Destroy(enemies));
                playModeCS.EnemiesOnArea.Clear();
                break;
        }
    }
}
