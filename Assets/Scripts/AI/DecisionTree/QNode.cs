using System;
using UnityEngine;

public class QNode : ITreeNode
{
    Func<bool> _question;
    ITreeNode _trueNode;
    ITreeNode _falseNode;

    public QNode(Func<bool> question, ITreeNode TrueNode, ITreeNode FalseNode)
    {
        _question = question;
        _trueNode = TrueNode;
        _falseNode = FalseNode;
    }

    public void Execute()
    { 
        if (_question())
        {
            // Execute True Node
            _trueNode.Execute();
        }
        else
        {
            // Execute False Node
            _falseNode.Execute();
        }
    }
}
