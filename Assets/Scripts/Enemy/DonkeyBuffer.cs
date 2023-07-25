using UnityEngine;

public class DonkeyBuffer : Enemy
{
    [SerializeField] protected GameObject powerUpAU;


    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        base.SetDamage(damage, damagePos, impulseScale);
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            playModeCS.UpgradeCallerText.text = "increased damage";
            playModeCS.Damage *= 2;
        }
        else
        {
            playModeCS.UpgradeCallerText.text = "reload time reduced";
            playModeCS.MaxReloadTime /= 2;
        }
        playModeCS.UpgradeCallerAnimator.SetTrigger("Call");
        Instantiate(powerUpAU, transform.position, Quaternion.identity);
        Death();
    }
}
