using System;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class GeometryUtil {

        public static Vector3 CalculateVectorReflection(Vector3 reflectionBaseNormal, Vector3 vector)
        {
            return Vector3.Reflect(vector, reflectionBaseNormal);
        }

    }

}
