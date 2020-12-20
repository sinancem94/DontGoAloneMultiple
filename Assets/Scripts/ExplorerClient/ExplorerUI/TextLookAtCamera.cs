using UnityEngine;

namespace ExplorerClient.ExplorerUI
{
    public class TextLookAtCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = ExplorerCamera.instance.myCamera.transform.rotation;
        }
    }
}
