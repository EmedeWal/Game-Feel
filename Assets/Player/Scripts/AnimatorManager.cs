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
            private int _dashHash;

            public void Init()
            {
                _animator = GetComponentInChildren<Animator>();

                _locomotionHash = Animator.StringToHash("Locomotion");
                _jumpHash = Animator.StringToHash("Jump");
                _fallHash = Animator.StringToHash("Fall");
                _hangHash = Animator.StringToHash("Hang");
                _dashHash = Animator.StringToHash("Dash");
            }

            public void FixedTick(LocomotionState locomotionState, float inputMagnitude)
            {
                _animator.SetFloat(_locomotionHash, inputMagnitude);

                switch (locomotionState)
                {
                    case LocomotionState.Grounded:
                        Play(_locomotionHash);
                        break;

                    case LocomotionState.Bouncing:
                        Play(_jumpHash);
                        break;

                    case LocomotionState.Jumping:
                        Play(_jumpHash);
                        break;

                    case LocomotionState.Falling:
                        Play(_fallHash);
                        break;

                    case LocomotionState.Hanging:
                        Play(_hangHash);
                        break;

                    case LocomotionState.Dashing:
                        Play(_dashHash);
                        break;
                }
            }

            private void Play(int hash)
            {
                AnimatorStateInfo currentAnimation = _animator.GetCurrentAnimatorStateInfo(0);
                if (hash != currentAnimation.shortNameHash) _animator.PlayInFixedTime(hash);
            }
        }
    }
}