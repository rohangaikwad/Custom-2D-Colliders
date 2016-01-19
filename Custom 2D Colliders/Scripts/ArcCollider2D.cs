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

[AddComponentMenu("Physics 2D/Arc Collider 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class ArcCollider2D : MonoBehaviour {

    [Range(1, 25)]
    public float radius = 3;
    [Range(10,90)]
    public int smoothness = 24;

    [Range(10, 360)]
    public int totalAngle = 360;

    [Range(0, 360)]
    public int offsetRotation = 0;

    [Header("Let there be Pizza")]
    public bool pizzaSlice;
    
    Vector2 origin, center;
    
    public Vector2[] getPoints(Vector2 off)
    {
        List<Vector2> points = new List<Vector2>();

        origin = transform.localPosition;
        center = origin + off;
        
        float ang = offsetRotation;

        if (pizzaSlice && totalAngle % 360 != 0) points.Add(center);

        for (int i = 0; i <= smoothness; i++)
        {
            float x = center.x + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            float y = center.y + radius * Mathf.Sin(ang * Mathf.Deg2Rad);

            points.Add(new Vector2(x, y));
            ang += (float)totalAngle/smoothness;
        }

        if (pizzaSlice && totalAngle % 360 != 0) points.Add(center);

        return points.ToArray();
    }
}
#endif