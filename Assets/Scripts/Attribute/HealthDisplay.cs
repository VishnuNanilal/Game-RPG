using RPG.Attribute;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attribute
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        // Start is called before the first frame update
        void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            float percHealth = health.GetPercentageHealth();
            GetComponent<Text>().text = string.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());

        }
    }
}


