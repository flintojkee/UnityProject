using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrownOrc : Orc
{
	public float radius = 3f;
	public float shootInterval = 1f;
	private float lastCarrot;

	protected override void attack() {
		currentSpeed = 0;
		animator.SetBool("run", false);
		animator.SetBool("walk", false);
		walk(getDirectionToRabit());
		Debug.Log("shoot with carrot.");
		if (Time.time - lastCarrot > this.shootInterval) {
			launchCarrot(getDirectionToRabit());
			animator.SetTrigger("attack");
		}
	}

	void launchCarrot(float direction)
	{
		//Створюємо копію Prefab
		GameObject obj = GameObject.Instantiate(Resources.Load("weapon", typeof(GameObject)) as GameObject);
		//Розміщуємо в просторі
		obj.transform.position = this.transform.position;
		obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + .5f);
		//Запускаємо в рух
		Weapon carrot = obj.GetComponent<Weapon>();
		lastCarrot = Time.time;
		carrot.launch(direction);
	}

	protected override bool isRabitInOrcZone() {
		Vector2 rabbitPos = HeroRabit.lastRabit.transform.position;
		Vector2 orcPos = this.transform.position;
		if (Mathf.Abs(rabbitPos.x - orcPos.x) <= radius) {
			return true;
		}
		return false;
	}
}
