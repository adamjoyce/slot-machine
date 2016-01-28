using RAIN.Entities.Aspects;
using RAIN.Perception.Sensors;
using RAIN.Serialization;
using UnityEngine;

[RAINSerializableClass]
public class RaySensor : VisualSensor
{
    [RAINSerializableField]
    private LayerMask _physicsMask = -1;
    public float rayRange;

    protected override bool TestVisibility(RAINAspect aAspect, float aSqrRange)
    {
        // Normal aspect test worked, we're done
        if (base.TestVisibility(aAspect, aSqrRange))
            return true;

        // Being a little pedantic about this, but just in case
        Transform tAspectTransform = aAspect.MountPoint;
        if (tAspectTransform == null)
            tAspectTransform = aAspect.Entity.Form.transform;

        float sizeX = AI.Body.GetComponent<Renderer>().bounds.size.x;
        float sizeY = AI.Body.GetComponent<Renderer>().bounds.size.y + 0.1f;
        


        float direction = AI.WorkingMemory.GetItem<float>("direction");
        // Do a physics check against the aspect, may need to add a mask for this
        RaycastHit2D myRaycastHit = Physics2D.Linecast(AI.Body.transform.position + new Vector3(direction * sizeX,0.0f,0.0f), AI.Body.transform.position + new Vector3(direction * (sizeX + rayRange), 0.0f, 0.0f));
        // If the collider is a child (or equal to) our aspect, we're good
        if (myRaycastHit.collider != null && myRaycastHit.collider.transform.IsChildOf(tAspectTransform))
            return true;

        return false;
    }
}