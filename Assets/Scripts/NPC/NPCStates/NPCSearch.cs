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

        NPCMemory.IsSearching = true;
        // No apagar SearchRequested todavía
        Debug.Log("SEARCH ENTER: Comienza búsqueda. IsSearching = true");
    }

    public override void Execute()
    {
        base.Execute();

        // Solo una vez apagamos SearchRequested
        if (!_searchRequestCleared && NPCMemory.SearchRequested)
        {
            NPCMemory.SearchRequested = false;
            _searchRequestCleared = true;
            Debug.Log("SEARCH: SearchRequested apagado en Execute()");
        }

        // Final de la búsqueda (pausa final)
        if (_isSearchFinished)
        {
            _move.Move(Vector3.zero);
            _finalPauseTimer += Time.deltaTime;

            if (_finalPauseTimer >= _finalPauseDuration)
            {
                NPCMemory.IsSearching = false;
                Debug.Log("SEARCH: Termina búsqueda. IsSearching = false");
            }

            return;
        }

        // Recorre puntos
        if (_visitedPoints >= _maxPoints)
        {
            _isSearchFinished = true;
            Debug.Log("SEARCH: Completó puntos. Entra en pausa final.");
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
                Debug.Log($"SEARCH: Retoma movimiento hacia nuevo punto #{_visitedPoints + 1}");
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
                Debug.Log($"SEARCH: Pausa en punto #{_visitedPoints}");
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        NPCMemory.IsSearching = false;
        Debug.Log("SEARCH EXIT: IsSearching = false (por seguridad)");
    }

    private void PickNewDirection()
    {
        Vector2 random = Random.insideUnitCircle.normalized * _stepDistance;
        Vector3 nextPoint = _lastPosition + new Vector3(random.x, 0, random.y);
        _currentDirection = (nextPoint - _move.Position).normalized;
    }
}
