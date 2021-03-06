﻿using UnityEngine;
using System.Collections;
using System;

public class Vehicle : VehicleBase {

	private void OnCollisionEnter(Collision collision) {
		ForceUpdate();
	}

	protected override void UpdatePosition() {
		// cache update
		position += forward * speed * Time.deltaTime;
	}

}
