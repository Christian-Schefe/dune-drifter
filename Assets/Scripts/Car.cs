using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CarSettings
{
    public float accel;
    public float boost;
    public float topSpeed;
    public float gripX;
    public float gripZ;
    public float rotate;
    public float rotVel;
    public float groundAngDrag;
    public float airAngDrag;

    public float minRotSpd;
    public float maxRotSpd;
    public AnimationCurve slipL;
    public AnimationCurve slipU;
    public float slipMod;
    public Vector3 centerOfMass;
}

[System.Serializable]
public struct CarValues
{
    public float rotate;
    public float accel;
    public float gripX;
    public float gripZ;
    public float rotVel;
    public float slip;

    public float isRight;
    public float isForward;

    public bool isRotating;
    public bool isGrounded;
    public bool isStumbling;

    public float distToGround;

    public float inThrottle;
    public float inTurn;
    public bool inReset;
    public bool isStuck;
    public bool autoReset;
    public bool inBoost;
    public bool inSlip;

    public Vector3 spawnP;
    public Quaternion spawnR;

    public Vector3 vel;
    public Vector3 pvel;
}

public class Car : MonoBehaviour
{
    public ComponentCache<Rigidbody> rb;
    private Bounds groupCollider;

    public CarSettings settings;
    public CarValues values = new()
    {
        isRight = 1.0f,
        isForward = 1.0f,

        isRotating = false,
        isGrounded = true,
        isStumbling = false,
    };

    private void Awake()
    {
        rb = new();
    }


    void Start()
    {
        rb.Get(gameObject).AddForce(Vector3.zero, ForceMode.Acceleration);

        values.spawnP = transform.position;
        values.spawnR = transform.rotation;

        groupCollider = GetBounds(gameObject);
        values.distToGround = groupCollider.extents.y;

        rb.Get(gameObject).centerOfMass = Vector3.Scale(groupCollider.extents, settings.centerOfMass);
    }

    void Update()
    {
        if (transform.position.y < -10)
        {
            transform.SetPositionAndRotation(values.spawnP, values.spawnR);
            values.inReset = true;
        }

        values.accel = settings.accel;
        values.rotate = settings.rotate;
        values.gripX = settings.gripX;
        values.gripZ = settings.gripZ;
        values.rotVel = settings.rotVel;
        rb.Get(gameObject).angularDrag = settings.groundAngDrag;

        values.accel *= Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        values.accel = values.accel > 0f ? values.accel : 0f;
        values.gripZ *= Mathf.Cos(transform.eulerAngles.x * Mathf.Deg2Rad);
        values.gripZ = values.gripZ > 0f ? values.gripZ : 0f;
        values.gripX *= Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad);
        values.gripX = values.gripX > 0f ? values.gripX : 0f;

        values.isGrounded = Physics.Raycast(transform.position, -transform.up, values.distToGround + 0.1f);
        if (!values.isGrounded)
        {
            values.rotate = 0f;
            values.accel = 0f;
            values.gripX = 0f;
            values.gripZ = 0f;
            rb.Get(gameObject).angularDrag = settings.airAngDrag;
        }

        values.isStumbling = rb.Get(gameObject).angularVelocity.magnitude > 0.1f * settings.rotate * Time.deltaTime;
        if (values.isStumbling)
        {
            //rotate = 0f;
        }

        if (values.pvel.magnitude < settings.minRotSpd)
        {
            values.rotate = 0f;
        }
        else
        {
            values.rotate = values.pvel.magnitude / settings.maxRotSpd * values.rotate;
        }

        if (values.rotate > settings.rotate) values.rotate = settings.rotate;

        if (!values.inSlip)
        {
            values.slip = settings.slipL.Evaluate(Mathf.Abs(values.pvel.x) / settings.slipMod);
            if (values.slip == 1f) values.inSlip = true;
        }
        else
        {
            values.slip = settings.slipU.Evaluate(Mathf.Abs(values.pvel.x) / settings.slipMod);
            if (values.slip == 0f) values.inSlip = false;
        }

        values.rotate *= (1f - 0.3f * values.slip);
        values.rotVel *= (1f - values.slip);

        InputKeyboard();

        Controller();

        values.vel = transform.InverseTransformDirection(rb.Get(gameObject).velocity);

        if (values.isRotating)
        {
            values.vel = values.vel * (1 - values.rotVel) + values.pvel * values.rotVel;
        }

        values.isRight = values.vel.x > 0f ? 1f : -1f;
        values.vel.x -= values.isRight * values.gripX * Time.deltaTime;
        if (values.vel.x * values.isRight < 0f) values.vel.x = 0f;

        values.isForward = values.vel.z > 0f ? 1f : -1f;
        values.vel.z -= values.isForward * values.gripZ * Time.deltaTime;
        if (values.vel.z * values.isForward < 0f) values.vel.z = 0f;

        if (values.vel.z > settings.topSpeed) values.vel.z = settings.topSpeed;
        else if (values.vel.z < -settings.topSpeed) values.vel.z = -settings.topSpeed;

        rb.Get(gameObject).velocity = transform.TransformDirection(values.vel);
    }

    void InputKeyboard()
    {
        values.inThrottle = Input.GetAxisRaw("Vertical");
       // values.inBoost = Input.GetAxisRaw("Boost") > 0f;
        values.inTurn = Input.GetAxisRaw("Horizontal");

        values.inReset = values.inReset || Input.GetKeyDown(KeyCode.R);
    }

    void Controller()
    {

        if (values.inBoost) values.accel *= settings.boost;

        if (values.inThrottle > 0.5f || values.inThrottle < -0.5f)
        {
            rb.Get(gameObject).velocity += transform.forward * values.inThrottle * values.accel * Time.deltaTime;
            values.gripZ = 0f;
        }

        if (values.autoReset)
        {
            if (values.pvel.magnitude <= 0.01f)
            {
                values.inReset = values.isStuck;
                values.isStuck = true;
            }
            else
            {
                values.isStuck = false;
            }
        }

        if (values.inReset)
        {  // Reset
            float y = transform.eulerAngles.y;
            transform.eulerAngles = new Vector3(0, y, 0);
            rb.Get(gameObject).velocity = new Vector3(0, -1f, 0);
            transform.position += Vector3.up * 2;
            values.inReset = false;
        }

        values.isRotating = false;

        values.pvel = transform.InverseTransformDirection(rb.Get(gameObject).velocity);

        if (values.inTurn > 0.5f || values.inTurn < -0.5f)
        {
            float dir = (values.pvel.z < 0) ? -1 : 1;
            RotateGradConst(values.inTurn * dir);
        }
    }

    Vector3 drot = new(0f, 0f, 0f);

    void RotateGradConst(float isCW)
    {
        drot.y = isCW * values.rotate * Time.deltaTime;
        transform.rotation *= Quaternion.AngleAxis(drot.y, transform.up);
        values.isRotating = true;
    }

    public static Bounds GetBounds(GameObject obj)
    {
        Bounds bounds = new();
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();

        if (colliders.Length > 0)
        {

            foreach (Collider collider in colliders)
            {

                if (collider.enabled)
                {
                    bounds = collider.bounds;
                    break;
                }
            }

            foreach (Collider collider in colliders)
            {
                if (collider.enabled)
                {
                    bounds.Encapsulate(collider.bounds);
                }
            }
        }
        return bounds;
    }
}
