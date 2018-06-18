using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

	public Vector3 movingDirection;
	public float speed;
	public float pause;

	private Vector3 A;
	private Vector3 B;
	private Vector3 currentDirection;
	private float waitTimeLeft;

	// Use this for initialization
	void Start()
	{
		A = this.transform.position;
		B = A + movingDirection;
		currentDirection = B;
		waitTimeLeft = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (waitTimeLeft > 0) {
			waitTimeLeft -= Time.deltaTime;
			return;
		}

		this.transform.position = Vector3.MoveTowards(
			this.transform.position,
			currentDirection,
			speed * Time.deltaTime
		);

		if (isArrived(this.transform.position, B))
		{
			currentDirection = A;
			waitTimeLeft = pause;
		}
		else if (isArrived(this.transform.position, A))
		{
			currentDirection = B;
			waitTimeLeft = pause;
		}
	}

	private bool isArrived(Vector3 pos, Vector3 target)
	{
		pos.z = 0;
		target.z = 0;
		return Vector3.Distance(pos, target) < 0.02f;
	}
}