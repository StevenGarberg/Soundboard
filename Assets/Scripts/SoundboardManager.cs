using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SoundboardManager : MonoBehaviour
{
    public static SoundboardManager Instance { get; private set; }
    public UnityAction Play { get; private set; }
    
    [SerializeField] private GameObject _soundEffectButtonPrefab;
    [SerializeField] private Transform _soundEffectButtonParentTransform;
    [SerializeField] private Toggle _stopPreviousToggle, _loopToggle;
    [SerializeField] private Button _randomButton;
    private ushort _buttonCount;
    
    private readonly List<SoundEffectButton> _soundEffectButtons = new List<SoundEffectButton>();
    
    private bool ShouldStopPrevious => _stopPreviousToggle.isOn;
    
    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        Play += OnPlay;
        
        var soundEffects = Resources.LoadAll<AudioClip>("Sounds");
        if (soundEffects == null || !soundEffects.Any())
        {
            Debug.Log("No sound effects could be found.");
        }
        else
        {
            Debug.Log($"{soundEffects.Length} sound effects were found.");
            foreach (var soundEffect in soundEffects)
            {
                _soundEffectButtons.Add(Instantiate(_soundEffectButtonPrefab, _soundEffectButtonParentTransform)
                    .GetComponent<SoundEffectButton>()
                    .Construct(soundEffect));
                
                Debug.Log($"Sound effect button was created for '{soundEffect.name}'");
                _buttonCount++;
            }
        }
        
        _loopToggle.onValueChanged.AddListener(isOn =>
        {
            Debug.Log($"Loop toggled to {isOn}");
            foreach (var soundEffectButton in _soundEffectButtons)
            {
                Debug.Log($"Toggling loop for {soundEffectButton.Name} to {isOn}");
                soundEffectButton.SetLoop(isOn);
            }
        });
        
        _randomButton.onClick.AddListener(() =>
        {
            Debug.Log("Playing random");
            _soundEffectButtons[Random.Range(0, _buttonCount)].Play();
        });
    }

    private void OnPlay()
    {
        Debug.Log("OnPlay callback");
        if (ShouldStopPrevious)
        {
            Debug.Log("Stopping previous");
            foreach (var soundEffectButton in _soundEffectButtons.Where(b => b.IsPlaying))
            {
                Debug.Log($"Stopping '{soundEffectButton.Name}'");
                soundEffectButton.Stop();
            }
        }
    }
}