using System;
using EventHabs;
using UnityEngine;
using UnityEngine.UI;
using VBCM;

public class BarInfoView : MonoBehaviour, ChangePointEvent.IBindable
{
	[SerializeField] private Text _pointText;

	public BarInfoView()
	{
		ChangePointEvent.Builder(this).CallBack(SetPoint).Build();
	}

	private void SetPoint(int count)
	{
		_pointText.text = count.ToString();
	}

	public Hub<ChangePointEvent, int, int>.ActionsPack BindActions { get; set; }
}
