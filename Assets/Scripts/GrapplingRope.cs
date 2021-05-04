using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Spring spring;
    private LineRenderer lr;
    private Vector3 currentGrapplePosition;
    //public GrapplingGun grapplingGun;
    public int quality;
    public float damper;
    public float strength;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public float simualtionSpeed = 12f;
    public AnimationCurve affectCurve;
    Hook hook;
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
        hook = GetComponent<Hook>();
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!hook.init)
        {
            //currentGrapplePosition = grapplingGun.gunTip.position;
            currentGrapplePosition = transform.position;
            spring.Reset();
            if (lr.positionCount > 0)
                lr.positionCount = 0;
            return;
        }

        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePoint = hook.TagPos;
        //var grapplePoint = transform.position;
        var gunTipPosition = hook.grapple.ShootTransform.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * simualtionSpeed);

        for (var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value *
                         affectCurve.Evaluate(delta);
            offset = new Vector3
                (
                Mathf.Clamp(offset.x, -waveHeight, waveHeight),
                Mathf.Clamp(offset.y, -waveHeight, waveHeight),
                Mathf.Clamp(offset.z, -waveHeight, waveHeight)
                );
            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offset);
        }
        transform.position = lr.GetPosition(quality);
    }
}
