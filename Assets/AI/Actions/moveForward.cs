using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class moveForward : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (!ai.WorkingMemory.ItemExists("direction"))
            ai.WorkingMemory.SetItem<float>("direction", 1.0f);

        float direction = ai.WorkingMemory.GetItem<float>("direction");

        ai.Body.GetComponent<Rigidbody2D>().velocity = new Vector3(direction * 2.0f, ai.Body.GetComponent<Rigidbody2D>().velocity.y);
        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}