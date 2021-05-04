using UnityEngine;

public class Hook : MonoBehaviour
{
    //[SerializeField] float hookForce = 25f;
    //[SerializeField] float hooktime = 1;
    [HideInInspector] public GrappleGear grapple;
    Rigidbody rigid;
    LineRenderer lineRenderer;
    [HideInInspector] public bool init = false;
    [HideInInspector] public Vector3 TagPos = Vector3.zero;
    public void Initialize(GrappleGear grapple, Vector3 shootDir, Vector3 shotPos)
    {
        init = true;
        transform.forward = shootDir;
        //transform.position = shotPos;
        //transform.DOMove(shotPos, hooktime);
        TagPos = shotPos;
        this.grapple = grapple;
        rigid = GetComponent<Rigidbody>();
        lineRenderer = GetComponent<LineRenderer>();
        //rigid.AddForce(transform.forward * hookForce, ForceMode.Impulse);
    }
    // Update is called once per frame
    void Update()
    {
        /*        Vector3[] positions = new Vector3[]
                    {
                        transform.position,
                        grapple.transform.position
                    };

                lineRenderer.SetPositions(positions);*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((LayerMask.GetMask("Grapple") & 1 << other.gameObject.layer) > 0)
        {
            rigid.useGravity = false;
            rigid.isKinematic = true;

            grapple.StartPull();
        }
    }
}
