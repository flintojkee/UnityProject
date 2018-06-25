using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Orc : MonoBehaviour
{

	public Vector3 pointA;
	public Vector3 pointB;
	public float speed = 1;

	protected float runSpeed;
	protected float currentSpeed;

	protected Animator animator = null;
	protected Rigidbody2D body = null;
	SpriteRenderer sr = null;
	protected Mode mode = Mode.GoToA;

	public enum Mode
	{
		GoToA,
		GoToB,
		Attack,
		Die
	}

	// Use this for initialization
	void Start()
	{
		animator = GetComponent<Animator>();
		body = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		currentSpeed = speed;
		runSpeed = this.speed * 2;

		animator.SetBool("walk", true);
	}

	// Update is called once per frame
	void Update()
	{

	}

	void FixedUpdate()
	{
		if (mode == Mode.Die) return;

		// walking
		if (mode == Mode.GoToA || mode == Mode.GoToB) {
			currentSpeed = speed;
			walk(getDirection());
		}

		if (mode == Mode.GoToA && isArrived(this.transform.position, pointA)) {
			mode = Mode.GoToB;
		}

		if (mode == Mode.GoToB && isArrived(this.transform.position, pointB)) {
			mode = Mode.GoToA;
		}

		// attacking
		if (isRabitInOrcZone()) {
			mode = Mode.Attack;
		} 

		// restore patrolling after attack
		if (mode == Mode.Attack && !isRabitInOrcZone()) {
			mode = Mode.GoToA;
			animator.SetBool("walk", true);
			animator.SetBool("run", false);
		}

		if (mode == Mode.Attack) {
			attack();
		}

		if (isRabitAboveOrc()) {
			mode = Mode.Die;      
			StartCoroutine(die());
		}
	}

	protected abstract void attack();

	protected bool performingAttack = false;
	protected virtual IEnumerator hitTheRabit() {
		performingAttack = true;
		animator.SetTrigger("attack");
		yield return new WaitForSeconds(
			animator.GetCurrentAnimatorStateInfo(0).length
			+ animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		if (LevelController.current.IsHeroBig()) {
			LevelController.current.makeSmaller(HeroRabit.lastRabit);
			HeroRabit.lastRabit.revive();
		} else {
			HeroRabit.lastRabit.kill();
		}
		performingAttack = false;
	}

	public void walk(float direction)
	{
		Vector2 vel = body.velocity;
		if (Mathf.Abs(direction) > 0)
		{
			vel.x = direction * currentSpeed;
			body.velocity = vel;

			sr.flipX = !(direction < 0);
		}
	}

	protected IEnumerator die()
	{
		currentSpeed = 0;
		animator.SetTrigger("die");
		yield return new WaitForSeconds(
			animator.GetCurrentAnimatorStateInfo(0).length
			+ animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		Destroy(this.gameObject);
	}

	protected bool isRabitAboveOrc()
	{
		Vector3 rabbit_pos = HeroRabit.lastRabit.transform.position;
		Vector3 my_pos = this.transform.position;
		float posDiff = rabbit_pos.y - my_pos.y;
		return posDiff < 1.5f && posDiff > 0 && Mathf.Abs(rabbit_pos.x - my_pos.x) < 0.5f;
	}

	protected bool isCloseToRabit()
	{
		Vector3 rabbit_pos = HeroRabit.lastRabit.transform.position;
		Vector3 my_pos = this.transform.position;
		float xDiff = Mathf.Abs(rabbit_pos.x - my_pos.x);
		float yDiff = rabbit_pos.y - my_pos.y;
		if (LevelController.current.IsHeroBig()) {
			return xDiff < 1.3 && yDiff < 0.8f;
		}
		return xDiff < 1f && yDiff < 0.5f;
	}

	protected virtual bool isRabitInOrcZone()
	{
		Vector3 rabbit_pos = HeroRabit.lastRabit.transform.position;
		Vector3 my_pos = this.transform.position;
		float startPoint = Mathf.Min(pointA.x, pointB.x);
		float endPoint = Mathf.Max(pointA.x, pointB.x);

		if (rabbit_pos.x >= startPoint && rabbit_pos.x <= endPoint
			&& Mathf.Abs(rabbit_pos.y - my_pos.y) < 1f) {
			return true;
		}
		return false;
	}

	protected float getDirection()
	{
		Vector3 my_pos = this.transform.position;
		if (mode == Mode.GoToA)
		{
			if (my_pos.x < pointA.x)
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}
		else if (mode == Mode.GoToB)
		{
			if (my_pos.x < pointB.x)
			{
				return 1;
			}
			else
			{
				return -1;
			}

		}
		return 0;
	}

	protected float getDirectionToRabit()
	{
		Vector2 my_pos = this.transform.position;
		if (my_pos.x < HeroRabit.lastRabit.transform.position.x)
		{
			return 1;
		}
		else
		{
			return -1;
		}
	}

	protected bool isArrived(Vector3 pos, Vector3 target)
	{
		float posX = Mathf.Abs(pos.x);
		float targetX = Mathf.Abs(target.x);
		return Mathf.Abs(posX - targetX) < 0.1f;
	}

}