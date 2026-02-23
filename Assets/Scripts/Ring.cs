using UnityEngine;

namespace PlanetTweaks2.UI
{
    public class Ring : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(Vector3.back, Time.deltaTime * 30);
        }
    }
}