using UnityEngine;

public class Arabic : Enemy
{
    [SerializeField] private Rigidbody goRB;
    [SerializeField] private MeshRenderer HatMeshRenderer;


    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        base.SetDamage(damage, damagePos, impulseScale);
        if (health > 0)
        {
            HatMeshRenderer.material.color = Color.Lerp(Color.black, Color.red, 1f / maxHealth * health);
            goRB.AddForce((transform.position - damagePos).normalized * Mathf.Lerp(0f, impulseScale, 1f / Vector3.Distance(transform.position, damagePos)), ForceMode.Impulse);
            SetRandomDestination();
        }
        else
            Death();
    }
}
