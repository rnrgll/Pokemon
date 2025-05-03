public class OpenLinkedUIAction : ISelectableAction
{
	private string _path;

	public OpenLinkedUIAction(string path)
	{
		_path = path;
	}

	public void Execute()
	{
		Manager.UI.ShowLinkedUI<UI_Linked>(_path);
	}
}