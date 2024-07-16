using RPG.Attribute;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience XP;
        // Start is called before the first frame update
        void Awake()
        {
            XP = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        // Update is called once per frame
        void Update()
        {
            GetComponent<Text>().text = string.Format("{0:0}", XP.GetExperience());

        }
    }
}


