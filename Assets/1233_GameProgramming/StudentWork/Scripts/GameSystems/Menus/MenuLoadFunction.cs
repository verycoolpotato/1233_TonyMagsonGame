using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MenuLoadFunction : MonoBehaviour
{
    [SerializeField] private AudioMixer MainAudioMixer;
    public void StartGame()
    {
        Invoke(nameof(timer), 3);
    }
    private void timer()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.InitializeGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeSFXVolume(float volume)
    {
        MainAudioMixer.SetFloat("SFXVolume", volume);
    }
    public void ChangeMusicVolume(float volume)
    {
        MainAudioMixer.SetFloat("MusicVolume", volume);
    }
}
