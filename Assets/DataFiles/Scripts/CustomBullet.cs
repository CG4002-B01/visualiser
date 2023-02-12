using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    // Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;
    
    // Stats 
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    public int explosionDamage;
    public float explosionRange;

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // When should bullet explode
        if (collisions > maxCollisions) Explode();

        // Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    void Setup()
    {
        // Create a new Physic Material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        // Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        // Set gravity
        rb.useGravity = useGravity;
    }

    void Explode()
    {
        // Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        // Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            // Get component of enemy and call take damage
        }

        // Add delay to make sure everything works fine
        Invoke("Delay", 0.05f);
    }

    void onCollisionEnter(Collision collision)
    {
        // Count up collisions
        collisions++;

        // Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch) Explode();
    }

    void Delay()
    {
        Destroy(gameObject);
    }
}
