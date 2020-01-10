using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class RollerAgent : Agent
{
    public Transform target;

    Rigidbody rBody;

    public float speed;

    private void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void AgentReset()
    { 
        //Roller Agent 위치 초기화
        if (this.transform.position.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.position = new Vector3(0, 0.5f, 0);
        }

        //Target 위치 초기화
        target.position = new Vector3(Random.value * 8 - 4, 0.5f, Random.value * 8 - 4);
    }

    public override void CollectObservations()
    {
        //target 과 agent의 위치 수집
        AddVectorObs(target.position);
        AddVectorObs(this.transform.position);

        //agent의 velocity 수집
        AddVectorObs(rBody.velocity.x);
        AddVectorObs(rBody.velocity.y);
    }

    public override void AgentAction(float[] vectorAction)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[0];
        controlSignal.z = vectorAction[1];
        rBody.AddForce(controlSignal * speed);

        float distanceToTarget = Vector3.Distance(this.transform.position, target.position);

        if(distanceToTarget < 1.42f)
        {
            SetReward(1.0f);
            Done();
        }

        if(this.transform.position.y < 0)
        {
            Done();
        }
    }

    public override float[] Heuristic()
    {
        var action = new float[2];
        action[0] = Input.GetAxis("Horizontal");
        action[1] = Input.GetAxis("Vertical");
        return action;
    }
}
