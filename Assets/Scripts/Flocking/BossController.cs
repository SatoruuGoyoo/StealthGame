using UnityEngine;

public class BossController : NPCController
{
    private bool _alreadyAlerted = false;

    protected override void Update()
    {
        base.Update();

        if (!_alreadyAlerted && _los != null && target != null && _los.LOS(target.transform))
        {
            BossAlertManager.Instance.Alert(transform);
            _alreadyAlerted = true;
        }
    }

    protected override void InitializedTree()
    {
        var patrol = new ActionNode(() => _fsm.Transition(StateEnum.Patrol));
        var search = new ActionNode(() => _fsm.Transition(StateEnum.Search));
        var chase = new ActionNode(() => _fsm.Transition(StateEnum.Chase));
        var attack = new ActionNode(() => _fsm.Transition(StateEnum.Attack));

        var qAttack = new QuestionNode(QuestionCanAttack, attack, chase);
        var qCanSee = new QuestionNode(QuestionCanSeePlayer, qAttack, search);
        var qSearchReq = new QuestionNode(QuestionSearchRequested, search, patrol);
        var qIsSearching = new QuestionNode(QuestionIsSearching, search, qSearchReq);

        _root = new QuestionNode(QuestionCanSeePlayer, qAttack, qIsSearching);
       
    }
}

