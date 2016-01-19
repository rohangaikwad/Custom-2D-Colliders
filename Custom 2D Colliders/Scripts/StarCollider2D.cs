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

[AddComponentMenu("Physics 2D/Star Collider 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class StarCollider2D : MonoBehaviour {

    [Range(1, 25)]
    public float radiusA = 1;

    [Range(1, 25)]
    public float radiusB = 2;

    [Range(3,36)]
    public int points = 5;
    
    [HideInInspector]
    public int rotation = 0;
    
    Vector2 origin, center;
    
    public Vector2[] getPoints(Vector2 off)
    {
        List<Vector2> pts = new List<Vector2>();

        origin = transform.localPosition;
        center = origin + off;

        float ang = rotation;

        for (int i = 0; i <= points * 2; i++)
        {
            float radius = (i % 2 == 0) ? radiusA : radiusB;
            float x = center.x + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
            float y = center.y + radius * Mathf.Sin(ang * Mathf.Deg2Rad);

            pts.Add(new Vector2(x, y));
            ang += 360f/(points*2f);
        }

        pts.Add(pts[0]);

        return pts.ToArray();
    }
}
#endif