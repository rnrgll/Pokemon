using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SoundData
{
	public string key;
	public AudioClip sound;
}

public class SoundManager : MonoBehaviour
{
	[SerializeField] AudioSource audioSource;

	[SerializeField] SoundData[] soundData;
	Dictionary<string, AudioClip> soundMap;
	[SerializeField] Player player;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();

		// 배열 > 딕셔너리
		soundMap = new Dictionary<string, AudioClip>();
		foreach (var data in soundData)
		{
			if (!soundMap.ContainsKey(data.key))
				soundMap.Add(data.key, data.sound);
		}
	}

	void Start()
	{
		player = GetComponentInParent<Player>();
		player.OnSceneChangeEvent += Play;
	}

	public void Play(string key)
	{
		if (soundMap.TryGetValue(key, out AudioClip sound))
		{
			// 이미 재생중이면 return
			if (audioSource.clip == sound && audioSource.isPlaying)
			{
				Debug.Log($"사운드매니저 : {key} 사운드는 이미 재생중");
				return;
			}

			Debug.Log($"사운드매니저 : {key} 사운드 재생");
			audioSource.clip = sound;
			audioSource.Play();
		}
		else
		{
			Debug.LogWarning($"사운드매니저 : {key} 에 해당하는 사운드가 없습니다.");
		}
	}
}
