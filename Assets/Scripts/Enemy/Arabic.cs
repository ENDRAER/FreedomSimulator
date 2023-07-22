using UnityEngine;

public class Arabic : Enemy
{
    [SerializeField] private Rigidbody goRB;
    [SerializeField] private MeshRenderer HatMeshRenderer;


    public override void SetDamage(float damage, Vector3 damagePos, float impulseScale)
    {
        base.SetDamage(damage, damagePos, impulseScale);
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
    }
}
