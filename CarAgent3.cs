using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using System;
using MLAgents;

[Serializable]
public enum DriveType2
{
	RearWheelDrive,
	FrontWheelDrive,
	AllWheelDrive
}
public class CarAgent3 : Agent
{
	Rigidbody rBody;

	bool guardar;

	public Transform Target;

	RayPerception rayPer;
	[Header ("NavMeshScript")]
	public RandomPointOnNavMesh rPNV;
	[Header ("WheelDrive")]
	public GameObject wheelShape;
	public DriveType driveType;
	private WheelCollider[] m_Wheels;


	Scene scene;
	void Start () {
		scene = SceneManager.GetActiveScene ();
		rayPer = GetComponent<RayPerception3D> ();
		rBody = GetComponent<Rigidbody>();
		m_Wheels = GetComponentsInChildren<WheelCollider>();

		for (int i = 0; i < m_Wheels.Length; ++i) 
		{
			var wheel = m_Wheels [i];

			// Create wheel shapes only when needed.
			if (wheelShape != null)
			{
				var ws = Instantiate (wheelShape);
				ws.transform.parent = wheel.transform;
			}
		}
	}
		

	public override void AgentReset()
	{
		//Quaternion newQuaternion = new Quaternion();
	//	newQuaternion.Set(0, 0.7f, 0, 1);
		//this.transform.rotation = newQuaternion;
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		if (scene.name == "Windridge City Demo") {
			this.transform.localPosition = new Vector3 (-486.418f, 289.72f, 155.71f);
		}
		if (scene.name == "Car1") {
			rPNV.teleport = true;
			this.transform.localPosition = new Vector3 (0f, 0.5f, 0f);
		}
		transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));
	//	Done();
		// Move the target to a new spot
		//Target.localPosition = new Vector3(Random.Range(-457.05f,-454.78f),
	//		290.39f,
	//		Random.Range(155.74f,154.26f));
	}
	public override void CollectObservations()
	{
		const float rayDistance = 35f;
		float[] rayAngles = {20f, 90f, 160f, 45f, 135f, 70f, 110f};
		float[] rayAngles1 = {25f, 95f, 165f, 50f, 140f, 75f, 115f};
		float[] rayAngles2 = {15f, 85f, 155f, 40f, 130f, 65f, 105f};

		string[] detectableObjects = {"block", "wall", "goal", "stone"};
		AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
		AddVectorObs(rayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, 5f));
		AddVectorObs(rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 10f));
		//AddVectorObs(switchLogic.GetState());
		AddVectorObs(transform.InverseTransformDirection(rBody.velocity));
		// Target and Agent positions
	//	AddVectorObs(Target.localPosition);
	//	AddVectorObs(this.transform.localPosition);
		// Agent velocity
		// Agent torque
	//	AddVectorObs(wDrive.angleG);
	//	AddVectorObs(wDrive.torqueG);	
	//	AddVectorObs(wDrive.handBrakeG);
	}
	public float speed = 10;
	public void MoveAgent(float[] act)
	{
		var dirToGo = Vector3.zero;
		var rotateDir = Vector3.zero;
		dirToGo = transform.forward * Mathf.Clamp(act[1], -1f, 1f);
		rotateDir = transform.up * Mathf.Clamp(act[0], -1f, 1f);

		if(this.transform.localPosition.y < -1){
		//	AddReward (-1f);
	//		AgentReset ();
		}

		transform.Rotate(rotateDir, Time.deltaTime * 100f);
		rBody.AddForce(dirToGo * 1f, ForceMode.VelocityChange);
	}
	public override void AgentAction(float[] vectorAction, string textAction)
	{
		AddReward(-1f / agentParameters.maxStep);
		MoveAgent(vectorAction);
		// Actions, size = 2
    //	wDrive.carAgentX =Mathf.Clamp(vectorAction[0]*100, -1,1);
	//	wDrive.carAgentZ = Mathf.Clamp (vectorAction [1]*100, -1, 1);
	//	wDrive.carBrake = Mathf.Clamp (vectorAction [2]*100, 0, 1f)*30000f;

	}
	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("goal"))
		{
			SetReward(2f);
			Done();
		}
		if (collision.gameObject.CompareTag("wall"))
		{
			//	AdInput.GetKey(KeyCode.X) ? brakeTorque : 0dReward (-50f);
		}
		//Done();
	}
	public void CollisionDetected(ChildScript childScript)
	{
	//Debug.Log("child collided");
//	AddReward (0.001f);
	}
}
