using System.Collections;
using UnityEngine;

public class GrappleGear : MonoBehaviour
{
    [SerializeField] float pullSpeed = 0.5f;
    [SerializeField] float boostForce = 10f;
    [SerializeField] float stopDistance = 4f;
    [SerializeField] LayerMask GrappleLayer = 1;
    [SerializeField] GameObject hookPrefab = null;
    [SerializeField] Transform shootTransform = null;
    Camera mainCam;
    Hook hook;
    bool pulling;
    Rigidbody rigid;
    [SerializeField] float cooldown = .5f;
    bool isCooled = true;
    public Transform ShootTransform { get => shootTransform; set => shootTransform = value; }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        pulling = false;
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Break();
            if (hook != null)
            {
                DestroyHook();
            }
            RaycastHit hitInfo;
            if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity))
            {
                RaycastHit newHitInfo;

                if (Physics.Raycast(shootTransform.position, (hitInfo.point.SetPositionY(shootTransform.position.y) - shootTransform.position).normalized, out newHitInfo, Mathf.Infinity, GrappleLayer))
                {
                    StopAllCoroutines();
                    pulling = false;
                    hook = Instantiate(hookPrefab, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
                    hook.Initialize(this, (newHitInfo.point - shootTransform.position).normalized, newHitInfo.point);
                    transform.LookAt(newHitInfo.point.SetPositionY(shootTransform.position.y), Vector3.up);
                    StartCoroutine(DestroyHookAfterLifetime());
                }
            }

        }
        if (hook != null && Input.GetMouseButtonDown(1))
        {
            //DestroyHook();
            rigid.AddForce((hook.transform.position.SetPositionY(shootTransform.position.y) - transform.position).normalized * boostForce, ForceMode.Impulse);
        }

        if (!pulling || hook == null) return;

        if (Vector3.Distance(transform.position, hook.transform.position.SetPositionY(shootTransform.position.y)) <= stopDistance)
        {
            DestroyHook();
        }


    }

    private void FixedUpdate()
    {
        if (pulling)
            rigid.AddForce((hook.transform.position.SetPositionY(shootTransform.position.y) - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
    }
    public void StartPull()
    {
        pulling = true;
    }

    private void DestroyHook()
    {
        if (hook == null) return;

        pulling = false;
        Destroy(hook.gameObject);
        hook = null;
    }
    private IEnumerator I_CoolDown()
    {
        isCooled = false;
        yield return new WaitForSeconds(cooldown);
        isCooled = true;
    }
    private IEnumerator DestroyHookAfterLifetime()
    {
        yield return new WaitForFixedUpdate();
        if (isCooled)
        {
            StartCoroutine(I_CoolDown());
            rigid.AddForce((hook.transform.position.SetPositionY(shootTransform.position.y) - transform.position).normalized * boostForce, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(8f);

        DestroyHook();
    }
}

