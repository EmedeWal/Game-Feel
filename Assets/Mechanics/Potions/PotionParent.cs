using ShatterStep.Player;
using System.Collections;
using UnityEngine;

namespace ShatterStep
{
    public class PotionParent : MonoBehaviour
    {
        private AudioSystem _audioSystem;

        [Header("REFERENCES")]
        [SerializeField] private AudioData _potionData;

        [Header("SETTINGS")]
        [SerializeField] private float _pickupDelay = 3;

        private AudioSource _audioSource;
        private Trigger[] _potionArray;

        public void Setup()
        {
            _audioSystem = AudioSystem.Instance;

            _audioSource = GetComponent<AudioSource>();
            _potionArray = GetComponentsInChildren<Trigger>();
            foreach (Trigger potion in _potionArray)
            {
                potion.PlayerEntered += PotionParent_PlayerEntered;
                potion.Init();
            }
        }

        public void Cleanup()
        {
            foreach (Trigger potion in _potionArray)
            {
                potion.PlayerEntered -= PotionParent_PlayerEntered;
            }
        }

        private void PotionParent_PlayerEntered(Manager player, Trigger trigger)
        {
            player.Data.RefreshAbilities();

            StartCoroutine(PotionCoroutine(trigger.gameObject, _pickupDelay));
        }

        private IEnumerator PotionCoroutine(GameObject trigger, float delay)
        {
            _audioSystem.Play(_potionData, _audioSource);

            trigger.SetActive(false);

            yield return new WaitForSeconds(delay);

            trigger.SetActive(true);
        }
    }
}