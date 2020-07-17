﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public Transform player;
    public float maxAngle;
    public float maxRadius;
    private Vector3 change;

    private bool isInFOV = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Vector3 fovLine1 = Quaternion.AngleAxis(maxAngle, transform.forward) * transform.up * maxRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-maxAngle, transform.forward) * transform.up * maxRadius;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, fovLine1);
        Gizmos.DrawRay(transform.position, fovLine2);

        if (!isInFOV)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(transform.position, (player.position - transform.position).normalized * maxRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.up * maxRadius);
    }
    
    public static bool inFOV(Transform checkingObject, Transform target, float maxAngle, float maxRadius)
    {
        Collider[] overlaps = new Collider[50];
        int count = Physics.OverlapSphereNonAlloc(checkingObject.position, maxRadius, overlaps);
        for (int i = 0; i < count + 1; i++)
        {
            if(overlaps[i] != null)
            {
                if(overlaps[i].transform == target)
                {
                    Vector3 directionBetween = (target.position - checkingObject.position).normalized;
                    directionBetween.z *= 0;
                    float angle = Vector3.Angle(checkingObject.up, directionBetween);
                    if(angle <= maxAngle)
                    {
                        Ray ray = new Ray(checkingObject.position, target.position - checkingObject.position);
                        RaycastHit hit;
                        if(Physics.Raycast(ray, out hit, maxRadius))
                        {
                            if(hit.transform == target)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    private void Update()
    {
        isInFOV = inFOV(transform, player, maxAngle, maxRadius);
    }
}
