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

using UnityEngine;
using System.Collections;

namespace UnityEditor
{

    [CustomEditor(typeof(BezierCurveCollider2D))]
    public class BezierCurveCollider_Editor : Editor
    {

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
                bc.addControlPoint();
            }

            if (bc.controlPoints.Count > 2) // minimum 2 control points are always required
            {
                if (GUILayout.Button("Remove Point"))
                {
                    bc.removeControlPoint();
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
                    if (bc.controlPoints.Count == 2)
                    {
                        bc.handlerPoints[i] += offset;
                    }
                    // if there are more than 2 control points
                    else if (bc.controlPoints.Count > 2)
                    {
                        // if you moved the first control point
                        if (i == 0)
                        {
                            bc.handlerPoints[0] += offset; // offset the handle
                        }
                        // if you moved the last control point
                        else if (i == bc.controlPoints.Count - 1)
                        {
                            bc.handlerPoints[bc.handlerPoints.Count - 1] += offset; // offset the handle
                        }
                        // if you moved one of the other control points in the middle
                        else
                        {
                            int ind = (i * 2) - 1;
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
                for (int i = 0; i < bc.handlerPoints.Count; i = i + 2)
                {
                    Handles.DrawLine(bc.handlerPoints[i], bc.controlPoints[c]);
                    Handles.DrawLine(bc.handlerPoints[i + 1], bc.controlPoints[c + 1]);
                    c++;
                }
            }


            if (GUI.changed)
            {
                bc.drawCurve();
            }
        }

    }
}