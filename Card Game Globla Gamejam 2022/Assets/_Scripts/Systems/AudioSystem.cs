using UnityEngine;

/// <summary>
/// Sistema de audio basico que soporta audio 3D.
/// Asegurarse de cambiar 'Sounds' audio source para usar 3D spatial blend si se quiere usar sonidos 3D.
/// </summary>
public class AudioSystem : Singleton<AudioSystem>
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _soundsSource;

    public void PlayMusic(AudioClip clip){
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip, Vector3 pos, float vol = 1){
        _soundsSource.transform.position = pos;
        PlaySound(clip, vol);
    }

    public void PlaySound(AudioClip clip, float vol = 1) {
        _soundsSource.PlayOneShot(clip, vol);
    }
}
