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
    public int smoothness = 15;

    [Range(.2f, 25)]
    public float height = 2;

    [Range(.2f, 25)]
    public float width = 2;

    [HideInInspector]
    public float radius = .5f, wt, wb;
    
    [Range(0.05f, .95f)]
    public float trapezoid = .5f;

    [HideInInspector]
    public Vector2 offset, center, center1, center2, center3, center4;
    float ang = 0;
    List<Vector2> points;

    public Vector2[] getPoints(Vector2 off)
    {
        points = new List<Vector2>();
        points.Clear();
        
        offset = off;
        center = transform.localPosition;
        center += offset;

        wt = (width + width) - ((width + width) * trapezoid);   // width top
        wb = (width + width) * trapezoid;                       // width bottom
        
        // vertices
        Vector2 vTR = new Vector2(center.x + (wt / 2f), center.y + (height / 2f)); // top right vertex
        Vector2 vTL = new Vector2(center.x + (wt /-2f), center.y + (height / 2f)); // top left vertex
        Vector2 vBL = new Vector2(center.x + (wb /-2f), center.y - (height / 2f)); // bottom left vertex
        Vector2 vBR = new Vector2(center.x + (wb / 2f), center.y - (height / 2f)); // bottom right vertex

        Vector2 dir = vBL - vTL;
        float hypAngleTL = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // hypertenuse angle top left corner
        hypAngleTL = (hypAngleTL + 360) % 360; // get it between 0-360 range
        hypAngleTL = 360 - hypAngleTL; // use the inside angle
        hypAngleTL /= 2f; // got our adjacent angle
        ///        
        ///    adj TL (Top Left)
        ///    _____
        ///    \    |
        ///     \   |
        ///   h  \  | opp = radius
        ///       \ |
        ///        \|
        /// 
        float adjTL = radius / Mathf.Tan(hypAngleTL*Mathf.Deg2Rad);
        center1 = new Vector3(vTR.x - adjTL, vTR.y - radius, 0);
        center2 = new Vector3(vTL.x + adjTL, vTL.y - radius, 0);



        float hypAngleBL = (180 - hypAngleTL * 2f) / 2f; // hypertenuse angle bottom left corner
        /// 
        ///        /|
        ///       / |
        ///   h  /  | opp = radius
        ///     /   |
        ///    /____|
        /// 
        ///     adj BL (Bottom Left)
        /// 
        float adjBL = radius / Mathf.Tan(hypAngleBL * Mathf.Deg2Rad);
        center3 = new Vector3(vBL.x + adjBL, vBL.y + radius, 0);
        center4 = new Vector3(vBR.x - adjBL, vBR.y + radius, 0);


        // prevent overlapping of the corners
        center1.x = Mathf.Max(center.x, center1.x);
        center2.x = Mathf.Min(center.x, center2.x);
        center3.x = Mathf.Min(center.x, center3.x);
        center4.x = Mathf.Max(center.x, center4.x);


        // curveTOP angles
        Vector2 tmpDir = vBR - vTR;
        float tmpAng = Mathf.Atan2(tmpDir.y, tmpDir.x) * Mathf.Rad2Deg;
        tmpAng = (tmpAng + 360) % 360;
        float x = vTR.x + adjTL * Mathf.Cos(tmpAng * Mathf.Deg2Rad);
        float y = vTR.y + adjTL * Mathf.Sin(tmpAng * Mathf.Deg2Rad);
        Vector2 startPos = new Vector2(x,y);

        bool canPlot = (Vector2.Distance(startPos, center1) >= radius * .85f) ? true : false;
        if (!canPlot) return null;

        tmpDir = startPos - center1;
        tmpAng = Mathf.Atan2(tmpDir.y, tmpDir.x) * Mathf.Rad2Deg;
        tmpAng = (tmpAng + 360) % 360;

        float t = (tmpAng > 180) ? tmpAng - 360 : tmpAng;

        ang = tmpAng;
        float totalAngle = (t < 0) ? 90f - t : 90f-tmpAng;        
        calcPoints(center1, totalAngle);
        calcPoints(center2, totalAngle);



        // curveBottom angles
        tmpDir = vTL - vBL;
        tmpAng = Mathf.Atan2(tmpDir.y, tmpDir.x) * Mathf.Rad2Deg;
        tmpAng = (tmpAng + 360) % 360;
        x = vBL.x + adjBL * Mathf.Cos(tmpAng * Mathf.Deg2Rad);
        y = vBL.y + adjBL * Mathf.Sin(tmpAng * Mathf.Deg2Rad);
        startPos = new Vector2(x, y);

        canPlot = (Vector2.Distance(startPos, center3) >= radius * .9f) ? true : false;
        if (!canPlot) return null;

        tmpDir = startPos - center3;
        tmpAng = Mathf.Atan2(tmpDir.y, tmpDir.x) * Mathf.Rad2Deg;
        tmpAng = (tmpAng + 360) % 360;

        ang = tmpAng;
        totalAngle = 270 - tmpAng;
        calcPoints(center3, totalAngle);
        calcPoints(center4, totalAngle);




        points.Add(points[0]);
        return points.ToArray();
    }



    void calcPoints(Vector2 ctr, float totAngle)
    {
        for (int i = 0; i <= smoothness; i++)
        {
            float a = ang * Mathf.Deg2Rad;
            float x = ctr.x - offset.x + radius * Mathf.Cos(a);
            float y = ctr.y - offset.y + radius * Mathf.Sin(a);

            points.Add(new Vector2(x, y));
            ang += totAngle / smoothness;
        }

        ang -= 90f / smoothness;
    }
}
#endif