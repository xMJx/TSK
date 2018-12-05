using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace KalmanSimulation
{
    public class KalmanFilter : MonoBehaviour
    {

        public float GPSError;

        private Vector4 prevState; // lastX, lastY, lastVx, lastVy
        private Vector4 currState;
        private Matrix4x4 prevP;
        public Matrix4x4 currP;
        private MarineTrafficResponse currentGPS;
        private MarineTrafficResponse initialGPS = null;

        private Matrix4x4 A;
        private Matrix4x4 AT;
        private Matrix4x4 Q;
        private Matrix4x4 PT;
        private Matrix4x4 H;
        private Matrix4x4 HT;
        private Matrix4x4 R;
        private int i = 0;

        public List<MarineTrafficResponse> GPSData { get; set; }

        public void Start()
        {
            prevState = new Vector4(transform.position.x, transform.position.y, 0.0f, 0.0f);
            A = new Matrix4x4();

            Q = new Matrix4x4();
            Q.SetRow(0, new Vector4(0.0001f, 0, 0, 0));
            Q.SetRow(1, new Vector4(0, 0.0001f, 0, 0));
            Q.SetRow(2, new Vector4(0, 0, 0.0001f, 0));
            Q.SetRow(3, new Vector4(0, 0, 0, 0.0001f));

            H.SetRow(0, new Vector4(1, 0, 0, 0));
            H.SetRow(1, new Vector4(0, 1, 0, 0));

            HT = H.transpose;

            GPSError = 5f; // kilometers

            R.SetRow(0, new Vector2(GPSError, 0));
            R.SetRow(1, new Vector2(0, GPSError));

            prevP = Q;
        }

        public Vector2 CalculatePosition(float timestep)
        {
            if (initialGPS == null)
            {
                initialGPS = GPSData[0];
            }
            if (i < GPSData.Count)
            {
                Debug.Log("=========================");
                Debug.Log("Index: " + i);
                Predict(timestep);
                Correct();
                Debug.Log("=========================");
            }

            return (Vector2)currState;
        }

        internal void DrawNextGPS(LineRenderer lineRenderer)
        {
            if (i <= GPSData.Count)
            {
                lineRenderer.positionCount = i;
                Vector4 z = UtilizeGPSData(currentGPS);
                //z = z * 0.001f;
                lineRenderer.SetPosition(i-1, new Vector3(z.x, z.y, 0));
            }
        }

        public void Predict(float timestep)
        {
            // odswiezenie A
            A.SetRow(0, new Vector4(1, 0, timestep, 0));
            A.SetRow(1, new Vector4(0, 1, 0, timestep));
            A.SetRow(2, new Vector4(0, 0, 1, 0));
            A.SetRow(3, new Vector4(0, 0, 0, 1));
            Debug.Log("A");
            Debug.Log(A);
            // estymacja stanu
            currState = A * prevState;
            prevState = currState;
            Debug.Log("currState");
            Debug.Log(currState);
            // transpozycja A
            AT = A.transpose;
            Debug.Log("AT");
            Debug.Log(AT);
            // odswiezenie P
            currP = AddMatrices(A * prevP * AT, Q);
            Debug.Log("currP");
            Debug.Log(currP);
            Debug.Log("PREDICT END");
        }

        public void Correct()
        {
            currentGPS = GPSData[i++];

            Vector4 z = UtilizeGPSData(currentGPS);
            Debug.Log("z");
            Debug.Log(z);
            PT = currP.transpose;
            Debug.Log("PT");
            Debug.Log(PT);
            //Matrix4x4 K = 
            Matrix4x4 temp1 = currP * HT;
            Debug.Log("temp1");
            Debug.Log(temp1);
            Matrix4x4 temp2 = (AddMatrices(H * currP * HT, R));
            Debug.Log("temp2");
            Debug.Log(temp2);
            Matrix4x4 temp3 = new Matrix4x4(
                new Vector4(temp2[5], -temp2[4], 0, 0),
                new Vector4(-temp2[1], temp2[0], 0, 0),
                Vector4.zero,
                Vector4.zero);
            for (int x = 0; x < 16; ++x)
                temp3[x] *= 1.0f / (temp2[0] * temp2[5] - temp2[1] * temp2[4]);
            Debug.Log("temp3");
            Debug.Log(temp3);
            Matrix4x4 K = temp1 * temp3;
            Debug.Log("K");
            Debug.Log(K);
            currP = (SubtractMatrices(Matrix4x4.identity, K * H)) * currP;
            Debug.Log("currP");
            Debug.Log(currP);
            currState = currState + K * (z - currState);
            Debug.Log("currState");
            Debug.Log(currState);
        }

        public double DistanceAB(Vector2 A, Vector2 B)
        {
            return Mathf.Sqrt(Mathf.Pow((B.x - A.x), 2) + Mathf.Pow((Mathf.Cos(A.x * Mathf.PI / 180) * (B.y - A.y)), 2)) * 40075.704 / 360;
        }

        public float KnotsToKilometersPerHour(float knots)
        {
            return 1.85f * knots;
        }

        public float CourseToRadians(int course)
        {
            return 0.0174532925f * (90 - course);
        }

        public Matrix4x4 AddMatrices(Matrix4x4 A, Matrix4x4 B)
        {
            Matrix4x4 ret = new Matrix4x4();
            ret.SetColumn(0, new Vector4(A.m00 + B.m00, A.m10 + B.m10, A.m20 + B.m20, A.m30 + B.m30));
            ret.SetColumn(1, new Vector4(A.m01 + B.m01, A.m11 + B.m11, A.m21 + B.m21, A.m31 + B.m31));
            ret.SetColumn(2, new Vector4(A.m02 + B.m02, A.m12 + B.m12, A.m22 + B.m22, A.m32 + B.m32));
            ret.SetColumn(3, new Vector4(A.m03 + B.m03, A.m13 + B.m13, A.m23 + B.m23, A.m33 + B.m33));

            return ret;
        }

        public Matrix4x4 SubtractMatrices(Matrix4x4 A, Matrix4x4 B)
        {
            Matrix4x4 ret = new Matrix4x4();
            ret.SetColumn(0, new Vector4(A.m00 - B.m00, A.m10 - B.m10, A.m20 - B.m20, A.m30 - B.m30));
            ret.SetColumn(1, new Vector4(A.m01 - B.m01, A.m11 - B.m11, A.m21 - B.m21, A.m31 - B.m31));
            ret.SetColumn(2, new Vector4(A.m02 - B.m02, A.m12 - B.m12, A.m22 - B.m22, A.m32 - B.m32));
            ret.SetColumn(3, new Vector4(A.m03 - B.m03, A.m13 - B.m13, A.m23 - B.m23, A.m33 - B.m33));

            return ret;
        }

        public Vector4 UtilizeGPSData(MarineTrafficResponse data)
        {
            // czy to dziala?
            Vector2 localPosition = new Vector2((float)data.LON, (float)data.LAT);
            Vector2 initial = new Vector2((float)initialGPS.LON, (float)initialGPS.LAT);
            Vector2 dir = localPosition - initial;
            dir.Normalize();
            dir *= (float)DistanceAB(localPosition, initial);

            float x = dir.x;
            float y = dir.y;
            float Vx = KnotsToKilometersPerHour(data.SPEED / 10 * Mathf.Cos(CourseToRadians(data.COURSE)));
            float Vy = KnotsToKilometersPerHour(data.SPEED / 10 * Mathf.Sin(CourseToRadians(data.COURSE)));

            return new Vector4(x, y, Vx, Vy);
        }
    }
}
