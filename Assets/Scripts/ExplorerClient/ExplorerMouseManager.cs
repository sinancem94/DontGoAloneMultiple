using UnityEngine;

namespace ExplorerClient
{
    public class ExplorerMouseManager 
    {    
        public enum CursorLayer
        {
            Default,
            Action,
            Interest,
            Door,
            Unreachable
        }
    
        //layers
        private readonly LayerMask _explorerLayer;
        private readonly LayerMask _movableSurfaceLayer;
        private readonly LayerMask _doorLayer;
        private readonly LayerMask _interestLayer;
        private readonly LayerMask _unreachableLayer;
    
        //cursor images
        private readonly Texture2D _defCursor;
        private readonly Texture2D _actionCursor;
        private readonly Texture2D _interestCursor;
        private readonly Texture2D _doorCursor;
        private readonly Texture2D _unreachableCursor;

        //current raycast data
        private Transform _rayHitTransform;
        private Vector3 _rayHitPoint;
        private LayerMask _rayHitLayerMask;

        private LayerMask _cursorLayer;
        private Texture2D _cursorTextureTexture;
        private CursorLayer _cursor;
        public Texture2D CurrentCursorTexture => _cursorTextureTexture;
        public CursorLayer CurrentCursor => _cursor;
        public Transform ClickedTransform => _rayHitTransform;
        public Vector3 ClickedWorldPosition => _rayHitPoint;
        public ExplorerMouseManager(GameEquipments equipments)
        {
            //set cursors
            _defCursor = equipments.GetCursor("defaultCursor".ToLower());
            _actionCursor = equipments.GetCursor("actionCursor".ToLower());
            _interestCursor = equipments.GetCursor("interestCursor".ToLower());
            _doorCursor = equipments.GetCursor("doorCursor".ToLower());
            _unreachableCursor = equipments.GetCursor("unreachableCursor".ToLower());
            
            //set layers
            _explorerLayer = LayerMask.NameToLayer("Explorer");
            _movableSurfaceLayer = LayerMask.NameToLayer("MovableSurface");
            _doorLayer = LayerMask.NameToLayer("Door");
            _interestLayer  = LayerMask.NameToLayer("Interest");
            _unreachableLayer  = LayerMask.NameToLayer("Unreachable");

            SetCursorLayers();
        }

        ~ExplorerMouseManager()
        {
            
        }
        
        #region public methods

        public void CastRay()
        {
            Ray ray = ExplorerCamera.instance.myCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            Debug.DrawRay(ray.origin, ray.direction * 30, Color.yellow,0.1f);
        
            if (Physics.Raycast(ray, out hit))
            {
                SetRayData(hit);
            }
            else
            {
                _rayHitLayerMask = _unreachableLayer;
            }
            
            ProcessRayData();
        }

        #endregion

        #region private methods

        void Clicked(Vector2 clickPos)
        {
            if (_cursorLayer != _unreachableLayer)
            {
                ExplorerGameManager.instance.explorer.MoveTo(_rayHitPoint);
            }
        }
        
        void SetCursorLayers()
        {
            _cursor = CursorLayer.Default;
            _cursorLayer = _movableSurfaceLayer;
        }

        void SetRayData(RaycastHit hit)
        {
            _rayHitTransform = hit.transform;
            _rayHitPoint = hit.point;
            _rayHitLayerMask = hit.transform.gameObject.layer;
        }

        void ProcessRayData()
        {
            if (_cursorLayer != _rayHitLayerMask)
            {
                _cursorLayer = _rayHitLayerMask;
                SetCursor();
            }
        }

        //switch case could be faster. make enum to cursorlayer
        void SetCursor()
        {
            if (_cursorLayer == _movableSurfaceLayer)
            {
                _cursor = CursorLayer.Default;
                _cursorTextureTexture = _defCursor;
            }
            else if (_cursorLayer == _explorerLayer)
            {
                _cursor = CursorLayer.Action;
                _cursorTextureTexture = _actionCursor;
            }
            else if (_cursorLayer == _interestLayer)
            {
                _cursor = CursorLayer.Interest;
                _cursorTextureTexture = _interestCursor;
            }
            else if (_cursorLayer == _doorLayer)
            {
                _cursor = CursorLayer.Door;
                _cursorTextureTexture = _doorCursor;
            }
            else if (_cursorLayer == _unreachableLayer)
            {
                _cursor = CursorLayer.Unreachable;
                _cursorTextureTexture = _unreachableCursor;
            }
        }

        #endregion


    }
}
