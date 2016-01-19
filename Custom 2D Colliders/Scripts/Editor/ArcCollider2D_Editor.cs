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

[CustomEditor (typeof(ArcCollider2D))]
public class ArcCollider_Editor : Editor {
    
    ArcCollider2D ac;
    EdgeCollider2D edgeCollider;
    Vector2 off;

    void OnEnable()
    {
        ac = (ArcCollider2D)target;

        edgeCollider = ac.GetComponent<EdgeCollider2D>();
        if (edgeCollider == null) {
            ac.gameObject.AddComponent<EdgeCollider2D>();
            edgeCollider = ac.GetComponent<EdgeCollider2D>();
        }
        edgeCollider.points = ac.getPoints(edgeCollider.offset);
    }

    public override void OnInspectorGUI()
    {
        GUI.changed = false;
        DrawDefaultInspector();

        if (GUI.changed || !off.Equals(edgeCollider.offset))
        {
            edgeCollider.points = ac.getPoints(edgeCollider.offset);
        }
        off = edgeCollider.offset;
    }
    
}