using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class turnBack : RAINAction
{
    private Vector3 turnTarget = new Vector3(0,0,99);

    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.WorkingMemory.GetItem<float>("direction") == 1.0f)
        {
            ai.WorkingMemory.SetItem<float>("direction", -1.0f);
            if (ai.Body.transform.localScale.x < 0)
            {
                Vector3 myScale = ai.Body.transform.localScale;
                myScale.x *= -1;
                ai.Body.transform.localScale = myScale;
            }
        }
        else
        {
            ai.WorkingMemory.SetItem<float>("direction", 1.0f);
            if (ai.Body.transform.localScale.x > 0)
            {
                Vector3 myScale = ai.Body.transform.localScale;
                myScale.x *= -1;
                ai.Body.transform.localScale = myScale;
            }
        }
        float direction = ai.WorkingMemory.GetItem<float>("direction");
        ai.Senses.Sensors[0].AngleOffset = new Vector3(0, direction * 90, 0);


        ai.Body.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        return ActionResult.SUCCESS;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}