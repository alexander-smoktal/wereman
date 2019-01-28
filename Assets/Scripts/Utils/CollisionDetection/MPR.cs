using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionDetection
{
    public static class MPR
    {
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

        static private Collider2D ConvertToInternalCollider(UnityEngine.Collider2D collider)
        {
            Collider2D result = null;

            if (collider is UnityEngine.CircleCollider2D)
            {
                result = new ColliderCircle2D(collider as UnityEngine.CircleCollider2D);
            }
            else if (collider is CapsuleCollider2D)
            {
                result = new ColliderCapsule2D(collider as UnityEngine.CapsuleCollider2D);
            }
            else if (collider is BoxCollider2D)
            {
                result = new ColliderBox2D(collider as UnityEngine.BoxCollider2D);
            }
            else if (collider is PolygonCollider2D)
            {
                result = new ColliderPolygon2D(collider as UnityEngine.PolygonCollider2D);
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

        static public bool IsIntersect(UnityEngine.Collider2D collider1, UnityEngine.Collider2D collider2)
        {
            if ((collider1 == null) || (collider2 == null))
            {
                Debug.Assert(false, "[MPR] Invalid colliders");
                return false;
            }

            if (!IsIntersect2D(collider1.bounds, collider2.bounds))
                return false;

            Collider2D internalCollider1 = ConvertToInternalCollider(collider1);
            Collider2D internalCollider2 = ConvertToInternalCollider(collider2);

            if ((internalCollider1 == null) || (internalCollider2 == null))
            {
                Debug.Assert(false, "[MPR] Invalid MPR colliders");
                return false;
            }

            return IsIntersectInternal(internalCollider1, internalCollider2);
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

            return IsIntersectInternal(collider1, collider2);
        }
        #endregion

        #region MPR 2D
        static private bool IsIntersectInternal(Collider2D collider1, Collider2D collider2)
        {
            Vector2 v0 = collider2.GetCenter() - collider1.GetCenter();
            Debug.Assert(v0.sqrMagnitude > 0.0f, "[MPR] Invalid  V0");

            Vector2 v1 = collider2.GetSupport(-v0) - collider1.GetSupport(v0);
            Debug.Assert(Vector2.Distance(v0, v1) > 0.0f, "[MPR] Invalid  V0 and V1");

            Vector2 n = Perpendicular(v0, v1);
            Vector2 v2 = collider2.GetSupport(n) - collider1.GetSupport(-n);

            const int maxIterationsCount = 10;
            for (int i = 0; i < maxIterationsCount; ++i)
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
}