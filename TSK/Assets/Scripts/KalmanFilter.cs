using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KalmanSimulation
{
    public class KalmanFilter : MonoBehaviour
    {
        public Vector4 previousState; // lastX, lastY, lastVx, lastVy
        private Matrix4x4 A;

        //public KalmanFilter(float startX, float startY)
        //{
        //    previousX = startX;
        //    previousY = startY;
        //}

        public void Start()
        {
            previousState = new Vector4(transform.position.x, transform.position.y, 0.0f, 0.0f);
            A = new Matrix4x4();
        }

        public Vector3 CalculatePosition(float timestep)
        {
            return Predict(timestep);
        }

        public Vector3 Predict(float timestep)
        {
            A.SetRow(0, new Vector4(1, 0, timestep, 0));
            A.SetRow(1, new Vector4(0, 1, 0, timestep));
            A.SetRow(2, new Vector4(0, 0, 1, 0)); // to jest stale, mozna wrzucic do startu
            A.SetRow(3, new Vector4(0, 0, 0, 1)); // to jest stale, mozna wrzucic do startu

            //Matrix4x4 AT = A.transpose;
            //Vector2 P = new Vector2();
            //Vector2 Q = new Vector2();

            Vector4 newState = A * previousState;

            //P = AddMatrices(A * P * AT, Q); // działa?

            //Matrix4x4 H = new Matrix4x4(1, 1, 0, 0);
            previousState = newState;

            Vector2 newCoord = (Vector2)newState;
            return newCoord;
        }

        public Matrix4x4 Correct(Matrix4x4 predict)
        {
            Matrix4x4 correct = predict;
            return correct;
        }

        public double DistanceAB(Vector2 A, Vector2 B)
        {
            return Mathf.Sqrt(Mathf.Pow((B.x - A.x), 2) + Mathf.Pow((Mathf.Cos(A.x * Mathf.PI / 180) * (B.y - A.y)), 2)) * 40075.704 / 360;
        }

        public Matrix4x4 AddMatrices (Matrix4x4 A, Matrix4x4 B)
        {
            Matrix4x4 ret = new Matrix4x4();
            ret.SetRow(0, new Vector4(A.m00 + B.m00, A.m10 + B.m10, A.m20 + B.m20, A.m30 + B.m30));
            ret.SetRow(1, new Vector4(A.m01 + B.m01, A.m11 + B.m11, A.m21 + B.m21, A.m31 + B.m31));
            ret.SetRow(2, new Vector4(A.m02 + B.m02, A.m12 + B.m12, A.m22 + B.m22, A.m32 + B.m32));
            ret.SetRow(3, new Vector4(A.m03 + B.m03, A.m13 + B.m13, A.m23 + B.m23, A.m33 + B.m33));

            return ret;
        }
    }
}
