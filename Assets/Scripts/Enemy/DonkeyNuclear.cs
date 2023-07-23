using UnityEngine;

public class DonkeyNuclear : Enemy
{
    [SerializeField] private GameObject NuclearExplosionPF;


    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        base.SetDamage(damage, damagePos, impulseScale);
        Instantiate(NuclearExplosionPF, transform.position, Quaternion.identity);
        playModeCS.ScoreUpdater(playModeCS.EnemiesOnArea.Count);
        playModeCS.EnemiesCounter.transform.localEulerAngles = new Vector3(0, -90, 0);
        playModeCS.EnemiesOnArea.ForEach(enemies => Destroy(enemies));
        playModeCS.EnemiesOnArea.Clear();
    }
}
