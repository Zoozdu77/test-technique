using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Camera")]
    [SerializeField] private Transform cameraRotationPoint;
    [SerializeField] private float camSensitivity;
    [SerializeField] private Vector2 camMinMax;
    [SerializeField] private Transform debugCube;
    [SerializeField] private float camcastDist;
    [SerializeField] private float camcastRange;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask hutLayer;
    [Header("Player Actions")]
    [SerializeField] private float size;
    [SerializeField] private float speedMax;
    [SerializeField] private float hutInteractionRange;
    [SerializeField] private float forceOnIce;
    [HideInInspector]public bool hutteOuverte = false;
    [HideInInspector] public List<Transform> cubesOnPlayer = new();

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        cameraRotationPoint.position = transform.position;
        if(Input.GetButton("Fire1"))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.nearClipPlane + 0.1f;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hutHit, camcastRange, hutLayer))
            {
                if (Vector3.Distance(hutHit.point, transform.position) < hutInteractionRange)
                {
                    hutHit.transform.GetComponent<HutScript>().OpenMenu(transform);
                    hutteOuverte = true;
                }
            }else if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit groundHit, camcastRange, groundLayer))
            {
                debugCube.transform.position = groundHit.point;
                Movement(groundHit.point);
            }
        }
        if(Input.GetButton("Fire2"))
        {
            Rotate();
        }
    }

    private void Movement(Vector3 targetPosition)
    {
        transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        Debug.DrawRay(transform.position, -transform.up);
         Debug.DrawRay(transform.position + transform.forward * (size / 2), -transform.up);
        Physics.Raycast(transform.position, -transform.up, out RaycastHit hitCenter, size * 2, groundLayer);
        Physics.Raycast(transform.position + transform.forward * (size / 2), -transform.up, out RaycastHit hitFront, size * 2, groundLayer);
        Vector3 Direction = (new Vector3(targetPosition.x, transform.position.y, targetPosition.z) - transform.position).normalized;
        if (hitFront.transform.GetComponent<Tile>().canWalk && Direction!= null)
        {
            if (!hitCenter.transform.GetComponent<Tile>().slippery)
            {
                rb.velocity = Vector3.zero;
                if (hitCenter.transform == hitFront.transform && 
                    Vector3.Distance(hitCenter.point, hitCenter.transform.position) >= Vector3.Distance(hitFront.point, hitCenter.transform.position))
                {
                    transform.position += speedMax * hitCenter.transform.GetComponent<Tile>().speedInMult * Time.deltaTime * Direction;
                } else
                {
                    transform.position += speedMax * hitCenter.transform.GetComponent<Tile>().speedOutMult * Time.deltaTime * Direction;
                }
            } else
            {
                if (rb.velocity.sqrMagnitude <= speedMax * 2)
                {
                    rb.AddForce(forceOnIce * Direction, ForceMode.Force);
                }
            }
        }
    }

    private void Rotate()
    {
        Vector2 cameraMovement = new(-Input.GetAxis("Mouse Y") * camSensitivity,Input.GetAxis("Mouse X") * camSensitivity);
        cameraRotationPoint.rotation = Quaternion.Euler(Mathf.Clamp(cameraRotationPoint.rotation.eulerAngles.x + cameraMovement.x, camMinMax.x, camMinMax.y),
                                                        cameraRotationPoint.rotation.eulerAngles.y + cameraMovement.y,
                                                        0);
    }
}
