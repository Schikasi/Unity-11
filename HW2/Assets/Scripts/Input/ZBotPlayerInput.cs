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

        private Vector3 _targetPosition;


        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var playerPosition = transform.position;

            if (Vector3.Distance(playerPosition, _targetPosition) <= 0.1f)
            {
                _targetPosition = FindNewTargetPosition();
                var nPath = new NavMeshPath();
                while(!NavMesh.CalculatePath(playerPosition, _targetPosition, NavMesh.AllAreas, nPath));
                _path = nPath.corners;

                for (int i = 0; i < _path.Length; i++)
                    _path[i].y = 0f;
                _targetPosition.y = 0f;
                
                _currentIndexPath = 0;
            }

            if (Vector3.Distance(playerPosition, _path[_currentIndexPath]) <= 0.1f)
                ++_currentIndexPath;

            var moveDirection = _path[_currentIndexPath] - playerPosition;
            moveDirection.y = 0f;


            return (moveDirection, Quaternion.LookRotation(moveDirection), false);
        }

        private Vector3 FindNewTargetPosition()
        {
            NavMeshHit hit;
            var maxX = levelMap.Points.Max(p => Mathf.RoundToInt(p.x));
            var minX = levelMap.Points.Min(p => Mathf.RoundToInt(p.x));

            var maxZ = levelMap.Points.Max(p => Mathf.RoundToInt(p.z));
            var minZ = levelMap.Points.Min(p => Mathf.RoundToInt(p.z));

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
            _targetPosition = transform.position;
        }
    }
}