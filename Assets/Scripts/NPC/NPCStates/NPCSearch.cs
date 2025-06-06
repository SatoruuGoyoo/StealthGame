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

        if (_fakeTarget == null)
        {
            _fakeTarget = CreateFakeTarget();
        }

        _visitedPoints = 0;
        _pauseTimer = 0f;
        _finalPauseTimer = 0f;
        _isPaused = false;
        _isSearchFinished = false;
        _searchRequestCleared = false;

        _lastPosition = _move.Position;
        _memory.IsSearching = true;

        PickNewPoint();
    }

    public override void Execute()
    {
        base.Execute();

        if (!_searchRequestCleared && _memory.SearchRequested)
        {
            _memory.SearchRequested = false;
            _searchRequestCleared = true;
        }

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

        if (_visitedPoints >= _maxPoints)
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
                _visitedPoints++;
                _lastPosition = _move.Position;
                PickNewPoint();
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _memory.IsSearching = false;

        if (_fakeTarget != null)
            GameObject.Destroy(_fakeTarget.gameObject);
    }

    private void PickNewPoint()
    {
        Vector3 candidate;
        int attempts = 0;

        do
        {
            Vector2 random = Random.insideUnitCircle * _stepDistance;
            candidate = _lastPosition + new Vector3(random.x, 0, random.y);
            candidate = Vector3Int.RoundToInt(candidate);
            attempts++;
        }
        while (!ObstacleManager.Instance.IsRightPos(candidate) && attempts < 10);

        _fakeTarget.position = candidate;
        SetPathAStarPlusVector(_move.Position, candidate);
    }
}
