using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

[CreateAssetMenu(fileName = "PokeObject", menuName = "Pokes / poke")]
public class PokeController : ScriptableObject
{
    [SerializeField]
    ProgressionPokeClass[] progressionPokeClasses = null;


    public int GetStat(Stat stat,PokeType pokeType, int level)
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
        public PokeType pokeType;
        public ProgressStat[] stats;
    }

    [System.Serializable]
    class ProgressStat
    {
        public Stat stat;
        public int[] levels;
    }
}
