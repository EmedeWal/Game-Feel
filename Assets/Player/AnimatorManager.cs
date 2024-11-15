using UnityEngine;

namespace Custom
{
    namespace Player
    {
        public class AnimatorManager : MonoBehaviour
        {
            private Animator _animator;
            private int _locomotionHash;
            private int _jumpHash;
            private int _fallHash;
            private int _hangHash;

            public void Init()
            {
                _animator = GetComponentInChildren<Animator>();

                _locomotionHash = Animator.StringToHash("Locomotion");
                _jumpHash = Animator.StringToHash("Jump");
                _fallHash = Animator.StringToHash("Fall");
                _hangHash = Animator.StringToHash("Hang");
            }

            public void FixedTick(LocomotionState locomotionState, float inputMagnitude)
            {
                _animator.SetFloat(_locomotionHash, inputMagnitude);

                switch (locomotionState)
                {
                    case LocomotionState.Grounded:
                        Play(Animator.StringToHash("Locomotion"));
                        break;

                    case LocomotionState.Jumping:
                        Play(Animator.StringToHash("Jump"));
                        break;

                    case LocomotionState.Falling:
                        Play(Animator.StringToHash("Fall"));
                        break;

                    case LocomotionState.Hanging:
                        Play(Animator.StringToHash("Hang"));
                        break;
                }
            }

            private void Play(int hash)
            {
                AnimatorStateInfo animatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                if (hash != animatorStateInfo.shortNameHash) _animator.PlayInFixedTime(hash);
            }
        }
    }
}