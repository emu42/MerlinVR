using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gestures;
using UnityEngine.Events;
using UnityEngine.XR;

public class TrackerSetup : MonoBehaviour {

    public static string GESTURE_CIRCLE = "Circle";
    public static string GESTURE_HEART = "Heart";
    public static string GESTURE_TRIANGLE = "Triangle";
    public static string GESTURE_SQUARE = "Square";
    public static float BALL_SPEED = 10;

    public static float PARTICLE_EFFECT_DURATION = 5;

    public GameObject ball;

    public GameObject bigBall;

    public GameObject healParticles;

    public GameObject bigHealParticles;

    public GameObject fireParticles;

    public GameObject bigFireParticles;

    public TextMesh text;
    private GestureMonitor tracker;
    public LineRenderer lineRenderer;
    public IController controller;

    private PlayerLogic playerLogic;

    void Start () {
        tracker = gameObject.AddComponent<GestureMonitor>();
        tracker.controller = controller;
        tracker.lineRenderer = lineRenderer;

        GenerateGestures();

        tracker.AddGestureCompleteCallback(GestureComplete);
        tracker.AddGestureFailedCallback(GestureFailed);
        tracker.AddGestureStartCallback(GestureStart);

        // try and set tracking reference pos
        List<XRInputSubsystem> lst = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(lst);
        for (int i = 0; i < lst.Count; i++) {
            lst[i].TrySetTrackingOriginMode(TrackingOriginModeFlags.Floor);
        }

        playerLogic = gameObject.GetComponent<PlayerLogic>();
    }

    
    void GestureStart() {
        playerLogic.IncreaseGestureActiveCount();
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;
    }


    void GestureComplete(GestureMetaData data) {
        int remainingActive = playerLogic.DecreaseGestureActiveCount();
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        SetText(data);
        if (remainingActive == 0) {
            string storedGesture = playerLogic.GetAndClearStoredGesture();

            if (storedGesture == null) {
                // casz single hand gesture spell
                if (data.name == GESTURE_CIRCLE) {
                    SpawnBall();
                } else if (data.name == GESTURE_HEART) {
                    CastHeal();
                } else if (data.name == GESTURE_TRIANGLE) {
                    CastFire();
                } else if (data.name == GESTURE_SQUARE) {
                    ResetVRPosition();
                }
            } else {
                CastCombinedGestureSpell(data.name, storedGesture);
            }
        }
        else {
            playerLogic.SetStoredGesture(data.name);
        }
    }


    void GestureFailed(GestureMetaData data) {
        // when a gesture fails, the stored one fizzles as well
        playerLogic.DecreaseGestureActiveCount();
        playerLogic.GetAndClearStoredGesture();

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

    void SpawnBigBall() {
        Vector3 handPos = controller.QueryGTransform().position;
        GameObject newBall = Instantiate(bigBall, handPos, Quaternion.identity);
        Rigidbody rb = newBall.GetComponent<Rigidbody>();
        Vector3 aimVector = handPos - Camera.main.transform.position;
        rb.velocity = aimVector.normalized * BALL_SPEED;
    }

    void CastHeal() {
        playerLogic.ReceiveHeal(10);

        GameObject effect = Instantiate(healParticles, controller.transform);
        Destroy(effect, PARTICLE_EFFECT_DURATION);
    }

    void CastBigHeal() {
        playerLogic.ReceiveHeal(50);

        GameObject effect = Instantiate(bigHealParticles, controller.transform);
        Destroy(effect, PARTICLE_EFFECT_DURATION);
    }

    void CastFire() {
        playerLogic.ReceiveDamage(10);

        GameObject effect = Instantiate(fireParticles, controller.transform);
        Destroy(effect, PARTICLE_EFFECT_DURATION);
    }

    void CastBigFire() {
        playerLogic.ReceiveDamage(50);

        GameObject effect = Instantiate(bigFireParticles, controller.transform);
        Destroy(effect, PARTICLE_EFFECT_DURATION);
    }

    void ResetVRPosition() {
        List<XRInputSubsystem> lst = new List<XRInputSubsystem>();
        SubsystemManager.GetInstances<XRInputSubsystem>(lst);
        for (int i = 0; i < lst.Count; i++) {
            lst[i].TryRecenter();
        }
    }

    void CastCombinedGestureSpell(string gesture1, string gesture2) {
        if (GesturesMatch(gesture1, gesture2, GESTURE_CIRCLE, GESTURE_CIRCLE)) {
            SpawnBigBall();
        } else if (GesturesMatch(gesture1, gesture2, GESTURE_CIRCLE, GESTURE_TRIANGLE)) {
            
        } else if (GesturesMatch(gesture1, gesture2, GESTURE_CIRCLE, GESTURE_HEART)) {
            // TODO cast chicken
        } else if (GesturesMatch(gesture1, gesture2, GESTURE_HEART, GESTURE_HEART)) {
            CastBigHeal();
        } else if (GesturesMatch(gesture1, gesture2, GESTURE_TRIANGLE, GESTURE_TRIANGLE)) {
            CastBigFire();
        } else {
            // fizzle
        }
    }

    bool GesturesMatch(string g1, string g2, string cmp1, string cmp2) {
        return (g1 == cmp1 && g2 == cmp2) || (g1 == cmp2 && g2 == cmp1);
    }

    void GenerateGestures() {

        tracker.AddGesture(GESTURE_SQUARE, new SquareGesture(.6f));
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
