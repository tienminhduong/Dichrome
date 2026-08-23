using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Awake()
    {
        Addressables.LoadSceneAsync(SceneDatabase.MAIN_MENU);
    }
}
