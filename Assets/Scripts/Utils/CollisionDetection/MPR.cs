using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MPR
{
    #region MPR Collider 2D
    private abstract class MPRCollider2D
    {
        private Collider2D m_Collider2D = null;

        public MPRCollider2D(Collider2D collider)
        {
            m_Collider2D = collider;
        }

        public Vector2 GetCenter()
        {
            return m_Collider2D.transform.TransformPoint(m_Collider2D.offset);
        }

        public abstract Vector2 GetSupport(Vector2 direction);
    }

    private class MPRColliderCircle2D : MPRCollider2D
    {
        private CircleCollider2D m_Collider = null;

        public MPRColliderCircle2D(CircleCollider2D collider) : base(collider)
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

    private class MPRColliderCapsule2D : MPRCollider2D
    {
        private CapsuleCollider2D m_Collider = null;

        public MPRColliderCapsule2D(CapsuleCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);
            localDirection.Normalize();

            bool    isHorizontal = (m_Collider.direction == CapsuleDirection2D.Horizontal);

            Vector2 capsuleDirection = isHorizontal ? Vector2.right : Vector2.up;
            float   diameter = isHorizontal ? m_Collider.size.y : m_Collider.size.x;
            float   length   = (isHorizontal ? m_Collider.size.x : m_Collider.size.y) - diameter;

            float halfLength = length   * 0.5f;
            float radius     = diameter * 0.5f;

            Vector2 point1 = -halfLength * capsuleDirection;
            Vector2 point2 =  halfLength * capsuleDirection;

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

    private class MPRColliderBox2D : MPRCollider2D
    {
        private BoxCollider2D m_Collider = null;

        public MPRColliderBox2D(BoxCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);

            Vector2 support = m_Collider.size * 0.5f;

            if(localDirection.x < 0.0f)
                support.x *= -1.0f;

            if (localDirection.y < 0.0f)
                support.y *= -1.0f;

            support += m_Collider.offset;
            support = m_Collider.transform.TransformPoint(support);

            return support;
        }
    }

    private class MPRColliderPolygon2D : MPRCollider2D
    {
        private PolygonCollider2D m_Collider = null;

        public MPRColliderPolygon2D(PolygonCollider2D collider) : base(collider)
        {
            m_Collider = collider;
        }

        public override Vector2 GetSupport(Vector2 direction)
        {
            Vector2 localDirection = m_Collider.transform.InverseTransformVector(direction);

            Vector2 support = m_Collider.points[0];
            float maxProjection = Vector2.Dot(support, localDirection);

            foreach(Vector2 point in m_Collider.points)
            {
                float projection = Vector2.Dot(point, localDirection);
                if(maxProjection < projection)
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
    #endregion

    #region Utility
    static private bool IsLessEqual2D(Vector3 v1, Vector3 v2)
    {
        return (v1.x <= v2.x) && (v1.y <= v2.y);
    }

    static private float Cross(Vector2 v1, Vector2 v2)
    {
        return v1.x * v2.y - v1.y * v2.x;
    }

    static private Vector2 Perpendicular(Vector2 v1, Vector2 v2)
    {
        Vector2 a = v2 - v1;
        Vector2 b = -v1;

        Vector2 r = Vector2.Perpendicular(a);

        if (Vector2.Dot(r, b) < 0.0f)
            r *= -1.0f;

        return r;
    }

    static private MPRCollider2D ConvertToMPRCollider(Collider2D collider)
    {
        MPRCollider2D result = null;

        if (collider is CircleCollider2D)
        {
            result = new MPRColliderCircle2D(collider as CircleCollider2D);
        }
        else if (collider is CapsuleCollider2D)
        {
            result = new MPRColliderCapsule2D(collider as CapsuleCollider2D);
        }
        else if (collider is BoxCollider2D)
        {
            result = new MPRColliderBox2D(collider as BoxCollider2D);
        }
        else if (collider is PolygonCollider2D)
        {
            result = new MPRColliderPolygon2D(collider as PolygonCollider2D);
        }
        else
        {
            Debug.Assert(false, "[MPR] Invalid collider type");
        }

        return result;
    }
    #endregion

    #region Interface
    static public bool IsIntersect2D(Bounds bounds1, Bounds bounds2)
    {
        return IsLessEqual2D(bounds1.min, bounds2.max) && IsLessEqual2D(bounds2.min, bounds1.max);
    }

    static public bool IsIntersect(Collider2D collider1, Collider2D collider2)
    {
        if ((collider1 == null) || (collider2 == null))
        {
            Debug.Assert(false, "[MPR] Invalid colliders");
            return false;
        }

        if (!IsIntersect2D(collider1.bounds, collider2.bounds))
            return false;

        MPRCollider2D mprCollider1 = ConvertToMPRCollider(collider1);
        MPRCollider2D mprCollider2 = ConvertToMPRCollider(collider2);

        if((mprCollider1 == null) || (mprCollider2 == null))
        {
            Debug.Assert(false, "[MPR] Invalid MPR colliders");
            return false;
        }

        return IsIntersect(mprCollider1, mprCollider2);
	}
    #endregion

    #region MPR 2D
    static private bool IsIntersect(MPRCollider2D collider1, MPRCollider2D collider2)
    {
        Vector2 v0 = collider2.GetCenter() - collider1.GetCenter();
        Debug.Assert(v0.sqrMagnitude > 0.0f, "[MPR] Invalid  V0");

        Vector2 v1 = collider2.GetSupport(-v0) - collider1.GetSupport(v0);
        Debug.Assert(Vector2.Distance(v0, v1) > 0.0f, "[MPR] Invalid  V0 and V1");

        Vector2 n = Perpendicular(v0, v1);
        Vector2 v2 = collider2.GetSupport(n) - collider1.GetSupport(-n);

        const int maxIterationsCount = 10;
        for(int i = 0; i < maxIterationsCount; ++i)
        {
            Vector2 portal = v2 - v1;

            float side1 = Cross(portal, v0 - v1);
            float side2 = Cross(portal, -v1);
            if (side1 * side2 >= 0.0f)
                return true;

            n = Perpendicular(v1, v2);
            Debug.Assert(v0.sqrMagnitude > 0.0f, "[MPR] Invalid  portal's normal");
            Vector2 v3 = collider2.GetSupport(n) - collider1.GetSupport(-n);
            if (Vector2.Dot(v3, n) < 0.0f)
                return false;

            side1 = Cross(v3 - v0, -v0);
            side2 = Cross(v1 - v0, -v0);
            if (side1 * side2 < 0.0f)
                v2 = v3;
            else
                v1 = v3;
        }

        Debug.LogWarning("[MPR] Failed to find a portal");
        return false;
    }
    #endregion
}
