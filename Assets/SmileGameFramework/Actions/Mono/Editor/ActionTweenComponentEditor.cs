using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SmileGame.Action
{
    [CustomEditor(typeof(ActionTweenComponent))]
    public class ActionTweenComponentEditor : Editor
    {
        private ActionTweenComponent m_BaseActionTweenComponent;

        #region 通用动画属性

        private SerializedProperty m_Duration;
        private SerializedProperty m_ActionDirection;
        private SerializedProperty m_ActionType;
        private SerializedProperty m_EaseEnum;
        private SerializedProperty m_ActionCurve;
        private SerializedProperty m_LoopTime;
        private SerializedProperty m_ScaleTime;
        private SerializedProperty m_Speed;
        private SerializedProperty m_UseSpeed;
        private SerializedProperty m_AutoPlay;
        private SerializedProperty m_AutoKill;
        private SerializedProperty m_Relative;

        #endregion

        #region 组件属性

        public SerializedProperty m_AnimationType;
        public SerializedProperty m_EnableEditorInRuntime;

        #region 通用动画回调事件

        public SerializedProperty OnBeginPlayEvent;
        public SerializedProperty OnCompleteEvent;
        public SerializedProperty OnKillEvent;
        public SerializedProperty OnLoopCompleteEvent;
        public SerializedProperty OnStepUpdateEvent;

        #endregion

        #endregion

        #region 具体动画属性

        #region Move Action 

        public SerializedProperty m_MoveToType;
        public SerializedProperty m_MoveTargetPos;
        public SerializedProperty m_MoveTargetTransform;

        #endregion

        #region Rotation Action

        public SerializedProperty m_RotToType;
        public SerializedProperty m_RotToTarget;
        public SerializedProperty m_RotToVec;
        public SerializedProperty m_RotModel;

        #endregion

        #region Scale Action

        public SerializedProperty m_ScaleTarget;

        #endregion

        #region Color Action

        public SerializedProperty m_ColorType;
        public SerializedProperty m_TarCol;

        #endregion

        #region Fade Action

        public SerializedProperty m_FadeType;
        public SerializedProperty m_FadeTarAlpha;

        #endregion

        #endregion

        #region Private Property

        private bool m_IsOpenActionEventSetting = false;

        #endregion

        #region Editor API

        private ActionTweenComponent GetTarget()
        {
            if (m_BaseActionTweenComponent == null)
            {
                m_BaseActionTweenComponent = target as ActionTweenComponent;
            }
            return m_BaseActionTweenComponent;
        }

        private void OnEnable()
        {
            m_Duration = serializedObject.FindProperty("m_Duration");
            m_ActionDirection = serializedObject.FindProperty("m_ActionDirection");
            m_ActionType = serializedObject.FindProperty("m_ActionType");
            m_EaseEnum = serializedObject.FindProperty("m_EaseEnum");
            m_ActionCurve = serializedObject.FindProperty("m_ActionCurve");
            m_LoopTime = serializedObject.FindProperty("m_LoopTime");
            m_ScaleTime = serializedObject.FindProperty("m_ScaleTime");
            m_Speed = serializedObject.FindProperty("m_Speed");
            m_UseSpeed = serializedObject.FindProperty("m_UseSpeed");
            m_AutoPlay = serializedObject.FindProperty("m_AutoPlay");
            m_AutoKill = serializedObject.FindProperty("m_AutoKill");
            m_Relative = serializedObject.FindProperty("m_Relative");

            // animation Type
            m_AnimationType = serializedObject.FindProperty("m_AnimationType");
            m_EnableEditorInRuntime = serializedObject.FindProperty("m_EnableEditorInRuntime");

            // animation Event
            OnBeginPlayEvent = serializedObject.FindProperty("OnBeginPlayEvent");
            OnCompleteEvent = serializedObject.FindProperty("OnCompleteEvent");
            OnKillEvent = serializedObject.FindProperty("OnKillEvent");
            OnLoopCompleteEvent = serializedObject.FindProperty("OnLoopCompleteEvent");
            OnStepUpdateEvent = serializedObject.FindProperty("OnStepUpdateEvent");

            // move 
            m_MoveToType = serializedObject.FindProperty("m_MoveToType");
            m_MoveTargetPos = serializedObject.FindProperty("m_MoveTargetPos");
            m_MoveTargetTransform = serializedObject.FindProperty("m_MoveTargetTransform");

            // rotation
            m_RotToType = serializedObject.FindProperty("m_RotToType");
            m_RotToTarget = serializedObject.FindProperty("m_RotToTarget");
            m_RotToVec = serializedObject.FindProperty("m_RotToVec");
            m_RotModel = serializedObject.FindProperty("m_RotModel");

            // scale 
            m_ScaleTarget = serializedObject.FindProperty("m_ScaleTarget");

            // color
            m_ColorType = serializedObject.FindProperty("m_ColorType");
            m_TarCol = serializedObject.FindProperty("m_TarCol");

            // fade
            m_FadeType = serializedObject.FindProperty("m_FadeType");
            m_FadeTarAlpha = serializedObject.FindProperty("m_FadeTarAlpha");

        }

        private void OnDisable()
        {

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (EditorApplication.isPlaying)
            {
                if (GetTarget().m_EnableEditorInRuntime)
                {
                    OnActionGeneralInspectorGUI();
                    OnActionTypeInspectorGUI();
                }
                OnActionControllerInspectorGUI();
            }
            else
            {
                OnActionGeneralInspectorGUI();
                OnActionTypeInspectorGUI();
            }
            // EditorGUILayout.Space();

            ApplyModify();
        }

        private void OnActionGeneralInspectorGUI()
        {
            OnActionAnimationTypeInspectorGUI();

            OnActionGeneralValueInspectorGUI();

            OnActionGeneralEventInspectorGUI();
        }

        /// <summary>
        /// 动画类型显示
        /// </summary>
        private void OnActionAnimationTypeInspectorGUI()
        {
            // EditorGUILayout.BeginHorizontal();

            GUILayout.Space(5);
            EditorGUILayout.LabelField("Animation Setting", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_AnimationType, new GUIContent("AnimationType"));
            EditorGUILayout.PropertyField(m_AutoPlay, new GUIContent("AutoPlay"));
            EditorGUILayout.PropertyField(m_AutoKill, new GUIContent("AutoKill"));
            EditorGUILayout.PropertyField(m_EnableEditorInRuntime, new GUIContent("EditorInRuntime"));

            // EditorGUILayout.EndHorizontal();


        }

        private void OnActionGeneralValueInspectorGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("General Setting", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(m_UseSpeed, new GUIContent("UseSpeed"));
            if (m_UseSpeed.boolValue)
            {
                EditorGUILayout.PropertyField(m_Speed, new GUIContent("Speed"));
            }
            else
            {
                EditorGUILayout.PropertyField(m_Duration, new GUIContent("Duration"));
            }

            EditorGUILayout.PropertyField(m_ActionType, new GUIContent("ActionType"));
            EditorGUILayout.PropertyField(m_ActionDirection, new GUIContent("ActionDirection"));
            EditorGUILayout.PropertyField(m_LoopTime, new GUIContent("LoopTime"));
            EditorGUILayout.PropertyField(m_Relative, new GUIContent("Relative"));
            EditorGUILayout.PropertyField(m_EaseEnum, new GUIContent("EaseEnum"));
            if(m_EaseEnum.intValue == 31)
            {
                EditorGUILayout.PropertyField(m_ActionCurve, new GUIContent("ActionCurve"));
            }
            EditorGUILayout.PropertyField(m_ScaleTime, new GUIContent("ScaleTime"));
        }

        private void OnActionGeneralEventInspectorGUI()
        {
            GUILayout.Space(10);
            m_IsOpenActionEventSetting = EditorGUILayout.Foldout(m_IsOpenActionEventSetting, "Event Setting",true, EditorStyles.foldout);
            if (m_IsOpenActionEventSetting)
            {
                GetTarget().m_EnableBeginAction = EditorGUILayout.ToggleLeft("OnBeginPlayEvent", GetTarget().m_EnableBeginAction);
                if (GetTarget().m_EnableBeginAction)
                {
                    EditorGUILayout.PropertyField(OnBeginPlayEvent, new GUIContent("OnBeginPlay"));
                }

                GetTarget().m_EnableCompleteAction = EditorGUILayout.ToggleLeft("OnCompleteEvent", GetTarget().m_EnableCompleteAction);
                if (GetTarget().m_EnableCompleteAction)
                {
                    EditorGUILayout.PropertyField(OnCompleteEvent, new GUIContent("OnComplete"));
                }

                GetTarget().m_EnableKillAction = EditorGUILayout.ToggleLeft("OnKillEvent", GetTarget().m_EnableKillAction);
                if (GetTarget().m_EnableKillAction)
                {
                    EditorGUILayout.PropertyField(OnKillEvent, new GUIContent("OnKill"));
                }

                GetTarget().m_EnableLoopCompleteAction = EditorGUILayout.ToggleLeft("LoopCompleteEvent", GetTarget().m_EnableLoopCompleteAction);
                if (GetTarget().m_EnableLoopCompleteAction)
                {
                    EditorGUILayout.PropertyField(OnLoopCompleteEvent, new GUIContent("LoopComplete"));
                }

                GetTarget().m_EnableStepUpdateAction = EditorGUILayout.ToggleLeft("StepUpdateEvent", GetTarget().m_EnableStepUpdateAction);
                if (GetTarget().m_EnableStepUpdateAction)
                {
                    EditorGUILayout.PropertyField(OnStepUpdateEvent, new GUIContent("StepUpdate"));
                }

            }

        }

        private void OnActionControllerInspectorGUI()
        {

            GUILayout.Space(10);
            EditorGUILayout.LabelField(GetTarget().m_AnimationType +" Action Controller", EditorStyles.boldLabel);

            if (GetTarget().IsKill())
            {
                EditorGUILayout.LabelField("Action had Killed" );
            }

            EditorGUI.BeginDisabledGroup(GetTarget().IsKill()); // 禁止可用

            #region Play Revert Pause Resume 

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Play"))
            {
                GetTarget().Play();
            }

            if (GUILayout.Button("Revert"))
            {
                GetTarget().Revert();
            }

            if (GUILayout.Button("Pause"))
            {
                GetTarget().Pause();
            }

            if (GUILayout.Button("Resume"))
            {
                GetTarget().Resume();
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region RePlay Reset Kill ReInit

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("RePlay"))
            {
                GetTarget().RePlay();
            }

            if (GUILayout.Button("Reset"))
            {
                GetTarget().Reset();
            }

            if (GUILayout.Button("Kill"))
            {
                GetTarget().Kill();
            }


            if (GetTarget().m_EnableEditorInRuntime && GUILayout.Button("ReInit"))
            {
                GetTarget().ReInit();
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// 根据动画类型显示GUI
        /// </summary>
        private void OnActionTypeInspectorGUI()
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField(GetTarget().m_AnimationType + " Action Setting", EditorStyles.boldLabel);

            switch (GetTarget().m_AnimationType)
            {
                case ActionTweenType.None:
                    break;
                case ActionTweenType.Move:
                    EditorGUILayout.PropertyField(m_MoveToType, new GUIContent("MoveToType"));
                    switch (GetTarget().m_MoveToType)
                    {
                        case MoveAction.MoveToType.Pos:
                            EditorGUILayout.PropertyField(m_MoveTargetPos, new GUIContent("MoveTargetPos"));
                            break;
                        case MoveAction.MoveToType.Target:
                            EditorGUILayout.PropertyField(m_MoveTargetTransform, new GUIContent("MoveTargetTran"));
                            break;
                    }
                    break;
                case ActionTweenType.Rotation:
                    EditorGUILayout.PropertyField(m_RotToType, new GUIContent("RotToType"));
                    switch (GetTarget().m_RotToType)
                    {
                        case RotationAction.RotToType.Value:
                            EditorGUILayout.PropertyField(m_RotToVec, new GUIContent("RotToVec"));
                            break;
                        case RotationAction.RotToType.Target:
                            EditorGUILayout.PropertyField(m_RotToTarget, new GUIContent("RotToTarget"));
                            break;
                        default:
                            break;
                    }
                    EditorGUILayout.PropertyField(m_RotModel, new GUIContent("RotModel"));
                    break;
                case ActionTweenType.Scale:
                    EditorGUILayout.PropertyField(m_ScaleTarget, new GUIContent("ScaleTarget"));
                    break;
                case ActionTweenType.Color:
                    EditorGUILayout.PropertyField(m_ColorType, new GUIContent("TargetColorType"));
                    EditorGUILayout.PropertyField(m_TarCol, new GUIContent("TargetColorVaule"));
                    break;
                case ActionTweenType.Fade:
                    EditorGUILayout.PropertyField(m_FadeType, new GUIContent("TargetFadeType"));
                    EditorGUILayout.PropertyField(m_FadeTarAlpha, new GUIContent("TargetFadeAlpha"));
                    break;
                default:
                    break;
            }
        }

        private void ApplyModify()
        {
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
            // 保存序列化数据，否则会出现设置数据丢失情况
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

    }
}