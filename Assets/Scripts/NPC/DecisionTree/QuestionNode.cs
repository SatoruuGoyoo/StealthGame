using System;
using UnityEngine;

// QuestionNode class for the Decision Tree
// This class represents a node in the decision tree that asks a question and executes one of two child nodes based on the answer.

public class QuestionNode : ITreeNode
{
    Func<bool> _question;
    ITreeNode _tNode;
    ITreeNode _fNode;

    public QuestionNode(Func<bool> question, ITreeNode tNode, ITreeNode fNode)
    {
        _question = question;
        _tNode = tNode;
        _fNode = fNode;
    }
    public void Execute()
    {
        if (_question())
        {
            _tNode.Execute();
        }
        else
        {
            _fNode.Execute();
        }
    }
}
