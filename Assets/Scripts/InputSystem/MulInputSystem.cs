using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MulInputSystem
{
    public class MulInputSystem : MonoBehaviour
    {
        public static MulInputSystem instance;
        public static PlayerActionMap playerActionMap;

        //输入预设
        
        public static float RawOffset = 0.1f;

        private static Dictionary<string, InputAction> inputActionDatas;

        private void OnEnable()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                enabled = false;
                return;
            }

            if (playerActionMap == null)
                playerActionMap = new PlayerActionMap();

            if(inputActionDatas == null)
                inputActionDatas = new();
            EnableInput(PlayerTag.Player1);
        }

        private void OnDestroy()
        {
            if (enabled)
            {
                instance = null;
                playerActionMap = null;
                inputActionDatas.Clear();
            }
        }
        #region ActionMap关联
        public void EnableInput(PlayerTag _playerTag)
        {
            switch (_playerTag)
            {
                case PlayerTag.Player1:
                    playerActionMap.Player1.Enable();
                    break;
                case PlayerTag.Gamepad:
                    playerActionMap.Gamepad.Enable();
                    break;
            }
        }
        #endregion

        #region 接口

        //====================基础输入====================
        /// <summary>
        /// 获取按键输入
        /// </summary>
        public static bool GetInput_Button(PlayerTag _playerTag, InputType _inputType)
        {
            InputAction action = FindAction(_playerTag, _inputType);
            if(action == null) return false;

            return action.ReadValue<float>() == 1;
        }

        /// <summary>
        /// 获取轴输入（平滑）
        /// </summary>
        public static float GetInput_Axis(PlayerTag _playerTag, InputType _inputType)
        {
            InputAction action = FindAction(_playerTag, _inputType);
            if (action == null) return 0f;

            return action.ReadValue<float>();
        }

        /// <summary>
        /// 获取轴输入（阶梯）
        /// </summary>
        public static float GetInput_AxisRaw(PlayerTag _playerTag, InputType _inputType)
        {
            InputAction action = FindAction(_playerTag, _inputType);
            if (action == null) return 0f;

            return MapToRaw(action.ReadValue<float>(), RawOffset);
        }



        //====================事件绑定====================
        public static void AddBinding_started(PlayerTag _playerTag, InputType _inputType,Action<InputAction.CallbackContext> _inputAction)
        {
            FindAction(_playerTag, _inputType).started += _inputAction;
        }

        public static void AddBinding_performed(PlayerTag _playerTag, InputType _inputType, Action<InputAction.CallbackContext> _inputAction)
        {
            FindAction(_playerTag, _inputType).performed += _inputAction;
        }

        public static void AddBinding_canceled(PlayerTag _playerTag, InputType _inputType, Action<InputAction.CallbackContext> _inputAction)
        {
            FindAction(_playerTag, _inputType).canceled += _inputAction;
        }

        public static void RemoveBinding_started(PlayerTag _playerTag, InputType _inputType, Action<InputAction.CallbackContext> _inputAction)
        {
            FindAction(_playerTag, _inputType).started -= _inputAction;
        }

        public static void RemoveBinding_performed(PlayerTag _playerTag, InputType _inputType, Action<InputAction.CallbackContext> _inputAction)
        {
            FindAction(_playerTag, _inputType).performed -= _inputAction;
        }

        public static void RemoveBinding_canceled(PlayerTag _playerTag, InputType _inputType, Action<InputAction.CallbackContext> _inputAction)
        {
            FindAction(_playerTag, _inputType).canceled -= _inputAction;
        }

        /// <summary>
        /// 查找输入事件
        /// </summary>
        public static InputAction FindAction(PlayerTag _playerTag, InputType _inputType)
        {
            InputAction action;
            string name = _playerTag.ToString() + "_" + _inputType;
            if (inputActionDatas.TryGetValue(name, out action))
            {

            }
            else
            {
                action = playerActionMap.FindAction(name);
                inputActionDatas[name] = action;
            }
            return action;
        }

        #endregion

        private static float MapToRaw(float _num,float _offset)
        {
            if (_num > _offset)
                return 1;
            else if (_num < -_offset)
                return -1;
            else
                return 0;
        }

    }



}
