using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace WinterUniverse
{
    public class PlayerInputManager : MonoBehaviour
    {
        private PlayerInputActions _inputActions;
        private Vector2 _moveInput;
        private Vector2 _lookInput;
        private bool _runInput;

        public Vector2 MoveInput => _moveInput;
        public Vector2 LookInput => _lookInput;

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }
            }
        }

        public void Initialize()
        {
            _inputActions = new();
            SceneManager.activeSceneChanged += OnSceneChanged;
            //Enable();
        }

        private void Enable()
        {
            enabled = true;
            _inputActions.Enable();
        }

        private void Disable()
        {
            _inputActions.Disable();
            enabled = false;
            _moveInput = Vector2.zero;
            _lookInput = Vector2.zero;
            _runInput = false;
        }

        private void OnSceneChanged(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == 0)
            {
                Disable();
            }
            else
            {
                Enable();
            }
        }

        private void Update()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            HandleRunInput();
        }

        private void HandleRunInput()
        {
            if (_moveInput != Vector2.zero && _runInput && GameManager.StaticInstance.Player.PlayerLocomotion.HandleRunning())
            {
                GameManager.StaticInstance.Player.IsRunning = true;
            }
            else
            {
                GameManager.StaticInstance.Player.IsRunning = false;
            }
        }

        public void OnMove(InputValue value)
        {
            _moveInput = value.Get<Vector2>();
        }

        public void OnLook(InputValue value)
        {
            _lookInput = value.Get<Vector2>();
        }

        public void OnRun(InputValue value)
        {
            _runInput = value.isPressed;
        }

        public void OnDash()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            GameManager.StaticInstance.Player.PlayerLocomotion.TryPerformDash();
        }

        public void OnInteract()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            GameManager.StaticInstance.Player.PlayerInteraction.Interact();
        }

        public void OnJump()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            GameManager.StaticInstance.Player.PlayerLocomotion.TryPerformJump();
        }

        public void OnActionPrimaryRight()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            GameManager.StaticInstance.Player.PawnCombat.UseWeaponAbility(GameManager.StaticInstance.Player.PawnEquipment.WeaponRightSlot.Data, HandSlotType.Right, GameManager.StaticInstance.Player.PawnEquipment.WeaponRightSlot.Data.PrimaryAbility);
        }

        public void OnActionPrimaryLeft()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            if (GameManager.StaticInstance.Player.PawnEquipment.WeaponRightSlot.Data.WeaponHandType == WeaponHandType.TwoHand)
            {
                OnActionPrimaryRight();
            }
            else
            {
                GameManager.StaticInstance.Player.PawnCombat.UseWeaponAbility(GameManager.StaticInstance.Player.PawnEquipment.WeaponLeftSlot.Data, HandSlotType.Left, GameManager.StaticInstance.Player.PawnEquipment.WeaponLeftSlot.Data.PrimaryAbility);
            }
        }

        public void OnActionSecondaryRight()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            GameManager.StaticInstance.Player.PawnCombat.UseWeaponAbility(GameManager.StaticInstance.Player.PawnEquipment.WeaponRightSlot.Data, HandSlotType.Right, GameManager.StaticInstance.Player.PawnEquipment.WeaponRightSlot.Data.SecondaryAbility);
        }

        public void OnActionSecondaryLeft()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            if (GameManager.StaticInstance.Player.PawnEquipment.WeaponRightSlot.Data.WeaponHandType == WeaponHandType.TwoHand)
            {
                OnActionSecondaryRight();
            }
            else
            {
                GameManager.StaticInstance.Player.PawnCombat.UseWeaponAbility(GameManager.StaticInstance.Player.PawnEquipment.WeaponLeftSlot.Data, HandSlotType.Left, GameManager.StaticInstance.Player.PawnEquipment.WeaponLeftSlot.Data.SecondaryAbility);
            }
        }

        public void OnCastSpell()
        {
            if (GameManager.StaticInstance.Player == null)
            {
                return;
            }
            GameManager.StaticInstance.Player.PawnCombat.UseSpellAbility(GameManager.StaticInstance.Player.PawnEquipment.SpellData);
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }
}