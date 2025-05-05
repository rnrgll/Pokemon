
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
		{ItemMessageKey.RestoreConfusion, "{0}는(은)\n혼란이 풀렸다."},
		{ItemMessageKey.CanNotUseBall, "트레이너가 볼을 쳐냈다!\n다른사람의 물건을 훔치면 도둑놈!"},
		{ItemMessageKey.TMHMBeforeUse, "기술 머신을 가동시켰다!\n \n안에는 {0}(이)가\n기록되어져 있다!\n{0}를(을)\n포켓몬에게 가르치겠습니까?"},
		{ItemMessageKey.CanNotLearn,"{0}과(와) {1}는(은)\n상성이 좋지 않았다!\n{1}은(는) 배울 수 없다!"},
		{ItemMessageKey.LearnFail,
			"{0}는(은) 새로\n{1}를(을) 배우고 싶다...!\n그러나 {0}는(은)\n기술을 4개\n기억하고 있기에 \n더 이상은 무리다\n다른 기술을 잊게 하겠습니까?"},
		{ItemMessageKey.ForgetSkill, "1  2    ....... 짠!\n \n{0}는(은) {1}의\n사용방법을 깨끗이 잊었다\n그리고......!"},
		{ItemMessageKey.LearnSuccess,"{0}는(은) 새로\n{1}를(을) 배웠다"},
		{ItemMessageKey.AlreadyKnow, "{0}는(은) 이미 \n{1}를(을) 알고 있습니다."}
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
