using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class SweepOnPlayer : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
        ai.WorkingMemory.SetItem<Vector3>("InitialPosition", new Vector3(0, 0, 99));
        ai.WorkingMemory.SetItem<Vector3>("PlayerPosition", new Vector3(0, 0, 99));
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.WorkingMemory.ItemExists("wallFound") && ai.WorkingMemory.GetItem<GameObject>("wallFound") != null)
        {
            ai.WorkingMemory.SetItem<Vector3>("PlayerPosition", new Vector3(0, 0, 99));
            return ActionResult.SUCCESS;
        }
        Vector3 initialPos = ai.WorkingMemory.GetItem<Vector3>("InitialPosition");
        if (initialPos.z == 99)
        {
            ai.WorkingMemory.SetItem<Vector3>("PlayerPosition", ai.WorkingMemory.GetItem<GameObject>("playerFound").transform.position + new Vector3(0,0.3f,0));
            ai.WorkingMemory.SetItem<Vector3>("InitialPosition", new Vector3(ai.WorkingMemory.GetItem<GameObject>("playerFound").transform.position.x, ai.Body.transform.position.y, 0));
        }
        else if((ai.Body.transform.position - ai.WorkingMemory.GetItem<Vector3>("PlayerPosition")).magnitude < 0.1f)
        {
            Vector3 playerPos = ai.WorkingMemory.GetItem<Vector3>("PlayerPosition");
            playerPos.z = 99;
                ai.WorkingMemory.SetItem<Vector3>("PlayerPosition", playerPos);
            return ActionResult.SUCCESS;
        }

        Vector3 direction = (ai.WorkingMemory.GetItem<Vector3>("PlayerPosition") - ai.Body.transform.position);
        if (direction.x > 0 && ai.Body.transform.localScale.x > 0 || direction.x < 0 && ai.Body.transform.localScale.x < 0)
            ai.Body.transform.localScale = new Vector3(-ai.Body.transform.localScale.x, ai.Body.transform.localScale.y, ai.Body.transform.localScale.z);


        direction.Normalize();
        ai.Body.GetComponent<Rigidbody2D>().velocity = direction * 2.0f;


        return ActionResult.RUNNING;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}