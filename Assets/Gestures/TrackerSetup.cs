using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gestures;
using UnityEngine.Events;


public class TrackerSetup : MonoBehaviour {

    public static string GESTURE_CIRCLE = "Circle";
    public static string GESTURE_HEART = "Heart";
    public static string GESTURE_TRIANGLE = "Triangle";

    public static float BALL_SPEED = 10;

    public static float PARTICLE_EFFECT_DURATION = 3;

    public GameObject ball;

    public GameObject healParticles;

    public GameObject fireParticles;

    public int playerHealth = 100;  

    public TextMesh text;
    private GestureMonitor tracker;
    public LineRenderer lineRenderer;
    public IController controller;

    void Start () {
        tracker = gameObject.AddComponent<GestureMonitor>();
        tracker.controller = controller;
        tracker.lineRenderer = lineRenderer;

        GenerateGestures();

        tracker.AddGestureCompleteCallback(GestureComplete);
        tracker.AddGestureFailedCallback(GestureFailed);
        tracker.AddGestureStartCallback(GestureStart);

    }

    
    void GestureStart() {
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
    }


    void GestureComplete(GestureMetaData data) {
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        SetText(data);
        if (data.name == GESTURE_CIRCLE) {
            SpawnBall();
        } else if (data.name == GESTURE_HEART) {
            CastHeal();
        } else if (data.name == GESTURE_TRIANGLE) {
            CastFire();
        }

    }


    void GestureFailed(GestureMetaData data) {
        if (text != null) {
            string newText = "Result: <i><color=red>" + "None" + "</color></i>";
            text.text = newText;
        }
    }

    void SpawnBall() {
        Vector3 handPos = controller.QueryGTransform().position;
        GameObject newBall = Instantiate(ball, handPos, Quaternion.identity);
        Rigidbody rb = newBall.GetComponent<Rigidbody>();
        Vector3 aimVector = handPos - Camera.main.transform.position;
        rb.velocity = aimVector.normalized * BALL_SPEED;
    }

    void CastHeal() {
        playerHealth = Math.Min(playerHealth + 10, 100);

        GameObject newHealEffect = Instantiate(healParticles, controller.transform);
        Destroy(newHealEffect, PARTICLE_EFFECT_DURATION);
    }

    void CastFire() {
        playerHealth = Math.Max(playerHealth - 10, 0);

        GameObject newHealEffect = Instantiate(fireParticles, controller.transform);
        Destroy(newHealEffect, PARTICLE_EFFECT_DURATION);
    }

    void GenerateGestures() {

        //tracker.AddGesture("Square", new SquareGesture(.6f));
        tracker.AddGesture(GESTURE_CIRCLE, new CircleGesture(.4f));
        tracker.AddGesture(GESTURE_TRIANGLE, new TriangleGesture(.8f));
        tracker.AddGesture(GESTURE_HEART, new HeartGesture());

        tracker.AddGesture("Letter-S", new Gesture().AddChecks(new List<Check> {
            new ArcCheck(new Vector3(.5f, .5f, 0), -90, new Vector3(0,.5f,0)),
            new ArcCheck(new Vector3(0, 1, 0), -90, new Vector3(0,.5f,0)),
            new ArcCheck(new Vector3(-.5f,.5f,0), -90, new Vector3(0,.5f,0)),

            new ArcCheck(new Vector3(0, 0, 0), 90, new Vector3(0,-.5f,0)),
            new ArcCheck(new Vector3(.5f,-.5f,0), 90, new Vector3(0,-.5f,0)),
            new ArcCheck(new Vector3(0,-1,0), 90, new Vector3(0,-.5f,0)) })
            
            .SetNormalizer(new FittedNormalizer(new Vector3(-.5f, -1.0f, 0), new Vector3(.5f, 1.0f, 0))));


        /*
        tracker.AddGesture("Plus", new Gesture().AddChecks(new List<Check>() {
            new LineCheck(new Vector3(-1,0,0), new Vector3(1,0,0)),
            new LineCheck(new Vector3(0,-1,0), new Vector3(0,1,0)),

            new RadiusCheck(new Vector3(-1,0,0)),
            new RadiusCheck(new Vector3(1,0,0)),
            new RadiusCheck(new Vector3(0,-1,0)),
            new RadiusCheck(new Vector3(0,1,0)),

        }).SetNormalizer(new FittedNormalizer()));
        */
    }



    private void SetText(GestureMetaData data) {
        if (text != null) {
            string newText = "Result: <i><color=green>" + data.name + "</color></i>";
            text.text = newText;
        }
    }

}
