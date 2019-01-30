using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionDetection
{
    public abstract class Collider2D
    {
        private UnityEngine.Collider2D m_Collider2D = null;

        public Collider2D(UnityEngine.Collider2D collider)
        {
            m_Collider2D = collider;
        }

        public Vector2 GetCenter()
        {
            return m_Collider2D.transform.TransformPoint(m_Collider2D.offset);
        }

        public bool IsIntersect(UnityEngine.Collider2D collider)
        {
            return IsIntersect(this, collider);
        }

        public bool IsIntersect(Collider2D collider)
        {
            return IsIntersect(this, collider);
        }

        #region Properties
        //
        // Summary:
        //     The world space bounding area of the collider.
        public Bounds bounds
        { get { return m_Collider2D.bounds; } }
        #endregion

        #region Abstract Methods
        public abstract Vector2 GetSupport(Vector2 direction);
        #endregion

        #region Static Methods
        public static bool IsIntersect(Bounds bounds1, Bounds bounds2)
        {
            return Utility.IsIntersect2D(bounds1, bounds2);
        }

        public static bool IsIntersect(UnityEngine.Collider2D collider1, UnityEngine.Collider2D collider2)
        {
            return MPR.IsIntersect(collider1, collider2);
        }

        public static bool IsIntersect(Collider2D collider1, Collider2D collider2)
        {
            return MPR.IsIntersect(collider1, collider2);
        }

        public static bool IsIntersect(Collider2D collider1, UnityEngine.Collider2D collider2)
        {
            return MPR.IsIntersect(collider1, collider2);
        }
        #endregion
    }

    public class ColliderCircle2D : Collider2D
    {
        private UnityEngine.CircleCollider2D m_Collider = null;

        public ColliderCircle2D(UnityEngine.CircleCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);
            localDirection.Normalize();

            Vector2 support = m_Collider.offset + localDirection * m_Collider.radius;
            support = m_Collider.transform.TransformPoint(support);

            return support;
        }
    }

    public class ColliderCapsule2D : Collider2D
    {
        private UnityEngine.CapsuleCollider2D m_Collider = null;

        public ColliderCapsule2D(UnityEngine.CapsuleCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);
            localDirection.Normalize();

            bool isHorizontal = (m_Collider.direction == CapsuleDirection2D.Horizontal);

            Vector2 capsuleDirection = isHorizontal ? Vector2.right : Vector2.up;
            float diameter = isHorizontal ? m_Collider.size.y : m_Collider.size.x;
            float length = (isHorizontal ? m_Collider.size.x : m_Collider.size.y) - diameter;

            float halfLength = length * 0.5f;
            float radius = diameter * 0.5f;

            Vector2 point1 = -halfLength * capsuleDirection;
            Vector2 point2 = halfLength * capsuleDirection;

            float projection1 = Vector2.Dot(point1, localDirection);
            float projection2 = Vector2.Dot(point2, localDirection);

            Vector2 support;
            if (projection1 > projection2)
                support = point1;
            else
                support = point2;

            support += m_Collider.offset + localDirection * radius;
            support = m_Collider.transform.TransformPoint(support);

            return support;
        }
    }

    public class ColliderBox2D : Collider2D
    {
        private UnityEngine.BoxCollider2D m_Collider = null;

        public ColliderBox2D(UnityEngine.BoxCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);

            Vector2 support = m_Collider.size * 0.5f;

            if (localDirection.x < 0.0f)
                support.x *= -1.0f;

            if (localDirection.y < 0.0f)
                support.y *= -1.0f;

            support += m_Collider.offset;
            support = m_Collider.transform.TransformPoint(support);

            return support;
        }
    }

    public class ColliderPolygon2D : Collider2D
    {
        private UnityEngine.PolygonCollider2D m_Collider = null;

        public ColliderPolygon2D(UnityEngine.PolygonCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);

            Vector2 support = m_Collider.points[0];
            float maxProjection = Vector2.Dot(support, localDirection);

            foreach (Vector2 point in m_Collider.points)
            {
                float projection = Vector2.Dot(point, localDirection);
                if (maxProjection < projection)
                {
                    support = point;
                    maxProjection = projection;
                }
            }

            support += m_Collider.offset;
            support = m_Collider.transform.TransformPoint(support);

            return support;
        }
    }
}