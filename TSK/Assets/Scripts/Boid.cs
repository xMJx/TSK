using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalmanSimulation
{

    public class Boid : MonoBehaviour
    {
        public KalmanFilter Kalman { get; set; }
        public float MaxTurnRate { get; set; }
        public Vector2 Heading;
        public Vector2 Side;
        

        public Boid()
        {
            MaxTurnRate = 1.0f;
            Heading = new Vector2(0.0f, 1.0f);
        }
        // Use this for initialization
        void Start()
        {
            Kalman = GetComponent<KalmanFilter>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Kalman.CalculatePosition(Time.fixedDeltaTime);

            //RotateHeadingToFacePosition(transform.position);
            RotateBoidToMatchHeading();
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

        bool RotateBoidToMatchHeading()
        {
            transform.rotation = Quaternion.EulerRotation(0,0,-Mathf.Atan2(Heading.x, Heading.y));

            return false;
        }

    }
}