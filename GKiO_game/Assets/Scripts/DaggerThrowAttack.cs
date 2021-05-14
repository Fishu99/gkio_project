using UnityEngine;

public class DaggerThrowAttack : WeaponAttack
{
    public GameObject dagger;
    private GameObject releasedDagger;
    private bool isAttacking = false;
    public float throwSpeed = 6f;
    public float damage = 30;
    public float rotationSpeed = 6f;
    public int layerToHit = 6;
    //Te ostatnie dwie wartoœci s¹ odczytywane z adaptera animacji
    public float daggerReleaseTime = 0.5f;
    public float daggerReplaceTime = 1.5f;
    
    void Start()
    {
        ConfigureDagger();
    }

    
    void Update()
    {
        
    }

    private void ConfigureDagger()
    {
        DaggerController daggerController = dagger.GetComponent<DaggerController>();
        daggerController.initialSpeed = throwSpeed;
        daggerController.rotationSpeed = rotationSpeed;
        daggerController.damage = damage;
        daggerController.layerToHit = layerToHit;
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

    private void ConfigureTimes()
    {
        var adapter = GetComponent<SkeletonDaggerAnimationAdapter>();
        daggerReleaseTime = adapter.ActualReleaseTime;
        daggerReplaceTime = adapter.ActualReplaceTime;
    }

    private void PrepareReleasedDagger()
    {
        Collider collider = GetComponent<Collider>();
        float offsetX = 0.22f; //Odleg³oœc w osi x od collidera
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
