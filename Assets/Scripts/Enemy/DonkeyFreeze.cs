using UnityEngine;

public class DonkeyFreeze : Enemy
{
    [SerializeField] private GameObject FreezeExplosionPF;


    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        base.SetDamage(damage, damagePos, impulseScale);
        playModeCS.StartCoroutine(playModeCS.FreezeSpawnIE());
        Instantiate(FreezeExplosionPF, transform.position, Quaternion.identity);
        Death();
    }
}
