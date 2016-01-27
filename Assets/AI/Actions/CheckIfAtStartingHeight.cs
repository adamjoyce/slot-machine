using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINDecision]
public class CheckIfAtStartingHeight : RAINDecision
{
    private int _lastRunning = 0;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        _lastRunning = 0;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        ActionResult tResult = ActionResult.SUCCESS;

        float currentY = ai.Body.transform.position.y;
        float startY = ai.WorkingMemory.GetItem<float>("startY");

        if (currentY == startY)
        {
            ActionResult myResult = ActionResult.SUCCESS;

            for (; _lastRunning < _children.Count; _lastRunning++)
            {
                myResult = _children[_lastRunning].Run(ai);
                if (myResult != ActionResult.SUCCESS)
                    break;
            }
            return myResult;
        }
        else
            return ActionResult.FAILURE;
        
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}