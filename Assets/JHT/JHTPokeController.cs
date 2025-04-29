using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

[CreateAssetMenu(fileName = "PokeObject", menuName = "Pokes / poke")]
public class JHTPokeController : ScriptableObject
{
    [SerializeField]
    ProgressionPokeClass[] progressionPokeClasses = null;


    public int GetStat(JHTStat stat, JHTPokeType pokeType, int level)
    {
        foreach (ProgressionPokeClass pokeClass in progressionPokeClasses)
        {
            if (pokeClass.pokeType != pokeType) continue;

            foreach(ProgressStat progressStat in pokeClass.stats)
            {
                if(progressStat.stat != stat) continue;
                if(progressStat.levels.Length <  level) continue;

                return progressStat.levels[level - 1];

            }
        }
        return 0;
    }


    [System.Serializable]
    class ProgressionPokeClass
    {
        public JHTPokeType pokeType;
        public ProgressStat[] stats;
    }

    [System.Serializable]
    class ProgressStat
    {
        public JHTStat stat;
        public int[] levels;
    }
}
