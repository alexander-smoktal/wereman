using System;

public class BoundingRect {
    public float left;
    public float bottom;
    public float right;
    public float top;

    public BoundingRect(float left, float right, float bottom, float top)
    {
        this.left = left;
        this.right = right;
        this.bottom = bottom;
        this.top = top;
    }

    public override String ToString()
    {
        return "[" + left.ToString() + ", " + right.ToString() + ", " + bottom.ToString() + ", " + top.ToString() + "]";
    }
}
