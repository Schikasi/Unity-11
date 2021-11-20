using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Game;
using Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Input
{
    public class ZBotPlayerInput : PlayerInput
    {
        [SerializeField] private LevelMap levelMap;


        private Vector3[] _path;

        private int _currentIndexPath;
        private float maxX;
        private float minX;
        private float maxZ;
        private float minZ;

        //private Vector3 _targetPosition;


        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var playerPosition = transform.position;

            if (Vector3.Distance(playerPosition, _path[_currentIndexPath]) <= 0.1f)
            {
                if (++_currentIndexPath >= _path.Length)
                {
                    CalculeteNewPath(playerPosition);
                    _currentIndexPath = 0;
                }
            }

            var moveDirection = _path[_currentIndexPath] - playerPosition;
            return (moveDirection, Quaternion.LookRotation(moveDirection), false);
        }

        private void CalculeteNewPath(Vector3 playerPosition)
        {
            Vector3 targetPosition;
            var nPath = new NavMeshPath();
            do
            {
                targetPosition = FindNewTargetPosition();
            } while (!NavMesh.CalculatePath(playerPosition, targetPosition, NavMesh.AllAreas, nPath));

            _path = nPath.corners;

            for (int i = 0; i < _path.Length; i++)
                _path[i].y = 0f;
        }

        private Vector3 FindNewTargetPosition()
        {
            NavMeshHit hit;

            do
            {
                var point = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));

                while (!NavMesh.SamplePosition(point, out hit, 2f, NavMesh.AllAreas))
                {
                    point = new Vector2(Random.Range(minX, maxX), Random.Range(minZ, maxZ));
                }
            } while (Vector3.Distance(transform.position, hit.position) <= 1f);

            return hit.position;
        }

        private void Awake()
        {
            maxX = levelMap.Points.Max(p => Mathf.RoundToInt(p.x));
            minX = levelMap.Points.Min(p => Mathf.RoundToInt(p.x));

            maxZ = levelMap.Points.Max(p => Mathf.RoundToInt(p.z));
            minZ = levelMap.Points.Min(p => Mathf.RoundToInt(p.z));

            _path = new[] {transform.position};
            _currentIndexPath = 0;
        }
    }
}