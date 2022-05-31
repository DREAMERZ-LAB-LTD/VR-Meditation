using UnityEngine;

public class Damager : MonoBehaviour
{
    public enum DamagerType
    {
        Continuous,
        Discreate
    }

    [SerializeField] private int damageRate = 5;
    [SerializeField] private DamagerType type = DamagerType.Continuous;


    private void ApplyDamage(GameObject other)
    {
        switch (type)
        {
            case DamagerType.Continuous:
                MakeDamage(other);
                break;
            case DamagerType.Discreate:
                MakeDeath(other);
                break;
        }
    }

    private void MakeDamage(GameObject other)
    { 
        var health = other.GetComponent<Health>();
        if (health)
            health.TakeDamage(damageRate, gameObject);
    }

    private void MakeDeath(GameObject other)
    {
        var health = other.GetComponent<Health>();
        if (health)
            health.ForceDeath(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        ApplyDamage(other.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        ApplyDamage(other.gameObject);
    }
}
