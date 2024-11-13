using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boid : MonoBehaviour
{
    
    private List<GameObject> boids = new List<GameObject>();
    private List<Rigidbody> rbs = new List<Rigidbody>();

    private void Start()
    {
        boids.AddRange(GameObject.FindGameObjectsWithTag("BoidLing"));
        foreach (GameObject boid in boids)
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rbs.Add(rb);
        }

        

    }

    private void FixedUpdate()
    {
        MoveBoid();
    }

    public void MoveBoid()
    {
        Vector3 v1, v2, v3, v4 = new Vector3();
        int counter = 0;

        foreach (GameObject boid in boids)
        {
            //Debug.Log(boid.name);
            Rigidbody rb = boid.GetComponent<Rigidbody>(); // use list to get corresponding rigidbody instead of "getting" it every frame

            v1 = FindCenterOfMass(boid);
            v2 = KeepSmallDistance(boid);
            v3 = MatchVelocity(boid, rb);
            v4 = BoundingThePosition(boid);

            rb.velocity = rb.velocity + v1 + v2 + v3 + v4;

            boid.transform.position = boid.transform.position + rb.velocity;
            counter++;

        }
    }

    public Vector3 FindCenterOfMass(GameObject b) // boids rule 1
    {
        Vector3 percievedCenter = new Vector3();

        foreach (GameObject boid in boids)
        {
            if(boid != b)
            {
                percievedCenter = percievedCenter + boid.transform.position;
                
            }
        }
        percievedCenter = percievedCenter / (boids.Count - 1);

        return (percievedCenter - b.transform.position) / 100;
    }

    public Vector3 KeepSmallDistance(GameObject b) // boids rule 2
    {
        Vector3 distanceCorrection = new Vector3(0,0,0);
        float xCorrection = 0;
        float yCorrection = 0;
        float zCorrection = 0;

        foreach(GameObject boid in boids)
        {
            if(boid != b)
            {
                if(Mathf.Abs(boid.transform.position.x - b.transform.position.x) < 0.1)
                {
                    xCorrection = xCorrection - (boid.transform.position.x - b.transform.position.x);
                }
                if (Mathf.Abs(boid.transform.position.y - b.transform.position.y) < 0.1)
                {
                    yCorrection = yCorrection - (boid.transform.position.y - b.transform.position.y);
                }
                if (Mathf.Abs(boid.transform.position.z - b.transform.position.z) < 0.1)
                {
                    zCorrection = zCorrection - (boid.transform.position.z - b.transform.position.z);
                }
                distanceCorrection = new Vector3(xCorrection, yCorrection, zCorrection);
            }
        }

        return distanceCorrection;
    }

    public Vector3 MatchVelocity(GameObject b, Rigidbody rb) // boids rule 3
    {
        Vector3 percievedVelocity = new Vector3();
        Vector3 personalVelocity = new Vector3();

        foreach (GameObject boid in boids)
        {
            rb.velocity = personalVelocity;


            if (boid != b)
            {
                percievedVelocity = percievedVelocity + personalVelocity;
            }
        }

        percievedVelocity = percievedVelocity / (boids.Count - 1);

        return (percievedVelocity - personalVelocity) / 8;
    }

    public Vector3 BoundingThePosition(GameObject b) // rule 4
    {
        int xMin = -5, xMax = 5, yMin = -5, yMax = 5, zMin = -5, zMax = 5;
        Vector3 v = new Vector3();

        if (b.transform.position.x < xMin)
            v.x = 0.5f;
        else if (b.transform.position.x > xMax)
            v.x = -0.5f;

        if (b.transform.position.y < yMin)
            v.y = 0.5f;
        else if (b.transform.position.y > yMax)
            v.y = -0.5f;

        if (b.transform.position.z < zMin)
            v.z = 0.5f;
        else if (b.transform.position.z > zMax)
            v.z = -0.5f;

        return v;
    }

    public Vector3 LimitVelocity(GameObject b, Rigidbody rb) // doesnt seem to work, gets stuck
    {
        float vLimit = 0.05f;
        Vector3 v = new Vector3();

        if (Mathf.Abs(rb.velocity.x) > vLimit)
            v.x = (rb.velocity.x / Mathf.Abs(rb.velocity.x)) * vLimit;
        if (Mathf.Abs(rb.velocity.y) > vLimit)
            v.y = (rb.velocity.y / Mathf.Abs(rb.velocity.y)) * vLimit;
        if (Mathf.Abs(rb.velocity.z) > vLimit)
            v.z = (rb.velocity.z / Mathf.Abs(rb.velocity.z)) * vLimit;



        return v;

    }


}
