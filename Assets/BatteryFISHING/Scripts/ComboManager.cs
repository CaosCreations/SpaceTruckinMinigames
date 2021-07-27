using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour {

	public static ComboManager instance;

	public Text comboText;

	public float resetTime = 2f;

	private Animator comboTextAnimator;

	private int totalCombo;

	private void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {

		comboTextAnimator = comboText.GetComponent<Animator>();

	}
	
	public void SetCombo()
	{
		totalCombo++;
		comboText.text = "x" + totalCombo;
		comboTextAnimator.SetTrigger("Hit");

		CancelInvoke();
		Invoke("ResetCombo", 2f);
	}

	public void ResetCombo()
	{
		totalCombo = 0;
	}
}
