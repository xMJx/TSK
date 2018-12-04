using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalmanSimulation
{
    [RequireComponent(typeof(LineRenderer))]
    public class Boid : MonoBehaviour
    {
        public KalmanFilter Kalman { get; set; }
        public Vector2 Heading;
        public Vector2 Side;
        public float MaxTurnRate { get; set; }
        private float time;
        private bool active;
        private LineRenderer lineRenderer;

        public Boid()
        {
            MaxTurnRate = 1.0f;
            Heading = new Vector2(0.0f, 1.0f);
            active = false;
        }
        // Use this for initialization
        void Start()
        {
            Kalman = GetComponent<KalmanFilter>();
            lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, Vector3.zero);
        }

        // Update is called once per frame
        void Update()
        {
            if (active && Time.time > time + 2)
            {
                time = Time.time;
                transform.position = Kalman.CalculatePosition(3600) * 0.1f;
                //Kalman.DrawNextGPS(lineRenderer);
                //RotateHeadingToFacePosition(transform.position);
                RotateBoidToMatchHeading();
            }
        }

        bool RotateHeadingToFacePosition(Vector2 target)
        {
            // Normalizowany wektor od boidu do celu
            Vector2 toTarget = (target - (Vector2)transform.position).normalized;

            float angle = Vector2.SignedAngle(Heading, toTarget);

            if (angle > 0)
                angle = Mathf.Min(angle, MaxTurnRate);
            else
                angle = -Mathf.Min(-angle, MaxTurnRate);

            //Heading = toTarget;
            Heading.Normalize();

            Heading = Quaternion.Euler(0, 0, angle) * Heading;


            return false;
        }

        public bool RotateBoidToMatchHeading()
        {
            transform.rotation = Quaternion.EulerRotation(0, 0, -Mathf.Atan2(Heading.x, Heading.y));

            return false;
        }

        public void Activate()
        {
            active = true;
            time = Time.time;
        }
    }
}