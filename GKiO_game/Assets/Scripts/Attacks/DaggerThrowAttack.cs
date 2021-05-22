using UnityEngine;

/// <summary>
/// The script for skeletons performing attacks with a dagger.
/// The script instatiates the dagger at correct time a distance from the skeleton
/// so as to match the skeleton's throwing animation.
/// The motion of the dagger itself is controlled by the DaggerController script.
/// </summary>
public class DaggerThrowAttack : WeaponAttack
{
    /// <summary>
    /// Speed with which the dagger is thrown.
    /// </summary>
    public float throwSpeed = 6f;
    /// <summary>
    /// Health damage caused by the dagger.
    /// </summary>
    public float damage = 30;
    /// <summary>
    /// Angular velocity of the dagger.
    /// </summary>
    public float rotationSpeed = 6f;
    /// <summary>
    /// The layer where objects to hit are placed
    /// </summary>
    public int layerToHit = 6;
    /// <summary>
    /// Time when the dagger is released after Attack call.
    /// </summary>
    public float daggerReleaseTime = 0.5f;
    /// <summary>
    /// Time when another dagger appears in skeleton's hand after Attack call.
    /// </summary>
    public float daggerReplaceTime = 1.5f;
    public GameObject dagger;
    private GameObject releasedDagger;
    private bool isAttacking = false;

    void Start()
    {
        ConfigureDagger();
    }

    public override void Attack()
    {
        if (!isAttacking)
        {
            ConfigureTimes();
            isAttacking = true;
            Invoke("ReleaseDagger", daggerReleaseTime);
            Invoke("ReplaceDagger", daggerReplaceTime);
        }
    }

    public override bool IsAimInAttackRange()
    {
        return true;
    }

    private void ConfigureDagger()
    {
        DaggerController daggerController = dagger.GetComponent<DaggerController>();
        daggerController.initialSpeed = throwSpeed;
        daggerController.rotationSpeed = rotationSpeed;
        daggerController.damage = damage;
        daggerController.layerToHit = layerToHit;
    }

    private void ConfigureTimes()
    {
        var adapter = GetComponent<SkeletonDaggerAnimationAdapter>();
        daggerReleaseTime = adapter.ActualReleaseTime;
        daggerReplaceTime = adapter.ActualReplaceTime;
    }

    private void PrepareReleasedDagger()
    {
        Collider collider = GetComponent<Collider>();
        float offsetX = 0.22f; //distance in x-axis from collider
        float daggerPositionX = collider.bounds.center.x + transform.forward.x * (collider.bounds.extents.x + offsetX);
        Vector3 daggerPosition = new Vector3(daggerPositionX, collider.bounds.max.y, collider.bounds.center.z);
        releasedDagger = Instantiate(dagger, dagger.transform.parent);
        releasedDagger.transform.SetParent(null);
        releasedDagger.transform.position = daggerPosition;
        releasedDagger.transform.rotation = Quaternion.FromToRotation(releasedDagger.transform.up, Vector3.back);
        Physics.IgnoreCollision(releasedDagger.GetComponent<Collider>(), GetComponent<Collider>());
        dagger.SetActive(false);
    }

    private void ReleaseDagger()
    {
        PrepareReleasedDagger();
        DaggerController dc = releasedDagger.GetComponent<DaggerController>();
        Vector3 aimPosition = Aim.GetComponent<Collider>().bounds.center;
        dc.Throw(aimPosition);
    }

    private void ReplaceDagger()
    {
        dagger.SetActive(true);
        isAttacking = false;
    }

    
}
