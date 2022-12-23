using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TicTacToe
{
    public class TileView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] MeshRenderer meshRenderer;
        
        [Header("Player Settings")]
        [SerializeField] float[] playerSelectRotationDeg;
        
        [Header("Interactions")]
        [SerializeField] float spinSpeed = 0.3f;
        [SerializeField] float mouseEnterScaleAmount = 1.1f;
        [SerializeField] float mouseEnterScaleSpeed = 0.3f;
        [SerializeField] float mouseExitScaleSpeed = 0.3f;

        public event Action OnMouseClick = () => { };
        
        // tile begins with no owner
        public PlayerType OwnerPlayer = PlayerType.None;
        
        private PlayerType turnPlayer;
        private PlayerConfig turnOwnerConfig;
        private Color defaultColor;
        private float defaultScale;
        private bool isAllowInteraction;
        
        
        private void Awake()
        {
            // keep track of initial values so we can reset to correct values when needed
            defaultColor = meshRenderer.material.color;
            defaultScale = transform.localScale.x;
        }

        public void SetTurnOwner(PlayerConfig playerConfig, PlayerType playerType)
        {
            turnOwnerConfig = playerConfig;
            turnPlayer = playerType;
            isAllowInteraction = true;
        }
        
        public void SetOwner(PlayerConfig playerConfig, PlayerType playerType)
        {
            turnOwnerConfig = playerConfig;
            OwnerPlayer = playerType;
            isAllowInteraction = false;

            SetOwnerColor(playerConfig);
            transform.DOLocalRotate(new Vector3(0, playerSelectRotationDeg[(int)playerType], 0f), spinSpeed).SetEase(Ease.OutBounce);
        }

        private void SetOwnerColor(PlayerConfig playerConfig)
        {
            meshRenderer.material.color = playerConfig.TeamColor;
            transform.DOScale(defaultScale * mouseEnterScaleAmount, mouseEnterScaleSpeed).SetEase(Ease.OutBounce);
        }
        
        #region Interaction Events
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            SetOwnerColor(turnOwnerConfig);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            meshRenderer.material.color = defaultColor;
            transform.DOScale(defaultScale, mouseExitScaleSpeed).SetEase(Ease.OutBounce);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            OnMouseClick();
            SetOwner(turnOwnerConfig, turnPlayer);
        }
        #endregion
    }
}