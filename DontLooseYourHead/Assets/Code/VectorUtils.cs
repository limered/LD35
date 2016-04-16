using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Code.Utils
{
    public static class VectorUtils
    {
        public static float Angle(Vector2 vector)
        {
            return Mathf.Atan2(vector.y, vector.x);
        }

        public static Vector2 FromAngle(float angle)
        {
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }
    }
}
