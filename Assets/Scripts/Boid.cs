using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boid : MonoBehaviour
{

    private List<Rigidbody> rbs = new List<Rigidbody>();

    private float xOutside = 0, yOutside = 0, zOutside = 0;

    private void Start()
    {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("BoidLing"));
        foreach (GameObject gameObject in gameObjects)
        {
            rbs.Add(gameObject.GetComponent<Rigidbody>());
        }
    }
    private void FixedUpdate()
    {
        MoveBoid();
    }

    public void MoveBoid()
    {
        Vector3 v1, v2, v3, v4, v5 = new Vector3();
        int randomNumber = Random.Range(0, 10);

        if(randomNumber > 5)
        {
            xOutside = Random.Range(-0.25f, 0.25f);
            yOutside = Random.Range(-0.25f, 0.25f);
            zOutside = Random.Range(-0.25f, 0.25f);
        }
       
        foreach (Rigidbody rb in rbs)
        {
            v1 = FindCenterOfMass(rb);
            v2 = KeepSmallDistance(rb);
            v3 = MatchVelocity(rb);
            v4 = BoundingThePosition(rb);
            v5 = OutsideForce(xOutside, yOutside, zOutside);

            rb.velocity = rb.velocity + v1 + v2 + v3 + v4 + v5;
            rb.velocity = LimitSpeed(rb.velocity);
            rb.transform.position = rb.transform.position + rb.velocity;
        }
    }

    public Vector3 FindCenterOfMass(Rigidbody rigidBody) // rule 1
    {
        Vector3 percievedCenter = new Vector3();
        int scalar = 75;

        foreach (Rigidbody rb in rbs)
        {
            if (rb != rigidBody)
            {
                percievedCenter = percievedCenter + rb.transform.position;
            }
        }
        percievedCenter = percievedCenter / (rbs.Count - 1);

        return (percievedCenter - rigidBody.transform.position) / scalar;
    }

    public Vector3 KeepSmallDistance(Rigidbody rigidBody) // rule 2 doesnt work well
    {
        Vector3 distanceCorrection = new Vector3(0, 0, 0);

        foreach (Rigidbody rb in rbs)
        {
            if (rb != rigidBody)
            {
                if (Mathf.Abs(rb.transform.position.x - rigidBody.transform.position.x) < 0.1)
                {
                    distanceCorrection.x = 2 * distanceCorrection.x - (rb.transform.position.x - rigidBody.transform.position.x);
                }
                if (Mathf.Abs(rb.transform.position.y - rigidBody.transform.position.y) < 0.1)
                {
                    distanceCorrection.y = 2 * distanceCorrection.y - (rb.transform.position.y - rigidBody.transform.position.y);
                }
                if (Mathf.Abs(rb.transform.position.z - rigidBody.transform.position.z) < 0.1)
                {
                    distanceCorrection.z = 2 * distanceCorrection.z - (rb.transform.position.z - rigidBody.transform.position.z);
                }
            }
        }
        return distanceCorrection;
    }

    public Vector3 MatchVelocity(Rigidbody rigidbody) // rule 3
    {
        Vector3 percievedVelocity = new Vector3();
        Vector3 personalVelocity = new Vector3();
        int scalar = 5;

        foreach (Rigidbody rb in rbs)
        {
            rb.velocity = personalVelocity;


            if (rb != rigidbody)
            {
                percievedVelocity = percievedVelocity + personalVelocity;
            }
        }

        percievedVelocity = percievedVelocity / (rbs.Count - 1);
        return (percievedVelocity - personalVelocity) / scalar;
    }

    public Vector3 BoundingThePosition(Rigidbody rb) // rule 4
    {
        int xMin = -5, xMax = 5, yMin = -5, yMax = 5, zMin = -1, zMax = 4;
        float movementCorrection = 0.25f;
        Vector3 v = new Vector3();

        if (rb.transform.position.x < xMin)
            v.x = movementCorrection;
        else if (rb.transform.position.x > xMax)
            v.x = -movementCorrection;

        if (rb.transform.position.y < yMin)
            v.y = movementCorrection;
        else if (rb.transform.position.y > yMax)
            v.y = -movementCorrection;

        if (rb.transform.position.z < zMin)
            v.z = movementCorrection;
        else if (rb.transform.position.z > zMax)
            v.z = -movementCorrection;

        return v;
    }

    public Vector3 OutsideForce(float xForce, float yForce, float zForce) // rule 5
    {
        Vector3 v = new Vector3(xForce, yForce, zForce);
        return v;
    }

    public Vector3 LimitSpeed(Vector3 v)
    {
        float speedLimit = 0.05f;

        if (Mathf.Abs(v.x) > speedLimit)
        {
            v.x = (v.x / Mathf.Abs(v.x)) * speedLimit;
        }
        if (Mathf.Abs(v.y) > speedLimit)
        {
            v.y = (v.y / Mathf.Abs(v.y)) * speedLimit;
        }
        if (Mathf.Abs(v.z) > speedLimit)
        {
            v.z = (v.z / Mathf.Abs(v.z)) * speedLimit;
        }

        return v;
    }


}
