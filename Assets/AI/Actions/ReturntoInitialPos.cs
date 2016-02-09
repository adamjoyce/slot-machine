using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

[RAINAction]
public class ReturntoInitialPos : RAINAction
{
    public override void Start(RAIN.Core.AI ai)
    {
        base.Start(ai);
    }

    public override ActionResult Execute(RAIN.Core.AI ai)
    {
        if (ai.WorkingMemory.ItemExists("wallFound") && ai.WorkingMemory.GetItem<GameObject>("wallFound") != null)
        {
            Vector3 currentIniPos = ai.WorkingMemory.GetItem<Vector3>("InitialPosition");
            currentIniPos.x = currentIniPos.x + 2*ai.Body.transform.position.x;
            ai.WorkingMemory.SetItem<Vector3>("InitialPosition", currentIniPos);
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
            return ActionResult.RUNNING;
        }
        Vector3 initialPos = ai.WorkingMemory.GetItem<Vector3>("InitialPosition");
        if (Mathf.Abs((ai.Body.transform.position - ai.WorkingMemory.GetItem<Vector3>("InitialPosition")).y) < 0.5f)
        {
            ai.WorkingMemory.SetItem<Vector3>("InitialPosition", new Vector3(99, 99, 99));
            return ActionResult.SUCCESS;
        }

        Vector3 direction = (ai.WorkingMemory.GetItem<Vector3>("InitialPosition") - ai.Body.transform.position);
        direction.Normalize();
        direction.x = -direction.x;
        ai.Body.GetComponent<Rigidbody2D>().velocity = direction * 2.0f;


        return ActionResult.RUNNING;
    }

    public override void Stop(RAIN.Core.AI ai)
    {
        base.Stop(ai);
    }
}