
using System.Collections.Generic;

public static class ItemMessage
{
	private static readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>
	{
		{ ItemMessageKey.CanNotUse, "오박사님의 말씀......\n{0}야(아)! 이런 것에는\n사용할 때가 따로 있는 법!" },
		{ItemMessageKey.NoEffect, "사용해도 효과가 없을껄"},
		{ItemMessageKey.HealHp, "{0}의 체력이\n{1} 회복되었다"},
		{ItemMessageKey.RestorePosition, "{0}의 독은\n깨끗이 사라졌다!"},
		{ItemMessageKey.RestoreBurn, "{0}의\n화상이 회복되었다"},
		{ItemMessageKey.RestoreFreeze, "{0}의 몸에\n얼음이 녹았다."},
		{ItemMessageKey.RestoreSleep, "{0}는(은)\n눈을 떴다"},
		{ItemMessageKey.RestoreParalysis, "{0}의 몸에\n마비가 사라졌다"},
		{ItemMessageKey.RestoreConfusion, "{0}는(은)\n혼란이 풀렸다."}
	};
	

	public static string Get(string key, params string[] args)
	{
		if (_dictionary.TryGetValue(key, out string template))
		{
			return string.Format(template, args);
		}

		return null;

	}
}
