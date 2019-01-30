using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionDetection
{
    public static class MPR
    {
        #region Interface
        static public bool IsIntersect(UnityEngine.Collider2D collider1, UnityEngine.Collider2D collider2)
        {
            if ((collider1 == null) || (collider2 == null))
            {
                Debug.Assert(false, "[MPR] Invalid colliders");
                return false;
            }

            if (!Utility.IsIntersect2D(collider1.bounds, collider2.bounds))
                return false;

            Collider2D internalCollider1 = Utility.ConvertCollider(collider1);
            Collider2D internalCollider2 = Utility.ConvertCollider(collider2);

            if ((internalCollider1 == null) || (internalCollider2 == null))
            {
                Debug.Assert(false, "[MPR] Invalid internal colliders");
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

            if (!Utility.IsIntersect2D(collider1.bounds, collider2.bounds))
                return false;

            return IsIntersectInternal(collider1, collider2);
        }

        static public bool IsIntersect(Collider2D collider1, UnityEngine.Collider2D collider2)
        {
            if ((collider1 == null) || (collider2 == null))
            {
                Debug.Assert(false, "[MPR] Invalid colliders");
                return false;
            }

            if (!Utility.IsIntersect2D(collider1.bounds, collider2.bounds))
                return false;

            Collider2D internalCollider2 = Utility.ConvertCollider(collider2);

            if (internalCollider2 == null)
            {
                Debug.Assert(false, "[MPR] Invalid internal collider");
                return false;
            }

            return IsIntersectInternal(collider1, internalCollider2);
        }
        #endregion

        #region MPR 2D
        static private bool IsIntersectInternal(Collider2D collider1, Collider2D collider2)
        {
            Vector2 v0 = collider2.GetCenter() - collider1.GetCenter();
            Debug.Assert(v0.sqrMagnitude > 0.0f, "[MPR] Invalid  V0");

            Vector2 v1 = collider2.GetSupport(-v0) - collider1.GetSupport(v0);
            Debug.Assert(Vector2.Distance(v0, v1) > 0.0f, "[MPR] Invalid  V0 and V1");

            Vector2 n = Utility.Perpendicular(v0, v1);
            Vector2 v2 = collider2.GetSupport(n) - collider1.GetSupport(-n);

            const int maxIterationsCount = 10;
            for (int i = 0; i < maxIterationsCount; ++i)
            {
                Vector2 portal = v2 - v1;

                float side1 = Utility.Cross(portal, v0 - v1);
                float side2 = Utility.Cross(portal, -v1);
                if (side1 * side2 >= 0.0f)
                    return true;

                n = Utility.Perpendicular(v1, v2);
                Debug.Assert(v0.sqrMagnitude > 0.0f, "[MPR] Invalid  portal's normal");
                Vector2 v3 = collider2.GetSupport(n) - collider1.GetSupport(-n);
                if (Vector2.Dot(v3, n) < 0.0f)
                    return false;

                side1 = Utility.Cross(v3 - v0, -v0);
                side2 = Utility.Cross(v1 - v0, -v0);
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