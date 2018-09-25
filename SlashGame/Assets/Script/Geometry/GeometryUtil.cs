using System;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class GeometryUtil {

        public static Vector3 CalculateVectorReflection(Vector3 reflectionBaseVector, Vector3 reflectionNormalVector, Vector3 vector)
        {
            float vectorProjectionMagnitude = Vector3.Dot(vector, reflectionBaseVector);
            return vector - reflectionBaseVector.normalized * vectorProjectionMagnitude * 2;
        }

    }

}
