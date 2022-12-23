using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TicTacToe
{
    public class TileView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [SerializeField] MeshRenderer meshRenderer;
        
        [Header("Player Settings")]
        [SerializeField] float[] playerSelectRotationDeg;
        
        [Header("Interactions")]
        [SerializeField] float spinSpeed = 0.3f;
        [SerializeField] float mouseEnterScaleAmount = 1.1f;
        [SerializeField] float mouseEnterScaleSpeed = 0.3f;
        [SerializeField] float mouseExitScaleSpeed = 0.3f;
        
        public event Action OnMouseEnter = () => { };
        public event Action OnMouseExit = () => { };
        public event Action OnMouseDown = () => { };
        public event Action OnMouseUp = () => { };
        public event Action OnMouseClick = () => { };
        
        // tile begins with no owner
        public PlayerType OwnerPlayer = PlayerType.None;
        
        PlayerType turnPlayer;
        PlayerConfig turnOwnerConfig;
        Tween tween;
        Color defaultColor;
        float defaultScale;
        bool isAllowInteraction;
        
        
        void Awake()
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
        
        #region Interaction Events
        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            OnMouseEnter();
            meshRenderer.material.color = turnOwnerConfig.TeamColor;
            tween = transform.DOScale(defaultScale * mouseEnterScaleAmount, mouseEnterScaleSpeed).SetEase(Ease.OutBounce);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            OnMouseExit();
            meshRenderer.material.color = defaultColor;
            tween = transform.DOScale(defaultScale, mouseExitScaleSpeed).SetEase(Ease.OutBounce);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            OnMouseDown();
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            OnMouseUp();
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (!isAllowInteraction)
                return;
            
            OnMouseClick();

            isAllowInteraction = false;
            OwnerPlayer = turnPlayer;
            
            tween = transform.DOLocalRotate(new Vector3(0, playerSelectRotationDeg[(int)turnPlayer], 0f), spinSpeed).SetEase(Ease.OutBounce);
        }
        #endregion
    }
}