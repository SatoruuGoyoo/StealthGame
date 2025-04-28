using System;
using UnityEngine;

// ActionNode class for the Decision Tree
// This class represents a node in the decision tree that executes an action when called.

public class ActionNode : ITreeNode
{
    Action _action;
    public ActionNode(Action action)
    {
        _action = action;
    }
    public void Execute()
    {
        _action();
    }
}
