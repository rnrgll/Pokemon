
using System.Collections.Generic;

public static class ItemMessage
{
	private static readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>
	{
		{ ItemMessageKey.CanNotUse, "오박사님의 말씀......\n{0}야(아)! 이런 것에는\n사용할 때가 따로 있는 법!" },
		{ItemMessageKey.NoEffect, "사용해도 효과가 없을껄"}
	};

	public static string Get(string key)
	{
		if (_dictionary.TryGetValue(key, out string message))
		{
			return message;
		}

		return null;

	}
}
