using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
	[SerializeField] private TMP_Text playTimeText;

	
    // Start is called before the first frame update
    void Start()
    {
	    float totalPlayTime = Manager.Data.PlayerData.GetPlayTime();
	    playTimeText.text = "PlayTime" + Util.FormatTimeHM(totalPlayTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
