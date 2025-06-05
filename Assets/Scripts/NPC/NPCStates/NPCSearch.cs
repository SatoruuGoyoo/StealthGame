using UnityEngine;

public class NPCSearch<T> : NPCBase<T>
{
    private Vector3 _lastPosition;
    private Vector3 _currentDirection;

    private float _pauseTimer;
    private float _changeDirTimer;
    private float _finalPauseTimer;

    private int _visitedPoints = 0;
    private const int _maxPoints = 4;

    private const float _pauseDuration = 2.5f;
    private const float _changeDirInterval = 1f;
    private const float _stepDistance = 8f;
    private const float _finalPauseDuration = 2f;

    private bool _isPaused = false;
    private bool _isSearchFinished = false;
    private bool _searchRequestCleared = false;

    private NPCMemory _memory;

    public NPCSearch(NPCMemory memory)
    {
        _memory = memory;
    }

    public override void Enter()
    {
        base.Enter();

        _visitedPoints = 0;
        _pauseTimer = 0f;
        _changeDirTimer = 0f;
        _finalPauseTimer = 0f;
        _isPaused = false;
        _isSearchFinished = false;
        _searchRequestCleared = false;

        _lastPosition = _move.Position;

        PickNewDirection();

        _memory.IsSearching = true;
    }
    public override void Execute()
    {
        base.Execute();

        if (!_searchRequestCleared && _memory.SearchRequested)
        {
            _memory.SearchRequested = false;
            _searchRequestCleared = true;
        }

        // End of Search
        if (_isSearchFinished)
        {
            _move.Move(Vector3.zero);
            _finalPauseTimer += Time.deltaTime;

            if (_finalPauseTimer >= _finalPauseDuration)
            {
                _memory.IsSearching = false;
            }

            return;
        }

        // Go trought points
        if (_visitedPoints >= _maxPoints)
        {
            _isSearchFinished = true;
            return;
        }

        if (_isPaused)
        {
            _pauseTimer += Time.deltaTime;
            _move.Move(Vector3.zero);

            if (_pauseTimer >= _pauseDuration)
            {
                _isPaused = false;
                _pauseTimer = 0f;
                PickNewDirection();

            }
        }
        else
        {
            _move.Move(_currentDirection);
            _changeDirTimer += Time.deltaTime;

            if (_changeDirTimer >= _changeDirInterval)
            {
                _isPaused = true;
                _changeDirTimer = 0f;
                _visitedPoints++;
                _lastPosition = _move.Position;

            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        _memory.IsSearching = false;

    }
    private void PickNewDirection()
    {
        Vector2 random = Random.insideUnitCircle.normalized * _stepDistance;
        Vector3 nextPoint = _lastPosition + new Vector3(random.x, 0, random.y);
        _currentDirection = (nextPoint - _move.Position).normalized;
    }
}
