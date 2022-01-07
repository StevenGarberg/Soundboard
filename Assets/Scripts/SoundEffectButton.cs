using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class SoundEffectButton : MonoBehaviour
{
    private GameObject _gameObject;
    private Button _button;
    private TextMeshProUGUI _text;
    private AudioSource _audioSource;

    public string Name => _text.text;
    public bool IsPlaying => _audioSource.isPlaying;

    private void Awake()
    {
        _gameObject = gameObject;
        _button = GetComponent<Button>();
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            Debug.Log("Sound effect button was clicked");
            Play();
        });
    }

    public SoundEffectButton Construct(AudioClip clip)
    {
        _gameObject.name = $"{_gameObject.name} - {clip.name}";
        _audioSource.clip = clip;
        _text.SetText(clip.name);
        return this;
    }

    public void Play()
    {
        if (_audioSource.clip != null)
        {
            Debug.Log($"Playing {_audioSource.clip.name}");
            SoundboardManager.Instance.Play?.Invoke();
            _audioSource.Play();
        }
    }
    
    public void Stop()
    {
        _audioSource.Stop();
    }

    public void SetLoop(bool shouldLoop)
    {
        _audioSource.loop = shouldLoop;
    }
}
