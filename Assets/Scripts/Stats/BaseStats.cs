using System;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {   
        [Range(0, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] GameObject LevelUpParticle = null;
        [SerializeField] bool shouldUseModifiers = false;

        public event Action OnLevelUp;

        LazyValue<int> currentLevel;

        Experience experience;
        
        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if(experience != null)
            {
                experience.OnExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if(experience != null)
            {
                experience.OnExperienceGained -= UpdateLevel;
            }
        }

        private void Start()
        {
            currentLevel.value = CalculateLevel();
            
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                OnLevelUp();
                LevelUpEffect();
            }
        }


        private void LevelUpEffect()
        {
            Instantiate(LevelUpParticle, transform);
        }

        public float GetStat(Stat stat)
        {
            return  (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifiers(stat)/100);
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;

            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach(float modifier in provider.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifiers(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        public int CalculateLevel()
        {
            Experience exp = GetComponent<Experience>();

            if (exp == null) return startingLevel;

            float currentXP = exp.GetExperience();

            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUP, characterClass);

            for(int level = 1;level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUP, characterClass, level);

                if (XPToLevelUp > currentXP)
                    return level;
            }

            return penultimateLevel + 1;
        }
        
        public int GetLevel()
        {
            return currentLevel.value;
        }
    } 
}

