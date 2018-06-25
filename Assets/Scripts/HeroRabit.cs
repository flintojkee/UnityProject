using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRabit : MonoBehaviour
{

	public float speed = 1;
	public Animator animator;
	public float MaxJumpTime = 2f;
	public float JumpSpeed = 2f;
	public float SmallToBigRabitRate = 0.5f;

	Rigidbody2D myBody = null;
	bool isGrounded = false;
	bool JumpActive = false;
	float JumpTime = 0f;
	private Transform heroParent = null;
	private bool dead;
	private bool freezed;

	public static HeroRabit lastRabit = null;
	public static bool isRabitScaling = false;

	void Awake() {
		HeroRabit.lastRabit = this;
	}

	static void SetNewParent(Transform obj, Transform new_parent)
	{
		if (obj.transform.parent != new_parent)
		{
			//Засікаємо позицію у Глобальних координатах
			Vector3 pos = obj.transform.position;
			//Встановлюємо нового батька
			obj.transform.parent = new_parent;
			//Після зміни батька координати кролика зміняться
			//Оскільки вони тепер відносно іншого об’єкта
			//повертаємо кролика в ті самі глобальні координати
			obj.transform.position = pos;
		}
	}
	// Use this for initialization
	void Start()
	{
		myBody = this.GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		LevelController.current.setStartPosition(transform.position);
		this.heroParent = this.transform.parent;
	}

	// Update is called once per frame
	void Update()
	{
	}

	private void FixedUpdate()
	{
		if (this.freezed || this.dead) {
			Vector2 vel = myBody.velocity;
			vel.x = 0;
			myBody.velocity = vel;
			animator.SetBool("run", false);
			animator.SetBool("jump", false);
			return;
		}

		float value = Input.GetAxis("Horizontal");

		// Set velocity to hero
		if (Mathf.Abs(value) > 0)
		{
			Vector2 vel = myBody.velocity;
			vel.x = value * speed;
			myBody.velocity = vel;
		}

		// Flip hero
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (value < 0)
		{
			sr.flipX = true;
		}
		else if (value > 0)
		{
			sr.flipX = false;
		}

		// Enable run animation
		float horizontal = Input.GetAxis("Horizontal");
		if (Mathf.Abs(horizontal) > 0)
		{
			animator.SetBool("run", true);
		}
		else
		{
			animator.SetBool("run", false);
		}

		Vector3 from = transform.position + Vector3.up * 0.3f;
		Vector3 to = transform.position + Vector3.down * 0.1f;
		int layer_id = 1 << LayerMask.NameToLayer("Ground");
		//Перевіряємо чи проходить лінія через Collider з шаром Ground
		RaycastHit2D hit = Physics2D.Linecast(from, to, layer_id);
		if (hit)
		{
			isGrounded = true;
			if (hit.transform != null &&
				hit.transform.GetComponent<MovingPlatform>() != null) {
				SetNewParent(this.transform, hit.transform);
			}
		}
		else
		{
			isGrounded = false;
			SetNewParent(this.transform, this.heroParent);
		}
		//Намалювати лінію (для розробника)
		Debug.DrawLine(from, to, Color.red);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			this.JumpActive = true;
		}
		if (this.JumpActive)
		{
			//Якщо кнопку ще тримають
			if (Input.GetButton("Jump"))
			{
				this.JumpTime += Time.deltaTime;
				if (this.JumpTime < this.MaxJumpTime)
				{
					Vector2 vel = myBody.velocity;
					vel.y = JumpSpeed * (1.0f - JumpTime / MaxJumpTime);
					myBody.velocity = vel;
				}
			}
			else
			{
				this.JumpActive = false;
				this.JumpTime = 0;
			}
		}

		animator.SetBool("jump", !this.isGrounded);
	}

	IEnumerator die() {
		this.dead = true;
		if (!animator.GetBool("jump")) {
			animator.SetTrigger("death");
			yield return new WaitForSeconds(2f);
		}
		revive();
		animator.SetTrigger("idle");
		LevelController.current.onRabitDeath(HeroRabit.lastRabit);
	}

	public void freezeRabit() {
		this.freezed = true;
	}

	public void kill() {
		StartCoroutine(die());    
	}

	public void revive() {
		this.dead = false;
		this.freezed = false;
	}

	public bool isDead() {
		return dead;
	}
}