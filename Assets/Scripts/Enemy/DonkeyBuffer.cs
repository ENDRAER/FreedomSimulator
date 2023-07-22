using UnityEngine;

public class DonkeyBuffer : Enemy
{
    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
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
        playModeCS.UpgradeCallerAnimator.Rebind();
        Destroy(gameObject);
        Destroy(gameObject);
    }
}
