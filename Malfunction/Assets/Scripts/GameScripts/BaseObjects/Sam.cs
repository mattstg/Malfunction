﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Sam : BO_Static {
    float clock = 0;
    public float rocketTime = 5;

    public Color rayColor = Color.red;
    public float rayWidth = 0.1f;

    Transform targetAsteroid;
    LineRenderer lineRenderer;

    public Material lineRendMat;
    public float rayAlfaBuffer = 0.4f;
    public virtual Type RocketTypeToSpawn => Type.Rocket;

    public Transform turretHead;
    public Transform firePoint;

    public override void Spawn(Vector2 posistion)
    {
        base.Spawn(posistion);
        clock = Random.Range(0, rocketTime);
        InitializeLineRenderer();
    }

    public override void Refresh(float dt)
    {
        base.Refresh(dt);

        UpdateTargetting(dt);

        clock += dt * BuffManager.turretFireTimePerc * Random.Range(.98f,1.01f); //To undo allignment of all aiming the same

        UpdateLaser();

        if (clock > rocketTime && targetAsteroid != null) // - rocketTime * BuffManager.turretLockOnReduction));
        {
            clock = 0;
            Rocket rocket = (Rocket)manager.SpawnObjectFromPool(RocketTypeToSpawn, firePoint.position);
            rocket.SetVelocity();
            rocket.SetTarget(targetAsteroid);
            SetTarget(dt);
        }
    }

    public virtual void SetTarget(float dt)
    {
        if(manager.activeAsteroids.Count > 0)
        {
            Transform closest = null;
            float distance = 0;
            foreach(Transform t in manager.activeAsteroids)
            {
                float dist = (t.position - transform.position).magnitude;
                if (closest == null || dist < distance)
                {
                    distance = dist;
                    closest = t;
                }
            }
            targetAsteroid = closest;
        }
    }

    public void UpdateLaser()
    {
        lineRenderer.startColor = new Color(rayColor.r, rayColor.g, rayColor.b, Mathf.Clamp01((clock / rocketTime) - rayAlfaBuffer + 0.1f));
        lineRenderer.endColor = new Color(rayColor.r, rayColor.g, rayColor.b, Mathf.Clamp01((clock / rocketTime) - rayAlfaBuffer));
    }

    public virtual void UpdateTargetting(float dt)
    {
        if ((targetAsteroid == null || !manager.activeAsteroids.Contains(targetAsteroid)))
        {
            SetTarget(dt);
        }
        else
        {
            UpdateLineRenderer();
            turretHead.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, (targetAsteroid.position - transform.position)));
        }
    }

    public virtual void InitializeLineRenderer()
    {
        if(lineRenderer == null)
        {
            lineRenderer = transform.gameObject.AddComponent<LineRenderer>();
            lineRenderer.startColor = lineRenderer.endColor = rayColor;
            lineRenderer.startWidth = lineRenderer.endWidth = rayWidth;
            lineRenderer.material = lineRendMat;
        }
    }

    public virtual void UpdateLineRenderer()
    {
        lineRenderer.SetPositions(new Vector3[] { firePoint.position, targetAsteroid.transform.position } );
    }
}
