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

        [SerializeField] private GameObject shooter;
        [SerializeField] private BulletController bullet;
        [SerializeField] private float visionDistance = 25f;

        private List<Vector2Int> _path;
        private int _currentIndexPath;
        private Vector2Int _targetMapPosition;
        private GameObject _targetZombie = null;

        private int[,] _map;
        private int _deltaX;
        private int _deltaZ;

        public override (Vector3 moveDirection, Quaternion viewDirection, bool shoot) CurrentInput()
        {
            // Выбор новой цели
            if (_targetZombie == null)
                _targetZombie = FindNewTarget();
            else if (!_targetZombie.GetComponentInParent<ZombieComponent>().IsAlive)
                _targetZombie = FindNewTarget();
            //Если новая цель отсуствует - стоим
            if (_targetZombie == null)
                return (Vector3.zero, Quaternion.identity, false);

            var playerWorldPosition = transform.position;
            var zombieWorldPosition = _targetZombie.transform.position;
            var playerMapPosition = ToInt(playerWorldPosition);
            var zombieMapPosition = ToInt(zombieWorldPosition);

            //Если зомби ушёл от места до которого рассчитан путь более чем на 1 клетку - перерасчитываем путь
            if (Vector2Int.Distance(_targetMapPosition, zombieMapPosition) > 1f)
            {
                _targetMapPosition = zombieMapPosition;
                _path = AStarFromGoogle.FindPath(_map, playerMapPosition, _targetMapPosition);
                _currentIndexPath = 0;
            }

            //Вычисляем вектор стрельбы в зомби (без упреждения)
            var shootDirection = ShootDirection(_targetZombie);
            if (shootDirection.Equals(Vector3.zero))
            {
                var shooterTarget = FindShootTarget();
                if (shooterTarget != null)
                    shootDirection = ShootDirection(shooterTarget);
            }

            var shoot = shootDirection != Vector3.zero;

            if (playerMapPosition == _targetMapPosition)
                return (Vector3.zero, Quaternion.LookRotation(shootDirection), shoot);

            if (playerMapPosition == _path[_currentIndexPath])
                ++_currentIndexPath;
            var moveDirection = new Vector3(_path[_currentIndexPath].x - _deltaX, playerWorldPosition.y,
                _path[_currentIndexPath].y - _deltaZ) - playerWorldPosition;

            var viewDirection = shoot ? shootDirection : moveDirection;
            viewDirection.y = 0;
            moveDirection = !shoot || !(Vector3.Distance(playerWorldPosition, zombieWorldPosition) <= 3f)
                ? moveDirection
                : Vector3.zero;
            return (moveDirection, Quaternion.LookRotation(viewDirection), shoot);
        }

        private GameObject FindShootTarget()
        {
            var targets = Physics.OverlapSphere(transform.position, visionDistance, 1 << 16);
            foreach (var target in targets)
            {
                var go = target.gameObject;
                var zombieComponent = target.transform.GetComponentInParent<ZombieComponent>();
                if (zombieComponent == null || !zombieComponent.IsAlive) continue;
                if (ShootDirection(go) != Vector3.zero) return go;
            }

            return null;
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
                if (ShootDirection(go) != Vector3.zero) return go;

                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > minDistance) continue;

                minDistance = distance;
                newTarget = go;
            }

            return newTarget;
        }

        private Vector3 ShootDirection(GameObject target)
        {
            var targetPosition = target.transform.position;
            var shooterPosition = transform.position + Vector3.up;
            Debug.DrawRay(shooterPosition, (targetPosition - shooterPosition), Color.red, 1f);
            var direction = targetPosition - shooterPosition;
            var ray = new Ray(shooterPosition, targetPosition - shooterPosition);
            var bulletScale = bullet.transform.localScale;
            if (!Physics.Raycast(ray, out var hit1, bullet.Speed * bullet.LifeTime, 1 << 16))
            {
                return Vector3.zero;
            }

            if (Physics.SphereCast(ray, bulletScale.x/2, out var hit2, hit1.distance, ~(1 << 16)))
            {
                return Vector3.zero;
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