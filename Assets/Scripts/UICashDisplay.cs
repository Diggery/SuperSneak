using UnityEngine;
using System.Collections;

public class UICashDisplay : MonoBehaviour {
	
	TextMesh jewelAmount;
	TextMesh cashAmount;

	public void setUp (GameObject cashFieldsPrefab) {
		GameObject cashFields = Instantiate(cashFieldsPrefab, transform.position,  transform.rotation) as GameObject;
		cashFields.transform.parent = transform;
		cashFields.transform.localPosition = Vector3.zero;
		cashFields.transform.localScale = Vector3.one;
		Transform jewelAmountField = cashFields.transform.Find("JewelAmount");
		jewelAmountField.renderer.material.renderQueue = 4000;
		jewelAmount = jewelAmountField.GetComponent<TextMesh>();
		Transform cashAmountField = cashFields.transform.Find("CashAmount");
		cashAmountField.renderer.material.renderQueue = 4000;
		cashAmount = cashAmountField.GetComponent<TextMesh>();

	}
	
	public void setJewelAmount(int newAmount) {
		jewelAmount.text = newAmount.ToString();
	}
	
	public void setCashAmount(int newAmount) {
		cashAmount.text = newAmount.ToString();
	}
}
