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