using System;
using UnityEngine;

public class ANode : ITreeNode
{
    Action _action;

    public ANode(Action <int >action)
    {
       // _action = action;
    }

    public void Execute()
    {
        _action();
    }
}
