using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Collectable
{
	protected override void OnRabitHit(HeroRabit rabit) {
		if (LevelController.current.IsHeroBig()) {
			LevelController.current.makeSmaller(rabit);
		} else {
			rabit.kill();
		}
		this.CollectedHide();
	}
}