using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static LevelController current;
	Vector3 startingPosition;
	private int coinsNumber;
	private bool isHeroBig;

	public void Awake()
	{
		current = this;
	}

	public void setStartPosition(Vector3 pos)
	{
		this.startingPosition = pos;
	}
	public void onRabitDeath(HeroRabit rabit)
	{
		//При смерті кролика повертаємо на початкову позицію
		rabit.transform.position = this.startingPosition;
	}

	public void addCoins(int coinsNumber) {
		this.coinsNumber += coinsNumber;
	}

	public void makeBigger(HeroRabit rabit) {
		float scaleRate = rabit.SmallToBigRabitRate;
		rabit.transform.localScale += new Vector3(scaleRate, scaleRate, 0);
		isHeroBig = true;
	}

	public void makeSmaller(HeroRabit rabit) {
		float scaleRate = rabit.SmallToBigRabitRate;
		rabit.transform.localScale -= new Vector3(scaleRate, scaleRate, 0);
		isHeroBig = false;
	}

	public bool IsHeroBig() {
		return this.isHeroBig;
	}
}