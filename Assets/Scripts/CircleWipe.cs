using UnityEngine;
using UnityEngine.Video;

public class CircleWipe : MonoBehaviour
{
    public void Wipe() {
        GetComponent<VideoPlayer>().Play();
    }
}
