using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collectable
{
	public GameObject prefabCarrot;

	private float direction;

	void Start() {
		StartCoroutine(destroyLater());
	}
	protected override void OnRabitHit(HeroRabit rabit)
	{
		if (HeroRabit.lastRabit.isDead()) return;
		if (LevelController.current.IsHeroBig())
		{
			LevelController.current.makeSmaller(HeroRabit.lastRabit);
		}
		else
		{
			HeroRabit.lastRabit.kill();
		}
		this.CollectedHide();
	}

	void FixedUpdate() {
		this.transform.position = new Vector2(
			this.transform.position.x + direction * 0.1f,
			this.transform.position.y
		);
	}

	public void launch(float direction)
	{
		Debug.Log("Carrot was launched.");
		this.direction = direction;
	}

	IEnumerator destroyLater()
	{
		yield return new WaitForSeconds(3.0f);
		Destroy(this.gameObject);
	}

}