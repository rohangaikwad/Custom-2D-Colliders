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


[AddComponentMenu("Physics 2D/Bezier Curve Collider 2D")]

[RequireComponent(typeof(EdgeCollider2D))]
public class BezierCurveCollider2D : MonoBehaviour {

    public List<Vector2> controlPoints, handlerPoints;

    [Range(3,36)]
    public int smoothness = 15;
        
    Vector2 origin, center;

    [HideInInspector]
    public bool initialized, openCustomize;

    public bool continous = true;

    [HideInInspector]
    public EdgeCollider2D edge;
    List<Vector2> pts;
    

    public void Init()
    {
        if (initialized) return;
        initialized = true;
        continous = true;
        smoothness = 15;

        controlPoints = new List<Vector2>();
        handlerPoints = new List<Vector2>();

        controlPoints.Clear();
        handlerPoints.Clear();

        Vector2 pos = transform.localPosition;        
        controlPoints.Add(pos);

        pos.x += 4;
        controlPoints.Add(pos);

        pos.x -= 4;
        pos.y += 4;
        handlerPoints.Add(pos);

        pos.x += 4;
        handlerPoints.Add(pos);

        drawCurve();
    }
    
    public void drawCurve()
    {
        pts = new List<Vector2>();
        pts.Clear();

        edge = GetComponent<EdgeCollider2D>();
        if (edge == null)
        {
            gameObject.AddComponent<EdgeCollider2D>();
        }

        if (controlPoints.Count == 2)
        {
            drawSegment(controlPoints[0], controlPoints[1], handlerPoints[0], handlerPoints[1]);
        } else if (controlPoints.Count > 2)
        {
            int h = 0;
            for(int i = 0; i < controlPoints.Count - 1; i++)
            {
                drawSegment(controlPoints[i], controlPoints[i+1], handlerPoints[h], handlerPoints[h+1]);
                h += 2;
            }
        }


        edge.points = pts.ToArray();

    }

    void drawSegment(Vector3 cPt1, Vector3 cPt2, Vector3 hPt1, Vector3 hPt2)
    {

        pts.Add(cPt1);
        for (int i = 1; i < smoothness; i++)
        {
            pts.Add(CalculateBezierPoint((1f / smoothness) * i, cPt1, hPt1, hPt2, cPt2));
        }
        pts.Add(cPt2);
    }

    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 handlerP0, Vector3 handlerP1, Vector3 p1)
    {
        //http://devmag.org.za/2011/04/05/bzier-curves-a-tutorial/
        Vector3 p = (Mathf.Pow((1.0f - t), 3) * p0) + (3 * Mathf.Pow((1 - t), 2) * t * handlerP0) + (3 * (1.0f - t) * Mathf.Pow(t, 2) * handlerP1) + (Mathf.Pow(t, 3) * p1);
        return p;
    }

    public void addPoint()
    {
        Vector2 pos = controlPoints[controlPoints.Count - 1];
        float hPosY = handlerPoints[handlerPoints.Count - 1].y;

        float mul = (hPosY > pos.y) ? -1 : 1; // check if the handler point was below or top of the control point and use that info to make sure that the next handler point is in the opposite direction
        
        handlerPoints.Add(new Vector2(pos.x, pos.y + (4 * mul)));
        
        pos.x += 4;
        controlPoints.Add(pos);

        pos.y += 4 * mul;
        handlerPoints.Add(pos);

        drawCurve();
    }

    public void removePoint()
    {
        if(controlPoints.Count > 2)
        {
            controlPoints.RemoveAt(controlPoints.Count - 1);
            handlerPoints.RemoveAt(handlerPoints.Count - 1);
            handlerPoints.RemoveAt(handlerPoints.Count - 1);
        }
    }
}
#endif