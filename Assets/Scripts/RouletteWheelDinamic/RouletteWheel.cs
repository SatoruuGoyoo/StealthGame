using UnityEngine;
using System.Collections.Generic;

public class RouletteWheel<T>
{
    private List<T> _items = new List<T>();
    private List<float> _weights = new List<float>();
    private float _totalWeight = 0f;

    public void Clear()
    {
        _items.Clear();
        _weights.Clear();
        _totalWeight = 0f;
    }

    public void AddItem(T item, float weight)
    {
        if (weight <= 0f) return;
        _items.Add(item);
        _weights.Add(weight);
        _totalWeight += weight;
    }

    public T GetRandom()
    {
        if (_items.Count == 0) return default;

        float r = Random.Range(0f, _totalWeight);
        float accum = 0f;

        for (int i = 0; i < _items.Count; i++)
        {
            accum += _weights[i];
            if (r <= accum)
                return _items[i];
        }

        return _items[_items.Count - 1]; // fallback
    }
}
