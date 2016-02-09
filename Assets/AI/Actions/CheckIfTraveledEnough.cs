using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINDecision]
public class CheckIfTraveledEnough : RAINDecision
{
    private int _lastRunning = 0;

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);

        _lastRunning = 0;
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.WorkingMemory.ItemExists("wallFound") && ai.WorkingMemory.GetItem<GameObject>("wallFound") != null)
            return ActionResult.FAILURE;

        ActionResult tResult = ActionResult.SUCCESS;
        if (ai.WorkingMemory.ItemExists("startPoint"))
        {
            Vector3 startPoint = ai.WorkingMemory.GetItem<Vector3>("startPoint");
            float dist = Mathf.Abs((startPoint - ai.Body.transform.position).magnitude);

            if (dist >= 5.0f)
                return ActionResult.FAILURE;
        }
        else
        {
            ai.WorkingMemory.SetItem<Vector3>("startPoint", ai.Body.transform.position);
            return ActionResult.FAILURE;
        }
        for (; _lastRunning < _children.Count; _lastRunning++)
        {
            tResult = _children[_lastRunning].Run(ai);
            if (tResult != ActionResult.SUCCESS)
                break;
        }

        return tResult;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}