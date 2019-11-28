using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class CarAgent2 : Agent
{
	Rigidbody rBody;

	public WheelDrive wDrive;
	public WheelCollider a0l;
	public WheelCollider a0r;
	public WheelCollider a1l;
	public WheelCollider a1r;
	public GameObject road;
	float distanceToTarget2;
	public float timer =0.0f;
	bool guardar;

	public Transform Target;
	RayPerception rayPer;

	[Header ("Sensors")]
	public float sensorLength =2;
	public Vector3 frontSensorPosition = new Vector3(0f,0.2f,1f);
	public Transform origin;
	public float frontSideSensorPosition = 0.2f;
	public float frontSensorAngle = 30;

	void Start () {

		rayPer = GetComponent<RayPerception3D> ();
		//distanceToTarget2 = Vector3.Distance(this.transform.localPosition,
		//	Target.localPosition);
		rBody = GetComponent<Rigidbody>();

	}

	void FixedUpdate(){
	}
		
	public override void AgentReset()
	{
		Quaternion newQuaternion = new Quaternion();
		newQuaternion.Set(0, 0, 0, 1);
		this.transform.rotation = newQuaternion;
		this.rBody.angularVelocity = Vector3.zero;
		this.rBody.velocity = Vector3.zero;
		this.transform.localPosition = new Vector3( 6.24f, 0.5f, 2.31f);

		// Move the target to a new spot
	//	Target.localPosition = new Vector3(6.35f,0.7f,40f);
	}
	public override void CollectObservations()
	{
		//Sensors ();
		//
		float rayDistance = 50f;
		float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f, -20f, -90f, -160f, -45f, -135f, -70f, -110f };
		string[] detectableObbjects = {"wall","goal"};
		AddVectorObs(rayPer.Perceive(rayDistance,rayAngles,detectableObbjects,0f,0f));
		Vector3 localVelocity = transform.InverseTransformDirection(rBody.velocity);
		// Target and Agent positions
		AddVectorObs(Target.localPosition);
		AddVectorObs(this.transform.localPosition);
		// Agent velocity
		AddVectorObs(localVelocity.x);
		AddVectorObs(localVelocity.z);

		// Agent torque
		AddVectorObs(wDrive.angleG);
		AddVectorObs(wDrive.torqueG);	
		//AddVectorObs(wDrive.brakeTorque);


	}

	public float speed = 10;
	public override void AgentAction(float[] vectorAction, string textAction)
	{
		// Actions, size = 2
		wDrive.carAgentX =Mathf.Clamp(vectorAction[0], -1,1)*100f;
		wDrive.carAgentZ = Mathf.Clamp (vectorAction [1], -1, 1) * 100f;
		//wDrive.carBrake = vectorAction[2];
		if (rBody.velocity.sqrMagnitude > 25f) // slow it down
		{
			rBody.velocity *= 0.95f;
		}
		float distanceToTarget = Vector3.Distance(this.transform.position,
			Target.position);
		
		if (distanceToTarget < 2f)
		{
			SetReward(1.0f);
			Done();
		}
		if(this.transform.position.y < 0){
			Done();
		}
		if(wDrive.torqueG == 900){
		//	StartCoroutine(WaitAndPrint(2f));
			AddReward(0.5f);
		}
		if (wDrive.torqueG < 0) {
			AddReward(-0.5f);
		}
		if(wDrive.angleG ==0){
			AddReward(0.5f);
		}
		else{
		//	AddReward(-0.001f);
		}
		//if (this.rBody.velocity == Vector3.zero) {
	//		StartCoroutine(WaitAndPrint(1f));
	//	}
		//print("angulo " +wDrive.angleG);
		//	print("torque " +wDrive.torqueG);
	}
	void OnCollisionEnter(Collision collision)
	{
	//	print("me estrelle");
		if (collision.gameObject.CompareTag("goal"))
		{
		AddReward (5f);
		}
		if (collision.gameObject.CompareTag("wall"))
		{
			AddReward (-10f);
		}
		Done();
	}

	public void CollisionDetected(ChildScript childScript)
	{
		road = childScript.ground;
		Debug.Log("child collided");
		//AddReward (0.001f);
	}
	IEnumerator WaitAndPrint(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);
		if (wDrive.torqueG == 900) {
			AddReward (0.1f);
		}
		/*if (this.rBody.velocity == Vector3.zero) {
			//AddReward (-1f);
			Done();
			this.rBody.angularVelocity = Vector3.zero;
			this.rBody.velocity = Vector3.zero;
			this.transform.localPosition = new Vector3( 6.24f, 0.5f, 2.31f);
		}*/
	}
		

	private void Sensors(){
		Quaternion newQuaternion = new Quaternion();
		newQuaternion.Set(0, 0, 0, 1);
		RaycastHit hit;
		Vector3 sensorStartPos = origin.localPosition;
		//front  sensor
		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
			if(hit.collider.name.Equals("Target")){
				AddReward (0.1f);
			}else{
				AddReward (-0.1f);}
		}
		Debug.DrawLine (sensorStartPos, hit.point,Color.red);

		//front right sensor
		sensorStartPos.z -= frontSideSensorPosition;
		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
			AddReward (-0.1f);
		}
		Debug.DrawLine (sensorStartPos, hit.point,Color.red);

		//front right angle sensor
		if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(frontSensorAngle,transform.up)*transform.forward, out hit, sensorLength)) {
			AddReward (-0.1f);
		}
		Debug.DrawLine (sensorStartPos, hit.point,Color.red);

		//front left sensor
		sensorStartPos.z += 2*frontSideSensorPosition;

		if (Physics.Raycast(sensorStartPos, transform.forward, out hit, sensorLength)) {
			AddReward (-0.1f);
		}
		Debug.DrawLine (sensorStartPos, hit.point,Color.red);

		//front left angle sensor
		if (Physics.Raycast(sensorStartPos, Quaternion.AngleAxis(-frontSensorAngle,transform.up)*transform.forward, out hit, sensorLength)) {
			AddReward (-0.1f);
		}
		Debug.DrawLine (sensorStartPos, hit.point,Color.red);
	}
}
