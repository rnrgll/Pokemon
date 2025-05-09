using System;

public class CustomAction : ISelectableAction
{
	private Action _action;

	public CustomAction(Action action)
	{
		_action = action;
	}

	public void Execute()
	{
		_action?.Invoke();
	}
}