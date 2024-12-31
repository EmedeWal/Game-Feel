using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace ShatterStep
{
    namespace UI
    {
        public class DialogueManager : MonoBehaviour
        { 
            [Header("REFERENCES")]
            [SerializeField] private TextMeshProUGUI _dialogueText;
            [SerializeField] private Button _previousButton;
            [SerializeField] private Button _nextButton;
            [SerializeField] private Button _skipButton;
            [SerializeField] private Image _background;

            [Header("SETTINGS")]
            [SerializeField] private float _typingSpeed = 0.05f;

            private DialogueData _data;
            private int _currentLineIndex = 0;
            private bool _isTyping = false;

            public void Initialize(DialogueData data)
            {
                _data = data;

                _background.sprite = data.BackgroundSprite;

                _previousButton.onClick.AddListener(OnPreviousButtonPressed);
                _nextButton.onClick.AddListener(OnNextButtonPressed);
                _skipButton.onClick.AddListener(OnSkipButtonPressed);

                UpdateButtonStates();
                ShowDialogue();                
            }

            private void OnDisable()
            {
                _previousButton.onClick.RemoveListener(OnPreviousButtonPressed);
                _nextButton.onClick.RemoveListener(OnNextButtonPressed);
                _skipButton.onClick.RemoveListener(OnSkipButtonPressed);
            }

            private void ShowDialogue() => StartCoroutine(TypeDialogue(_data.Dialogue[_currentLineIndex]));

            private IEnumerator TypeDialogue(string line)
            {
                _isTyping = true;
                _dialogueText.text = "";

                foreach (char c in line.ToCharArray())
                {
                    _dialogueText.text += c;
                    yield return new WaitForSeconds(_typingSpeed);
                }

                _isTyping = false;
            }

            private void OnSkipButtonPressed()
            {
                SceneLoader.Instance.LoadNextScene();
            }

            private void OnNextButtonPressed()
            {
                if (_isTyping)
                {
                    StopAllCoroutines();
                    _dialogueText.text = _data.Dialogue[_currentLineIndex];
                    _isTyping = false;
                    return;
                }

                _currentLineIndex++;
                if (_currentLineIndex < _data.Dialogue.Length)
                {
                    ShowDialogue();
                }
                else
                {
                    SceneLoader.Instance.LoadNextScene();
                }

                UpdateButtonStates();
            }

            private void OnPreviousButtonPressed()
            {
                if (_isTyping)
                {
                    StopAllCoroutines();
                    _dialogueText.text = _data.Dialogue[_currentLineIndex];
                    _isTyping = false;
                    return;
                }

                if (_currentLineIndex > 0)
                {
                    _currentLineIndex--;
                    ShowDialogue();
                }

                UpdateButtonStates();
            }

            private void UpdateButtonStates()
            {
                _previousButton.interactable = _currentLineIndex > 0;
                _nextButton.interactable = _currentLineIndex < _data.Dialogue.Length;
            }
        }
    }
}