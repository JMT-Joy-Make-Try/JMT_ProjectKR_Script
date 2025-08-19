using System;
using System.Collections;
using DG.Tweening;
using JMT.Core.Tool;
using JMT.System.AgentSystem.PlayerSystem.Component;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JMT.UISystem.Start
{
    public class StartController : MonoBehaviour
    {
        [SerializeField] private StartView view;
        [SerializeField] private RectTransform titleImage;
        [SerializeField] private RectTransform startUI;
        [SerializeField] private PlayerAnimator player;
        [SerializeField] private FadeUI fadeUI;

        private void Awake()
        {
            view.OnStartEvent += HandleStartEvent;
            view.OnOptionEvent += HandleOptionEvent;
            view.OnExitEvent += HandleExitEvent;
        }

        private void Start()
        {
            player.ChangeState(PlayerAnimatorState.Idle);
            Debug.Log("StartController initialized.");
        }

        private void HandleStartEvent()
        {
            // �� �̵�. �ӽ÷� �� ������ �̵���
            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            titleImage.DOAnchorPosY(1000f, 1f).SetEase(Ease.InOutQuad);
            startUI.DOAnchorPosY(-1000f, 1f).SetEase(Ease.InOutQuad);
            yield return WaitForSecondsCache.Get(1f);
            // Fade
            player.ChangeState(PlayerAnimatorState.Run);
            player.transform.DOMoveX(40, 5f).SetEase(Ease.Linear);
            yield return WaitForSecondsCache.Get(3f);
            fadeUI.OpenPanel();
            yield return WaitForSecondsCache.Get(1f);
            SceneManager.LoadScene("JSY");
        }

        private void HandleOptionEvent()
        {
            // �ɼ�â ����
        }

        private void HandleExitEvent()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Debug.Log("게임이 종료되었습니다.");
            Application.Quit();
        }
    }
}
