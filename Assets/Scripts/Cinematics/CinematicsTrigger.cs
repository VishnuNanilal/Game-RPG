using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicsTrigger : MonoBehaviour
    {
        bool alreadyTriggered;
        void Start()
        {
            alreadyTriggered = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player"||alreadyTriggered) return;

            GetComponent<PlayableDirector>().Play();
            alreadyTriggered = true;

        }

    }
}
