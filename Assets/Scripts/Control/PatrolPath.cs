using UnityEngine;

namespace RPG.Core
{
    public class PatrolPath : MonoBehaviour
    {
        const float wayPointGizmosRadius = .3f;

        private void OnDrawGizmos()
        {
            for(int i=0;i<transform.childCount;i++)
            {
                Gizmos.DrawSphere(GetWayPoint(i), wayPointGizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i), GetWayPoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i == transform.childCount - 1)
                return 0;
            else
                return i + 1;
        }
        public Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }

}