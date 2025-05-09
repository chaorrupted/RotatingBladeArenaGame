using UnityEngine;
using DG.Tweening;
using ScratchCardAsset;
using Utilities;

namespace Weapons
{
    public class MeleeWeapon: MonoBehaviour
    {
        [SerializeField] public BoxCollider2D selfCollider;
        
        public int ParentWeaponBeltID { get; set; } = 0;
        public bool Clashed { get; set; } = false;
        
        /* kinda buggy :( todo: sword scratching
        private int _scratcherID = 4; 

        private ScratchCardManager _cardManager;
        private ScratchIDGenerator _idGenerator;

        
        private void Awake()
        {
            var managerGameObject = GameObject.FindGameObjectWithTag("ScratchManager");
            _cardManager = managerGameObject.GetComponent<ScratchCardManager>();
            _idGenerator = managerGameObject.GetComponent<ScratchIDGenerator>();
        }
        
        private void OnEnable()
        {
            Vector2 position = _cardManager.MainCamera.WorldToScreenPoint(transform.position);
            _scratcherID = _idGenerator.GetID();
            _cardManager.Card.Input.Register(_scratcherID, position);
        }
        
        private void LateUpdate()
        {
            if (Clashed)
            {
                return;
            }
            Vector2 position = _cardManager.MainCamera.WorldToScreenPoint(transform.position);
            _cardManager.Card.Input.Scratch(_scratcherID, position);
        }*/
        
        public void RotateSelfToAngle(float angle)
        {
            transform.DOLocalRotate(new Vector3(0, 0, angle), 0.7f, RotateMode.Fast) // todo: test, consider over360?
                .SetRelative(false)
                .SetEase(Ease.OutCubic); // todo: adjust duration, ease
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Clashed)
            {
                return;
            }
            
            if (!other.gameObject.activeInHierarchy)
            {
                return;
            }
            
            if (other.gameObject.CompareTag("RedSword"))
            {
                var otherParentID = other.GetComponent<RedSword>().ParentWeaponBeltID;
                if (otherParentID == 0) { return; }
                if (otherParentID == ParentWeaponBeltID) { return; }

                selfCollider.enabled = false;
                Clashed = true;
            }
        }
    }
}