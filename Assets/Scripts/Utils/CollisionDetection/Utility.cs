using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CollisionDetection
{
    public static class Utility
    {
        static private bool IsLessEqual(Vector2 v1, Vector2 v2)
        {
            return (v1.x <= v2.x) && (v1.y <= v2.y);
        }

        static public float Cross(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        static public Vector2 Perpendicular(Vector2 v1, Vector2 v2)
        {
            Vector2 a = v2 - v1;
            Vector2 b = -v1;

            Vector2 r = Vector2.Perpendicular(a);

            if (Vector2.Dot(r, b) < 0.0f)
                r *= -1.0f;

            return r;
        }

        static public bool IsIntersect2D(Bounds bounds1, Bounds bounds2)
        {
            return IsLessEqual(bounds1.min, bounds2.max) && IsLessEqual(bounds2.min, bounds1.max);
        }

        static public Collider2D ConvertCollider(UnityEngine.Collider2D collider)
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
    }
}