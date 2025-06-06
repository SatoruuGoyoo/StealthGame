using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPCSearch<T> : StatePathfinding<T>
{
    private Vector3 _lastPosition;
    private float _pauseTimer;
    private float _finalPauseTimer;

    private int _visitedPoints = 0;
    private const int _maxPoints = 4;

    private const float _pauseDuration = 2f;
    private const float _finalPauseDuration = 2f;
    private const float _stepDistance = 3f;

    private bool _isPaused = false;
    private bool _isSearchFinished = false;
    private bool _searchRequestCleared = false;

    private Transform _fakeTarget;
    private NPCMemory _memory;

    private List<Vector3> _searchPoints;
    private int _currentIndex = 0;

    private static Transform CreateFakeTarget()
    {
        var go = new GameObject("FakeTarget_Search");
        go.hideFlags = HideFlags.HideInHierarchy;
        return go.transform;
    }

    public NPCSearch(Transform entity, IMove move, Animator anim, NPCMemory memory)
        : base(entity, move, anim)
    {
        _memory = memory;
        _fakeTarget = CreateFakeTarget();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entró a estado SEARCH");

        if (_fakeTarget == null)
        {
            _fakeTarget = CreateFakeTarget();
        }

        _searchPoints = _memory.SearchPoints;
        _currentIndex = 0;
        _memory.IsSearching = true;

        if (_searchPoints != null && _searchPoints.Count > 0)
        {
            _fakeTarget.position = _searchPoints[0];
            SetPathAStarPlusVector(_move.Position, _searchPoints[0]);
            Debug.Log("Empezando búsqueda en: " + _searchPoints[0]);
        }
        else
        {
            Debug.LogWarning("NO hay puntos de búsqueda asignados en Search::Enter()");
        }
    }

    public override void Execute()
    {
        base.Execute();

        if (_searchPoints == null || _searchPoints.Count == 0)
            return;

        if (_isSearchFinished)
        {
            _move.Move(Vector3.zero);
            _finalPauseTimer += Time.deltaTime;
            if (_finalPauseTimer >= _finalPauseDuration)
            {
                _memory.IsSearching = false;
                _memory.SearchRequested = false;
            }
            return;
        }

        if (_currentIndex >= _searchPoints.Count)
        {
            _isSearchFinished = true;
            return;
        }

        if (IsFinishPath && !_isPaused)
        {
            _pauseTimer = 0f;
            _isPaused = true;
        }

        if (_isPaused)
        {
            _move.Move(Vector3.zero);
            _pauseTimer += Time.deltaTime;

            if (_pauseTimer >= _pauseDuration)
            {
                _isPaused = false;
                _currentIndex++;

                if (_currentIndex < _searchPoints.Count)
                {
                    _fakeTarget.position = _searchPoints[_currentIndex];
                    SetPathAStarPlusVector(_move.Position, _fakeTarget.position);
                    Debug.DrawRay(_searchPoints[_currentIndex] + Vector3.up, Vector3.up * 2, Color.cyan, 2f);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _memory.IsSearching = false;
        _memory.SearchRequested = false;
        if (_fakeTarget != null)
            GameObject.Destroy(_fakeTarget.gameObject);
    }

    //private void PickNewPoint()
    //{
    //    if (_searchPoints == null || _searchPoints.Count == 0)
    //        return;

    //    if (_isSearchFinished)
    //    {
    //        _move.Move(Vector3.zero);
    //        _finalPauseTimer += Time.deltaTime;

    //        if (_finalPauseTimer >= _finalPauseDuration)
    //        {
    //            _memory.IsSearching = false;
    //            _memory.SearchRequested = false;
    //        }
    //        return;
    //    }

    //    if (_currentIndex >= _searchPoints.Count)
    //    {
    //        _isSearchFinished = true;
    //        return;
    //    }

    //    if (IsFinishPath && !_isPaused)
    //    {
    //        _pauseTimer = 0f;
    //        _isPaused = true;
    //    }

    //    if (_isPaused)
    //    {
    //        _move.Move(Vector3.zero);
    //        _pauseTimer += Time.deltaTime;

    //        if (_pauseTimer >= _pauseDuration)
    //        {
    //            _isPaused = false;
    //            _currentIndex++;

    //            if (_currentIndex < _searchPoints.Count)
    //            {
    //                _fakeTarget.position = _searchPoints[_currentIndex];
    //                SetPathAStarPlusVector(_move.Position, _fakeTarget.position);
    //            }
    //        }
    //    }
    //}

    public static List<Vector3> GetRandomSearchPoints(Vector3 origin, int count, float radius)
    {
        List<Vector3> points = new List<Vector3>();
        int attempts = 0;

        while (points.Count < count && attempts < count * 10)
        {
            Vector2 random = UnityEngine.Random.insideUnitCircle * radius;
            Vector3 candidate = Vector3Int.RoundToInt(origin + new Vector3(random.x, 0, random.y));

            if (ObstacleManager.Instance.IsRightPos(candidate))
            {
                points.Add(candidate);
            }

            attempts++;
        }

        return points;
    }

    public void SetSearchPoints(List<Vector3> points)
    {
        _searchPoints = points;
        _currentIndex = 0;

        foreach (var point in _searchPoints)
        {
            if (!ObstacleManager.Instance.IsRightPos(point))
            {
                Debug.LogWarning("Punto inválido: " + point);
            }
            else
            {
                Debug.DrawRay(point + Vector3.up * 0.1f, Vector3.up * 1f, Color.green, 3f);
            }
        }

        Debug.Log("Puntos de búsqueda asignados: " + _searchPoints.Count);
    }
}
