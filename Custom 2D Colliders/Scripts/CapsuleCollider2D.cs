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

[AddComponentMenu("Physics 2D/Capsule Collider 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class CapsuleCollider2D : MonoBehaviour {

    [Range(.5f, 25)]
    public float radius = 1;

    [Range(1, 25)]
    public float height = 4;

    [Range(10,90)]
    public int smoothness = 20;

    [Range(0, 180)]
    public int rotation = 0;
    
    Vector2 origin, center, center1, center2;
    
    public Vector2[] getPoints(Vector2 off)
    {
        List<Vector2> points = new List<Vector2>();

        origin = transform.localPosition;
        center = origin + off;

        float r = (height / 2f) - (radius);
        
        center1.x = center.x + r * Mathf.Sin(rotation * Mathf.Deg2Rad);
        center1.y = center.y + r * Mathf.Cos(rotation * Mathf.Deg2Rad);

        center2.x = center.x + r * Mathf.Sin((rotation + 180f) * Mathf.Deg2Rad);
        center2.y = center.y + r * Mathf.Cos((rotation + 180f) * Mathf.Deg2Rad);

        float ang = 360 - rotation;

        for (int i = 0; i <= smoothness/2; i++)
        {
            float a = ang * Mathf.Deg2Rad;
            float x = center1.x + radius * Mathf.Cos(a);
            float y = center1.y + radius * Mathf.Sin(a);

            points.Add(new Vector2(x, y));
            ang += 360f/smoothness;
        }

        ang -= 360f / smoothness;


        for (int i = smoothness/2; i <= smoothness; i++)
        {
            float a = ang * Mathf.Deg2Rad;
            float x = center2.x + radius * Mathf.Cos(a);
            float y = center2.y + radius * Mathf.Sin(a);

            points.Add(new Vector2(x, y));
            ang += 360f / smoothness;
        }

        points.Add(points[0]);

        return points.ToArray();
    }
}
#endif