using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class CarAgent : Agent
{
	Rigidbody rBody;

	public WheelDrive wDrive;
	public WheelCollider a0l;
	public WheelCollider a0r;
	public WheelCollider a1l;
	public WheelCollider a1r;
	float distanceToTarget2;
	void Start () {

		distanceToTarget2 = Vector3.Distance(this.transform.position,
			Target.position);
		rBody = GetComponent<Rigidbody>();

	}

	void FixedUpdate(){
		//	print (this.transform.rotation);
		if (this.transform.rotation.z > 0.5 || this.transform.rotation.z < -0.5) {
			AgentReset ();
		}
		if (this.transform.rotation.x > 0.5 || this.transform.rotation.x < -0.5) {
			AgentReset ();
		}
	}

	public Transform Target;
	public override void AgentReset()
	{
		Quaternion newQuaternion = new Quaternion();
		newQuaternion.Set(0, 0f, 0, 1);
		this.transform.rotation = newQuaternion;
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		this.transform.localPosition = new Vector3( 6f, 1.08f, 2.31f);

		// Move the target to a new spot
	}
	public override void CollectObservations()
	{
		// Target and Agent positions
		AddVectorObs(Target.position);
		AddVectorObs(this.transform.position);
		//AddVectorObs (rBody.velocity.z);
		// Agent velocity
		AddVectorObs(rBody.velocity.x);
		AddVectorObs(rBody.velocity.z);

		// Agent torque

	}
	public float speed = 10;
	public override void AgentAction(float[] vectorAction, string textAction)
	{
		// Actions, size = 2
	
		wDrive.carAgentX =Mathf.Clamp(vectorAction[0], -1f, 1f)*900;
		wDrive.carAgentZ =Mathf.Clamp(vectorAction[1], -1f, 1f)*30;

		//rBody.AddForce(controlSignal * speed);
		//print(controlSignal.x+ " "+ controlSignal.z);
		print(rBody.velocity.x);
		// Rewards
		float distanceToTarget = Vector3.Distance(this.transform.position,
			Target.position);
		// Reached target
		if (distanceToTarget < 2f)
		{
			SetReward(1.0f);
			Done();
		}
		//StartCoroutine(WaitAndPrint(3.0F));
		if(this.rBody.velocity.x>0f && this.rBody.velocity.x<5f){
			AddReward(0.1f); 	
		}




	}
	void OnCollisionEnter(Collision collision)
	{
		print("me estrelle");
		SetReward (-1.0f);
		Done();
	}
	public void CollisionDetected(ChildScript childScript)
	{
		Debug.Log("child collided");
		//AddReward (0.001f);
	}
	IEnumerator WaitAndPrint(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		if (this.rBody.velocity == Vector3.zero) {
			Done();
			this.rBody.angularVelocity = Vector3.zero;
			this.rBody.velocity = Vector3.zero;
			this.transform.localPosition = new Vector3( 6f, 1.08f, 2.31f);
		}
	}
}
