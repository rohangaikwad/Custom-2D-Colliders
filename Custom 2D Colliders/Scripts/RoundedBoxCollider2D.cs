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

[AddComponentMenu("Physics 2D/Rounded Box Collider 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class RoundedBoxCollider2D : MonoBehaviour {


    [Range(10, 90)]
    public int smoothness = 10;

    [Range(.2f, 25)]
    public float height = 2;

    [Range(.2f, 25)]
    public float width = 2;

    [HideInInspector]
    public float radius = .5f;

    Vector2 origin, center, center1, center2, center3, center4;
    float ang = 0;
    List<Vector2> points;

    public Vector2[] getPoints(Vector2 off)
    {
        points = new List<Vector2>();
        points.Clear();

        origin = transform.localPosition;
        center = origin + off;

        center1.x = center.x + (width / 2f) - radius;
        center1.y = center.y + (height / 2f) - radius;

        center2.x = center.x - (width / 2f) + radius;
        center2.y = center.y + (height / 2f) - radius;

        center3.x = center.x - (width / 2f) + radius;
        center3.y = center.y - (height / 2f) + radius;

        center4.x = center.x + (width / 2f) - radius;
        center4.y = center.y - (height / 2f) + radius;

        ang = 0;

        calcPoints(center1, true);
        calcPoints(center2, true);
        calcPoints(center3, true);
        calcPoints(center4, false);

        points.Add(points[0]);

        return points.ToArray();
    }

    void calcPoints(Vector2 ctr, bool decrement)
    {
        for (int i = 0; i <= smoothness; i++)
        {
            float a = ang * Mathf.Deg2Rad;
            float x = ctr.x + radius * Mathf.Cos(a);
            float y = ctr.y + radius * Mathf.Sin(a);

            points.Add(new Vector2(x, y));
            ang += 90f / smoothness;
        }

        if(decrement) ang -= 90f / smoothness;
    }
}
#endif