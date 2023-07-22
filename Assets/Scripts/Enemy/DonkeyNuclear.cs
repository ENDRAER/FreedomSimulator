using UnityEngine;

public class DonkeyNuclear : Enemy
{
    [SerializeField] private GameObject NuclearExplosionPF;


    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        base.SetDamage(damage, damagePos, impulseScale);
        playModeCS.ScoreUpdater(playModeCS.EnemiesOnArea.Count);
        playModeCS.EnemiesOnArea.ForEach(enemies => Destroy(enemies));
        playModeCS.EnemiesOnArea.Clear();
        playModeCS.EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90 + (18 * playModeCS.EnemiesOnArea.Count), 0);
        Instantiate(NuclearExplosionPF, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
