using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenOrc : Orc{

	protected override void attack()
	{
		if (isCloseToRabit()) {
			Debug.Log("Is closed to rabbit!");
			if (!LevelController.current.IsHeroBig()) HeroRabit.lastRabit.freezeRabit();
			currentSpeed = 0;
			animator.SetBool("walk", false);
			animator.SetBool("run", false);
			if (performingAttack || HeroRabit.lastRabit.isDead()) return;
			StartCoroutine(hitTheRabit());
		} else {
			animator.SetBool("walk", false);
			animator.SetBool("run", true);
			currentSpeed = runSpeed;
			walk(getDirectionToRabit());
		}
	}
}

