using System;
using Game;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(LevelMap))]
    public class MapEditor : UnityEditor.Editor
    {
        [SerializeField] private GameObject prefab;

        private GameObject go;
        private GameObject walls;
        private GameObject floor;

        private void OnEnable()
        {
            go = ((LevelMap) this.target).gameObject;
            walls = go.transform.Find("Wall")?.gameObject;
            floor = go.transform.Find("Floor")?.gameObject;
            if (walls == null)
            {
                walls = new GameObject("Wall");
                walls.transform.SetParent(go.transform);
            }

            if (floor == null)
            {
                floor = new GameObject("Floor");
                floor.transform.SetParent(go.transform);
            }
        }

        private void OnSceneGUI()
        {
            switch (Event.current.type)
            {
                case EventType.Layout:
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(GetHashCode(), FocusType.Passive));
                    break;
                case EventType.MouseDown:
                case EventType.MouseDrag:
                {
                    if (!Event.current.control)
                        DrawMap();
                    else
                        ErraseMap();
                    break;
                }
            }
        }

        private void ErraseMap()
        {
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000.0f))
            {
                if (!hit.collider.gameObject.name.Equals("Wall"))
                    return;
                DestroyImmediate(hit.collider.gameObject);
            }
        }

        private void DrawMap()
        {
            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out var hit, 1000.0f))
            {
                if (!hit.collider.gameObject.Equals(floor))
                    return;
                var point = Vector3Int.RoundToInt(hit.point + hit.normal * 0.9f);
                if (Physics.OverlapBox(point, new Vector3(0.45f, 0.45f, 0.45f)).Length == 0)
                {
                    var wall = PrefabUtility.InstantiatePrefab(prefab, walls.transform) as GameObject;
                    wall.transform.position = point;
                }
            }
        }
    }
}