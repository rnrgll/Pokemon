using UnityEngine;
using static Define;

public class BagInputHandler
{
	private readonly UI_Bag _bag;

	public BagInputHandler(UI_Bag bag)
	{
		_bag = bag;
	}

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
				_bag.OnCancle();
				break;
		}
	}
}