/*
The MIT License (MIT)

Copyright (c) 2016 GuyQuad

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

You can contact me by email at guyquad27@gmail.com or on Reddit at https://www.reddit.com/user/GuyQuad
*/


#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Physics 2D/Ellipse Collider 2D")]

[RequireComponent(typeof(PolygonCollider2D))]
public class EllipseCollider2D : MonoBehaviour {

    [Range(1, 25)]
    public float radiusX = 1, radiusY = 2;

    [Range(10,90)]
    public int smoothness = 30;

    [Range(0, 180)]
    public int rotation = 0;
    
    Vector2 origin, center;
    
    public Vector2[] getPoints()
    {
        List<Vector2> points = new List<Vector2>();
        
        float ang = 0;
        float o = rotation * Mathf.Deg2Rad;

        for (int i = 0; i <= smoothness; i++)
        {
            float a = ang * Mathf.Deg2Rad;

            // fan shuriken
            //float radius;
            //float radX = 90 - (Mathf.Abs(ang) % 90);
            //float radY = 90 - radX;
            //radius = ((radiusX * radX / 90f) + (radiusY * radY / 90f)) / 2f;
            //float x = center.x + radius * Mathf.Cos(a);
            //float y = center.y + radius * Mathf.Sin(a);

            // https://www.uwgb.edu/dutchs/Geometry/HTMLCanvas/ObliqueEllipses5a.HTM
            float x = radiusX * Mathf.Cos(a) * Mathf.Cos(o) - radiusY * Mathf.Sin(a) * Mathf.Sin(o);
            float y = -radiusX * Mathf.Cos(a) * Mathf.Sin(o) - radiusY * Mathf.Sin(a) * Mathf.Cos(o);

            points.Add(new Vector2(x, y));
            ang += 360f/smoothness;
        }

        points.RemoveAt(0);
        return points.ToArray();
    }
}
#endif
