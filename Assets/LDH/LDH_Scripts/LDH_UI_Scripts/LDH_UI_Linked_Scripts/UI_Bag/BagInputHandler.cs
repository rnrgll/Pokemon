using UnityEngine;
using static Define;

// UI_Bag에서 받은 입력을 분석해 UI_Bag의 행동을 호출
public class BagInputHandler
{
	private readonly UI_Bag _bag;

	public BagInputHandler(UI_Bag bag)
	{
		_bag = bag;
	}

	// UIInputType에 따라 해당 동작을 호출 (UI_Bag 메서드)
	public void Handle(UIInputType inputType)
	{
		switch (inputType)
		{
			case UIInputType.Up:
				_bag.MoveCursor(-1);
				break;
			case UIInputType.Down:
				_bag.MoveCursor(1);
				break;
			case UIInputType.Left:
				_bag.MovePanel(-1);
				break;
			case UIInputType.Right:
				_bag.MovePanel(1);
				break;
			case UIInputType.Select:
				_bag.OnSelect();
				break;
			case UIInputType.Cancel:
				_bag.OnCancel();
				break;
		}
	}
}