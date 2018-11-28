using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KalmanSimulation
{
    public class KalmanFilter : MonoBehaviour
    {
        private Vector4 prevState; // lastX, lastY, lastVx, lastVy
        private Vector4 currState;
        private Matrix4x4 prevP;
        public Matrix4x4 currP;

        private Matrix4x4 A;
        private Matrix4x4 AT;
        private Matrix4x4 Q;
        
        public List<MarineTrafficResponse> GPSData { get; set; }

        //public KalmanFilter(float startX, float startY)
        //{
        //    previousX = startX;
        //    previousY = startY;
        //}

        public void Start()
        {
            prevState = new Vector4(transform.position.x, transform.position.y, 0.0f, 0.0f);
            A = new Matrix4x4();

            Q = new Matrix4x4();
            Q.SetRow(0, new Vector4(0.0001f, 0, 0, 0));
            Q.SetRow(1, new Vector4(0, 0.0001f, 0, 0));
            Q.SetRow(2, new Vector4(0, 0, 0.0001f, 0));
            Q.SetRow(3, new Vector4(0, 0, 0, 0.0001f));

            prevP = Q;
        }

        public Vector2 CalculatePosition(float timestep)
        {
            Predict(timestep);

            return (Vector2)currState;
        }

        public void Predict(float timestep)
        {
            // odswiezenie A
            A.SetRow(0, new Vector4(1, 0, timestep, 0));
            A.SetRow(1, new Vector4(0, 1, 0, timestep));
            A.SetRow(2, new Vector4(0, 0, 1, 0));
            A.SetRow(3, new Vector4(0, 0, 0, 1));
            
            // estymacja stanu
            currState = A * prevState;
            prevState = currState;
            
            // transpozycja A
            AT = A.transpose;

            // odswiezenie P
            currP = AddMatrices(A*prevP*AT, Q);
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
            ret.SetColumn(0, new Vector4(A.m00 + B.m00, A.m10 + B.m10, A.m20 + B.m20, A.m30 + B.m30)); 
            ret.SetColumn(1, new Vector4(A.m01 + B.m01, A.m11 + B.m11, A.m21 + B.m21, A.m31 + B.m31));
            ret.SetColumn(2, new Vector4(A.m02 + B.m02, A.m12 + B.m12, A.m22 + B.m22, A.m32 + B.m32));
            ret.SetColumn(3, new Vector4(A.m03 + B.m03, A.m13 + B.m13, A.m23 + B.m23, A.m33 + B.m33));

            return ret;
        }
    }
}
