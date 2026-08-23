using Cysharp.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    //play sound, on ended, return to pool
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        UniTask.Delay((int)(clip.length * 1000)).ContinueWith(() =>
        {
            AudioManager.Instance.ReturnSoundEmitter(this);
        });
    }
}