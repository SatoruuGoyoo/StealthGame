using UnityEngine;

public class NPCMemory
{
    private float _lastSeenTime = -100f;
    private float _chaseMemoryTime = 3f;

    public bool SearchRequested { get; set; } = false;
    public bool IsSearching { get; set; } = false;

    public bool ShouldKeepChasing => Time.time - _lastSeenTime <= _chaseMemoryTime;

    public void UpdateLastSeen()
    {
        _lastSeenTime = Time.time;
    }
}
