using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloToolkit.Unity.InputModule;
using UnityEngine.VR.WSA.Input;

public class GestureManager : MonoBehaviour {

    private Vector3 prevPos;
    private bool isHold;
    private GameObject focusObj;

	// Use this for initialization
	void Start () {
        InteractionManager.SourcePressed += InteractionManager_SourcePressed;
        InteractionManager.SourceReleased += InteractionManager_SourceReleased;
        InteractionManager.SourceLost += InteractionManager_SourceLost;
        InteractionManager.SourceUpdated += InteractionManager_SourceUpdated;
	}

    private void InteractionManager_SourceUpdated(InteractionSourceState state)
    {
        if(!isHold || focusObj == null)
        {
            return;
        }

        Vector3 handPosition;
        state.properties.location.TryGetPosition(out handPosition);

        if(state.source.kind == InteractionSourceKind.Hand && state.properties.location.TryGetPosition(out handPosition))
        {
            var moveVector = Vector3.zero;
            moveVector = handPosition - prevPos;

            prevPos = handPosition;

            var handDistance = Vector3.Distance(Camera.main.transform.position, handPosition);
            var objectDistance = Vector3.Distance(Camera.main.transform.position, focusObj.transform.position);

            focusObj.transform.position += (moveVector * (objectDistance / handDistance));
        }
    }

    private void InteractionManager_SourceLost(InteractionSourceState state)
    {
        if(focusObj == null)
        {
            return;
        }

        isHold = false;
        focusObj = null;
    }

    private void InteractionManager_SourceReleased(InteractionSourceState state)
    {
        if(focusObj == null)
        {
            return;
        }

        isHold = false;
        focusObj = null;
    }

    private void InteractionManager_SourcePressed(InteractionSourceState state)
    {
        if(focusObj == null)
        {
            return;
        }
        Vector3 handPosition;
        if(state.source.kind == InteractionSourceKind.Hand && state.properties.location.TryGetPosition(out handPosition))
        {
            isHold = true;
            prevPos = handPosition;
        }
    }

    // Update is called once per frame
    void Update () {
        var obj = GazeManager.Instance.HitObject;
        if(obj != null && !isHold)
        {
            if(obj.tag == "Interaction")
            {
                focusObj = obj;
            }
        }
	}
}
