using UnityEditor;
using UnityEngine.Video;
using UnityEngine;
using System.IO;

public class ClipLoader : MonoBehaviour
{
    public VideoClip scully;
    public VideoPlayer player;

    [MenuItem("Tools/Build Video AssetBundle")]
    static public void AssembleBundle()
    {
        string[] videoPaths = Directory.GetFiles("Assets/UI Assets/Scully", "*.webm", SearchOption.AllDirectories);

        foreach (string path in videoPaths)
        {
            string assetPath = path.Replace("\\", "/");
            var importer = AssetImporter.GetAtPath(assetPath);
            if (importer != null)
            {
                importer.assetBundleName = "video_bundle";
            }

            BuildPipeline.BuildAssetBundles("Assets/AssetBundles",
            BuildAssetBundleOptions.None,
            BuildTarget.StandaloneWindows64);

            Debug.Log("Video AssetBundle built!");
        }
    }
}
