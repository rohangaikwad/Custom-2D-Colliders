///  Custom 2D Colliders
///  Description: A set of useful custom 2d colliders creation scripts to use in your game.
///  Copyright(C) 2016 GuyQuad

///  This program is free software: you can redistribute it and/or modify
///  it under the terms of the GNU General Public License as published by
///  the Free Software Foundation, either version 3 of the License, or
///  any later version.

///  This program is distributed in the hope that it will be useful,
///  but WITHOUT ANY WARRANTY; without even the implied warranty of
///  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
///  GNU General Public License for more details.

///  You should have received a copy of the GNU General Public License
///  along with this program.If not, see<http://www.gnu.org/licenses/>.
///  You can contact me by email at guyquad27@gmail.com or on Reddit at https://www.reddit.com/user/GuyQuad


#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Physics 2D/Ellipse Collider 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class EllipseCollider2D : MonoBehaviour {

    [Range(1, 25)]
    public float radiusX = 1, radiusY = 2;

    [Range(10,90)]
    public int smoothness = 30;

    [Range(0, 180)]
    public int rotation = 0;
    
    Vector2 origin, center;
    
    public Vector2[] getPoints(Vector2 off)
    {
        List<Vector2> points = new List<Vector2>();

        origin = transform.localPosition;
        center = origin + off;
        
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
            float x = center.x + radiusX * Mathf.Cos(a) * Mathf.Cos(o) - radiusY * Mathf.Sin(a) * Mathf.Sin(o);
            float y = center.y - radiusX * Mathf.Cos(a) * Mathf.Sin(o) - radiusY * Mathf.Sin(a) * Mathf.Cos(o);

            points.Add(new Vector2(x, y));
            ang += 360f/smoothness;
        }

        return points.ToArray();
    }
}
#endif