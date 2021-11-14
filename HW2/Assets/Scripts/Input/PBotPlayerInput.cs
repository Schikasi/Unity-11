using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using Search;
using UnityEngine;
using UnityEngine.Serialization;

namespace Input
{
    public class PBotPlayerInput : PlayerInput
    {
        [FormerlySerializedAs("_levelMap")] [SerializeField]
        private LevelMap levelMap;

        [SerializeField] private BulletController bullet;
        [SerializeField] private float visionDistance = 25f;

        private List<Vector2Int> _path;
        private int _currentIndexPath;
        private Vector2Int _targetPosition;
        private GameObject _targetZombie;

        private int[,] _map;
        private int _deltaX;
        private int _deltaZ;

        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            var zc = _targetZombie?.GetComponent<ZombieComponent>();
            if (zc == null || !zc.IsAlive)
            {
                _targetZombie = FindNewTarget();
                if (_targetZombie == null)
                {
                    return (Vector3.zero, Quaternion.identity, false);
                }
            }

            if (Vector2Int.Distance(_targetPosition, ToInt(_targetZombie.transform.position)) > 1f)
            {
                _targetPosition = ToInt(_targetZombie.transform.position);
                _path = AStarFromGoogle.FindPath(_map, ToInt(transform.position), _targetPosition);
                _currentIndexPath = 0;
            }


            var playerPosition = transform.position;
            var zombiePosition = _targetZombie.transform.position;
            var directVector = CanShoot(_targetZombie);
            var shoot = directVector != Vector3.zero;

            if (ToInt(transform.position) == _targetPosition)
                return (Vector3.zero, Quaternion.LookRotation(directVector), shoot);

            if (ToInt(transform.position) == _path[_currentIndexPath])
                ++_currentIndexPath;
            var moveDirection = new Vector3(_path[_currentIndexPath].x - _deltaX, playerPosition.y,
                _path[_currentIndexPath].y - _deltaZ) - playerPosition;

            directVector = shoot ? directVector : moveDirection;
            moveDirection = !shoot || !(Vector3.Distance(playerPosition, zombiePosition) <= 3f)
                ? moveDirection
                : Vector3.zero;
            return (moveDirection, Quaternion.LookRotation(directVector), shoot);
        }

        private GameObject FindNewTarget()
        {
            var targets = Physics.OverlapSphere(transform.position, visionDistance, 1 << 16);
            GameObject newTarget = null;
            var minDistance = float.MaxValue;
            foreach (var target in targets)
            {
                var go = target.gameObject;
                var zombieComponent = target.transform.GetComponentInParent<ZombieComponent>();
                if (zombieComponent == null || !zombieComponent.IsAlive) continue;
                if (CanShoot(go) != Vector3.zero) return go;

                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > minDistance) continue;

                minDistance = distance;
                newTarget = go;
            }

            return newTarget;
        }

        private Vector3 CanShoot(GameObject target)
        {
            var targetPosition = target.transform.position;
            var playerPosition = transform.position;
            var direction = targetPosition - playerPosition;
            var ray = new Ray(playerPosition, targetPosition - playerPosition);
            var bulletScale = bullet.transform.localScale;
            if (!Physics.Raycast(ray, out var hit1, bullet.Speed * bullet.LifeTime, 1 << 16))
                return Vector3.zero;
            if (Physics.Raycast(ray, out var hit2, bullet.Speed * bullet.LifeTime))
            {
                return hit1.collider == hit2.collider ? direction : Vector3.zero;
            }

            return direction;
        }

        private Vector2Int ToInt(Vector3 vector3) =>
            new Vector2Int(_deltaX + Mathf.RoundToInt(vector3.x), _deltaZ + Mathf.RoundToInt(vector3.z));

        private void Awake()
        {
            CreateMap();
        }

        private void CreateMap()
        {
            var maxX = levelMap.Points.Max(p => Mathf.RoundToInt(p.x));
            var minX = levelMap.Points.Min(p => Mathf.RoundToInt(p.x));

            var maxZ = levelMap.Points.Max(p => Mathf.RoundToInt(p.z));
            var minZ = levelMap.Points.Min(p => Mathf.RoundToInt(p.z));

            _deltaX = minX < 0 ? -minX : 0;
            _deltaZ = minZ < 0 ? -minZ : 0;

            _map = new int[maxX + _deltaX + 1, maxZ + _deltaZ + 1];

            foreach (var point in levelMap.Points)
            {
                _map[_deltaX + Mathf.RoundToInt(point.x), _deltaZ + Mathf.RoundToInt(point.z)] = -1;
            }
        }
    }
}