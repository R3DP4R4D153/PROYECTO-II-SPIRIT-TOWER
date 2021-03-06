﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public Transform player;
    public float maxAngle;
    public float maxRadius;
    public int maxColliders;
    public AudioSource Alarm;
    public bool isInFOV = false;

    // Start is called before the first frame update
    private void Start()
    {
        Alarm = this.GetComponent<AudioSource>();
    }
    // Debud method to see the lines of the FOV when the player is near 
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
        Gizmos.DrawRay(transform.position, player.position - transform.position);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position, transform.up * maxRadius);

    }
    //Creates a RayCastHit to detect if a player is in th FOV 
    public static bool inFOV(Transform enemy, Transform target, float maxAngle, float maxRadius, int maxColliders)
    {
        Collider[] hitColliders = new Collider[maxColliders];
        int count = Physics.OverlapSphereNonAlloc(enemy.position, maxRadius, hitColliders);
        for (int i = 0; i < count; i++)
        {
            if(hitColliders[i] != null)
            {
                if (hitColliders[i].transform == target)
                {
                    Vector3 directionBetween = (target.transform.position - enemy.transform.position);
                    directionBetween.z *= 0;
                    float angle = Vector3.Angle(enemy.up, directionBetween);
                    if(angle <= maxAngle)
                    {
                        Ray ray = new Ray(enemy.position, target.position - enemy.position);
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
    // Enable Alarm sound
    private void AlarmActivation()
    {
        if (!isInFOV)
        {
            Alarm.Play();
        }
    }
    // Update is called once per frame
    private void Update()
    {
        isInFOV = inFOV(transform, player, maxAngle, maxRadius, maxColliders);
        AlarmActivation();
    }
}
