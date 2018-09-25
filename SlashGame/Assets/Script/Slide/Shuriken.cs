﻿using Assets.Script.Geometry;
using UnityEngine;

namespace Assets.Script.Slide
{
    public class Shuriken
    {

        public GameObject ShurikenGameObject;
        public Vector3 VelocityVector;
        public Vector3 InitialPosition;
        public Quaternion InitialRotation;
        public float SHURIKEN_DIMENSION = 0.5f;

        public Shuriken(Point initialPosition, Vector3 initialVelocity, Vector3 planeNormal)
        {
            this.InitialPosition = new Vector3(initialPosition.x, initialPosition.y, 0);
            this.VelocityVector = initialVelocity;
            this.InitialRotation = CalculateQuaternionRotation(initialVelocity, planeNormal);
            
        }

        private Quaternion CalculateQuaternionRotation(Vector3 forward, Vector3 up)
        {
            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(forward, up);
            return rotation;
        }

        public void SetShurikenGameObject(GameObject shurikenObject)
        {
            this.ShurikenGameObject = shurikenObject;
            shurikenObject.transform.position = InitialPosition;
            shurikenObject.GetComponent<Rigidbody>().velocity = VelocityVector;
        }

        public Point GetShurikenCenterPosition()
        {
            return ShurikenGameObject != null ? new Point(ShurikenGameObject.transform.position.x, ShurikenGameObject.transform.position.y) : new Point(float.MaxValue, float.MaxValue) ;
        }

        public void SetShurikenVelocity(Vector3 velocityVector)
        {
            this.VelocityVector = velocityVector;
            this.ShurikenGameObject.GetComponent<Rigidbody>().velocity = Vector3.down;
        }
    }
}
