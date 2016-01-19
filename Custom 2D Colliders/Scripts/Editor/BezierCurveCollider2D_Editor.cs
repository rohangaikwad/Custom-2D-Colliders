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


using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(BezierCurveCollider2D))]
public class BezierCurveCollider_Editor : Editor {

    BezierCurveCollider2D bc;

    void OnEnable()
    {
        bc = (BezierCurveCollider2D)target;

        if (!bc.initialized) bc.Init();
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();

        if (!bc.edge.offset.Equals(Vector2.zero)) bc.edge.offset = Vector2.zero; // prevent changes to offset

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Point"))
        {
            bc.addPoint();
        }

        if (bc.controlPoints.Count > 2) // minimum 2 control points are always required
        {
            if (GUILayout.Button("Remove Point"))
            {
                bc.removePoint();
            }
        }

        EditorGUILayout.EndHorizontal();


        if (GUILayout.Button("Reset"))
        {
            bc.initialized = false;
            bc.Init();
        }

        if (GUI.changed)
        {
            bc.drawCurve();
        }
    }

    void OnSceneGUI()
    {
        GUI.changed = false;
        Handles.color = Color.yellow;

        // manage control points
        for (int i = 0; i < bc.controlPoints.Count; i++)
        {
            Vector3 start = bc.controlPoints[i];
            Vector3 newPos = Handles.FreeMoveHandle(bc.controlPoints[i], Quaternion.identity, .25f, Vector3.zero, Handles.ConeCap);
            bc.controlPoints[i] = newPos;

            // if the control point was moved.. offset the joining handler points
            if (!start.Equals(newPos))
            {
                Vector2 offset = newPos - start;

                // if there are only 2 control points
                if(bc.controlPoints.Count == 2)
                {
                    bc.handlerPoints[i] += offset;
                }
                // if there are more than 2 control points
                else if (bc.controlPoints.Count > 2)
                {
                    // if you moved the first control point
                    if(i == 0)
                    {
                        bc.handlerPoints[0] += offset; // offset the handle
                    }
                    // if you moved the last control point
                    else if (i == bc.controlPoints.Count - 1)
                    {
                        bc.handlerPoints[bc.handlerPoints.Count-1] += offset; // offset the handle
                    }
                    // if you moved one of the other control points in the middle
                    else
                    {
                        int ind= (i * 2) - 1;
                        bc.handlerPoints[ind] += offset; // offset the top handle
                        bc.handlerPoints[++ind] += offset; // offset the bottom handle

                    }
                }
            }
        }


        // manage handler points
        // when using continous curves
        if (!bc.continous)
        {
            for (int i = 0; i < bc.handlerPoints.Count; i++)
            {
                bc.handlerPoints[i] = Handles.FreeMoveHandle(bc.handlerPoints[i], Quaternion.identity, .5f, Vector3.zero, Handles.ConeCap);
            }
        }
        else
        // when using non-continous curves
        {
            for (int i = 0; i < bc.handlerPoints.Count; i++)
            {
                // if there are only 2 control points
                if (bc.controlPoints.Count == 2)
                {
                    bc.handlerPoints[i] = Handles.FreeMoveHandle(bc.handlerPoints[i], Quaternion.identity, .5f, Vector3.zero, Handles.ConeCap);
                }
                // if there are more than 2 control points
                else if (bc.controlPoints.Count > 2)
                {
                    // no additional calculations required for the first and last handler points
                    if (i == 0 || i == bc.handlerPoints.Count - 1)
                    {
                        bc.handlerPoints[i] = Handles.FreeMoveHandle(bc.handlerPoints[i], Quaternion.identity, .5f, Vector3.zero, Handles.ConeCap);
                    }
                    else
                    {
                        // changes for the rest of the handler points in the middle
                        Vector3 start = bc.handlerPoints[i];
                        Vector3 newPos = Handles.FreeMoveHandle(bc.handlerPoints[i], Quaternion.identity, .5f, Vector3.zero, Handles.ConeCap);
                        bc.handlerPoints[i] = newPos;

                        if (!start.Equals(newPos))
                        {
                            bool movedTop = (i % 2 == 1) ? true : false;

                            // if we are moving the top handle
                            if (movedTop)
                            {
                                int cp = (i + 1) / 2; // get the control point for this handle
                                // calc angle of the top handle
                                Vector2 dir = bc.handlerPoints[i] - bc.controlPoints[cp]; 
                                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                                angle = (angle + 360) % 360;

                                // adjust the angle of the bottom handle
                                float magH2 = Vector2.Distance(bc.controlPoints[cp], bc.handlerPoints[i + 1]);
                                angle = 270 - angle;

                                float x = bc.controlPoints[cp].x + magH2 * Mathf.Sin(angle * Mathf.Deg2Rad);
                                float y = bc.controlPoints[cp].y + magH2 * Mathf.Cos(angle * Mathf.Deg2Rad);

                                bc.handlerPoints[i + 1] = new Vector2(x, y);

                            }
                            else
                            //if we are moving the bottom handle
                            {
                                int cp = i / 2; // get the control point for this handle
                                // calc the angle of the bottom angle
                                Vector2 dir = bc.controlPoints[cp] - bc.handlerPoints[i];
                                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                                angle = (angle + 360) % 360;

                                // adjust the angle of top handle
                                float magH2 = Vector2.Distance(bc.controlPoints[cp], bc.handlerPoints[i - 1]);
                                angle = 360 - angle + 90;

                                float x = bc.controlPoints[cp].x + magH2 * Mathf.Sin(angle * Mathf.Deg2Rad);
                                float y = bc.controlPoints[cp].y + magH2 * Mathf.Cos(angle * Mathf.Deg2Rad);

                                bc.handlerPoints[i - 1] = new Vector2(x, y);
                            }
                        }
                    }
                }
            }
        }



        // draw a line from the control point to handler points
        if (bc.handlerPoints.Count == 2)
        {
            Handles.DrawLine(bc.handlerPoints[0], bc.controlPoints[0]);
            Handles.DrawLine(bc.handlerPoints[1], bc.controlPoints[1]);
        }
        else
        {
            int c = 0;
            for (int i = 0; i < bc.handlerPoints.Count; i = i+2)
            {
                Handles.DrawLine(bc.handlerPoints[i], bc.controlPoints[c]);
                Handles.DrawLine(bc.handlerPoints[i+1], bc.controlPoints[c+1]);
                c++;
            }
        }


        if (GUI.changed)
        {
            bc.drawCurve();
        }
    }

}