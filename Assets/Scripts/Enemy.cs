using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private enum _EnemyType { Arabic, NuclearDonkey, FreezeDonkey };
    [SerializeField] private _EnemyType EnemyType;
    [NonSerialized] private PlayMode playModeCS;
    [SerializeField] private GameObject NuclearExplosionPF;
    [SerializeField] private GameObject FreezeExplosionPF;
    [SerializeField] private NavMeshAgent goNavMeshAgent;
    [SerializeField] private Rigidbody goRB;
    [SerializeField] private MeshRenderer HatMeshRenderer;
    [NonSerialized] private float maxHealth;
    [SerializeField] public float health;
    [SerializeField] private float impulseScale; 


    void Start()
    {
        playModeCS = PlayMode.MainBridge;
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

    private void SetRandomDestination()
    {
        goNavMeshAgent.SetDestination(new Vector3(UnityEngine.Random.Range(-20, 20), 0, UnityEngine.Random.Range(-20, 20)));
    }

    public void Damage(float damage, Vector3 damagePos)
    {
        switch (EnemyType)
        {
            case _EnemyType.Arabic:
                health -= damage;
                if (health <= 0)
                {
                    playModeCS.ScoreUpdater(1);
                    playModeCS.EnemiesOnArea.Remove(gameObject);
                    playModeCS.EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90 + (18 * playModeCS.EnemiesOnArea.Count), 0);
                    Destroy(gameObject);
                }
                else
                {
                    HatMeshRenderer.material.color = Color.Lerp(Color.black, Color.red, 1f / maxHealth * health);
                    goRB.AddForce((transform.position - damagePos).normalized * Mathf.Lerp(0f, impulseScale, 1f / Vector3.Distance(transform.position, damagePos)), ForceMode.Impulse);
                    SetRandomDestination();
                }
                break;
            case _EnemyType.NuclearDonkey:
                playModeCS.ScoreUpdater(playModeCS.EnemiesOnArea.Count);
                playModeCS.EnemiesOnArea.ForEach(enemies => Destroy(enemies));
                playModeCS.EnemiesOnArea.Clear();
                playModeCS.EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90 + (18 * playModeCS.EnemiesOnArea.Count), 0);
                Instantiate(NuclearExplosionPF, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
            case _EnemyType.FreezeDonkey:
                playModeCS.FreezeStarter();
                Instantiate(FreezeExplosionPF, transform.position, Quaternion.identity);
                Destroy(gameObject);
                break;
        }
    }
}
