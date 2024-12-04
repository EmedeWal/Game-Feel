using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShatterStep
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Button previousButton;
        [SerializeField] private Button nextButton;

        [Header("Settings")]
        [SerializeField] private float typingSpeed = 0.05f;
        [TextArea][SerializeField] private string[] dialogueLines;

        private int currentLineIndex = 0;
        private bool isTyping = false;

        private void Start()
        {
            previousButton.onClick.AddListener(OnPreviousButtonPressed);
            nextButton.onClick.AddListener(OnNextButtonPressed);

            UpdateButtonStates();
            ShowDialogue();
        }

        private void ShowDialogue()
        {
            StartCoroutine(TypeDialogue(dialogueLines[currentLineIndex]));
        }

        private IEnumerator TypeDialogue(string line)
        {
            isTyping = true;
            dialogueText.text = "";

            foreach (char c in line.ToCharArray())
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
        }

        private void OnNextButtonPressed()
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
                return;
            }

            currentLineIndex++;
            if (currentLineIndex < dialogueLines.Length)
            {
                ShowDialogue();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

            UpdateButtonStates();
        }

        private void OnPreviousButtonPressed()
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
                return;
            }

            if (currentLineIndex > 0)
            {
                currentLineIndex--;
                ShowDialogue();
            }

            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            previousButton.interactable = currentLineIndex > 0;
            nextButton.interactable = currentLineIndex < dialogueLines.Length;
        }
    }
}