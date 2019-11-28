using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class CarAcademy : Academy
{
	public override void InitializeAcademy()
	{
		Monitor.verticalOffset = 1f;
		Physics.defaultSolverIterations = 12;
		Physics.defaultSolverVelocityIterations = 12;
		Time.fixedDeltaTime = 0.01333f; // (75fps). default is .2 (60fps)
		Time.maximumDeltaTime = .15f; // Default is .33
	}

	//TODO: 
	//SETUP: Ambiente donde el agente necesita navegar al punto objetivo.
    // Start is called before the first frame update

}
