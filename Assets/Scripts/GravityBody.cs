// Created by jmanchuck October 2020

/* 
    A message for those of you who have came here to read the source code...
    
    Welcome.

    I applaud your curiosity to learn more and see what's happening behind the scenes. 
    Perhaps some of you may already understand this code, others may see it as some 
    monstrosity of words and numbers. 

    I have documented this code to (hopefully) make it easier for you to read.
    The functions that carry out the meat of the simulation are FixedUpdate and Attract,
    so have a look at those.
    
    Everything else is just some extra sugar and padding. 

    If you have any questions, feel free to reach out to me if you can find my contact.
*/

using System.Collections.Generic;
using UnityEngine;

// GravityBody is the script that is in charge of calculating the kinematics of each body
// via Newton's Law of Gravitation. 

public class GravityBody : MonoBehaviour
{
    // STATIC VARIABLES // 
    public static float G_Runtime = 67f;
    public static bool initialized;
    public static List<GravityBody> AllBodies = new List<GravityBody>();
    public static bool SoundOnCollision;

    // PRIVATE INSTANCE VARIABLES //
    protected Transform bodyTransform;
    protected Rigidbody rb;

    // PUBLIC VARIABLES //
    public float mass = 1;
    public Vector3 initVelocity;
    private float radius;

    // Initialisation and gathering references
    protected void Awake()
    {
        GravityBody.Initialize();
    }

    // This is where all the physics happens
    // Upon every update frame, Unity will call the functions defined within this body
    protected void FixedUpdate()
    {
        // Attract all the other gravity objects to itself (this)
        foreach (GravityBody body in AllBodies)
        {
            if (body != this)
            {
                this.Attract(body);
            }
        }
    }

    // Attract another gravitational body
    // This is where Newton's law of gravitation happens
    protected virtual void Attract(GravityBody gb)
    {
        if (gb != null)
        {
            Vector3 r_to_this = this.bodyTransform.position - gb.bodyTransform.position;

            // Newton's law of gravitation
            Vector3 force = r_to_this * G_Runtime * gb.rb.mass * this.rb.mass / Mathf.Pow(r_to_this.magnitude, 3);
            gb.SetForceTo(force);
        }
    }

    // Gets all references to gravity bodies and places them in static list for interactions
    public static void Initialize()
    {
        if (GravityBody.initialized)
        {
            return;
        }

        // Find all GravityBody objects and put them in static list
        CollectAllGB();

        foreach (GravityBody gb in AllBodies)
        {
            gb.AssignVariables();
        }
        GravityBody.initialized = true;
    }

    // Finds all GravityBody objects in the Scene and adds them to the static list
    public static void CollectAllGB()
    {
        AllBodies = new List<GravityBody>();
        foreach (GravityBody gb in FindObjectsOfType<GravityBody>())
        {
            AllBodies.Add(gb);
        }
    }

    // Assigns private references to components 
    private void AssignVariables()
    {
        rb = GetComponent<Rigidbody>();
        bodyTransform = GetComponent<Transform>();
        radius = GetComponent<SphereCollider>().radius;
        rb.mass = mass;
        rb.velocity = initVelocity;
        rb.useGravity = false;
    }

    // Print list of the names of all the gravitational bodies in console 
    private static void PrintGravityBodyNames()
    {
        List<string> gbNames = new List<string>
        {
            "Gravity body list:"
        };
        foreach (GravityBody gb in AllBodies)
        {
            gbNames.Add(gb.transform.root.name + " | ");
        }
        Debug.Log(string.Join("", gbNames));
    }

    // Pauses time if PauseOnCollision flag is true - currently disabled
    // Plays some sound if SoundOnCollision flag is true
    private void OnCollisionEnter(Collision other)
    {
        if (!GravityBody.SoundOnCollision)
        {
            return;
        }

        AudioSource audioSource;
        GameObject gObj = GameObject.Find("audio");
        if (gObj == null)
        {
            gObj = new GameObject("audio");
            audioSource = gObj.AddComponent<AudioSource>();
            AudioClip collisionSound = (AudioClip)Resources.Load("Audio/bongCollision");
            audioSource.clip = collisionSound;
            audioSource.pitch = 2f;
        }
        audioSource = gObj.GetComponent<AudioSource>();
        audioSource.Play();
    }

    public static GameObject CreateGravityBody(string name, Vector3 position, float mass, float scale, MaterialColours.Colours colour, bool trail = false)
    {
        GameObject gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Rigidbody rb = gObj.AddComponent<Rigidbody>();
        GravityBody gb = gObj.AddComponent<GravityBody>();
        if (trail)
        {
            // Trail gameobject as child of GB so they scale together
            GameObject trailGObj = new GameObject();
            trailGObj.name = "Trail";
            trailGObj.transform.parent = gObj.transform;

            // Create component under trail gameobject
            TrailRenderer tr = trailGObj.AddComponent<TrailRenderer>();
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0.0f, 1.0f);
            curve.AddKey(0.35f, 0.35f);
            curve.AddKey(1.0f, 0.0f);
            tr.widthCurve = curve;
            tr.widthMultiplier = scale * 0.5f;
            tr.material = new Material(Shader.Find("Sprites/Default"));
            tr.time = 30f;
        }
        gObj.transform.name = name;
        gObj.transform.position = position;
        gObj.transform.localScale = scale * Vector3.one;
        gb.mass = mass;
        gObj.GetComponent<Renderer>().material = MaterialColours.GetMaterial(colour);
        return gObj;
    }



    /* GET SET METHODS */

    public void SetVelocityTo(Vector3 velocity)
    {
        rb.velocity = velocity;
    }

    public void SetForceTo(Vector3 force)
    {
        rb.AddForce(force);
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public Rigidbody GetRigidbody()
    {
        return this.rb;
    }

    public float GetRadius()
    {
        return this.radius;
    }

    public float GetMass()
    {
        return rb.mass;
    }

    public Vector3 GetVelocity()
    {
        if (rb == null)
        {
            return Vector3.zero;
        }
        else
        {
            return rb.velocity;
        }
    }

    // With respect the origin
    public float GetSpeed()
    {
        return this.rb.velocity.magnitude;
    }

    // With respect to some other GravityBody
    public float GetSpeed(GravityBody othergb)
    {
        return (this.rb.velocity - othergb.rb.velocity).magnitude;
    }

    public static float DistanceBetween(GravityBody a, GravityBody b)
    {
        Vector3 r = a.transform.position - b.transform.position;
        return r.magnitude;
    }

    public Vector3 VectorTo(GravityBody to)
    {
        return to.transform.position - this.transform.position;
    }
}
