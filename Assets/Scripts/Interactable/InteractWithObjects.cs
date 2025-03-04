using System.Collections.Generic;
using UnityEngine;

public class InteractWithObjects : MonoBehaviour
{
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private float _interactDistance;
    [SerializeField] private LayerMask _interactableLayerMask;
    [SerializeField] private RaycastHit _interactableObjectHitted;
    [SerializeField] private Material _outlineMaterial;
    MeshRenderer _interactableObjectMeshRenderer;
    List<Material> oldMaterials;

    private void Start()
    {
        _inputManager.GetInput.OnInteractInput += CheckForInteraction;
    }

    private void OnDisable()
    {
        _inputManager.GetInput.OnInteractInput -= CheckForInteraction;
    }

    private void Update()
    {
        if (Physics.Raycast(_playerCamera.position, _playerCamera.transform.forward, out _interactableObjectHitted, _interactDistance, _interactableLayerMask))
        {
            //if(_interactableObjectMeshRenderer)
            //{
            //    return;
            //}

            //_interactableObjectMeshRenderer = _interactableObjectHitted.collider.GetComponent<MeshRenderer>();
            //oldMaterials = _interactableObjectMeshRenderer.materials.ToList();
            //System.Collections.Generic.List<Material> materials = oldMaterials;
            //materials.Add( _outlineMaterial );
            //_interactableObjectMeshRenderer.SetMaterials( materials );
        }
        //else
        //{
        //    if (_interactableObjectMeshRenderer)
        //    {
        //        _interactableObjectMeshRenderer.SetMaterials(oldMaterials);
        //        _interactableObjectMeshRenderer = null;
        //    }
        //}
    }

    private void CheckForInteraction()
    {
        if (!_interactableObjectHitted.collider)
        {
            return;
        }

        Debug.DrawRay(_playerCamera.position, _playerCamera.transform.forward * _interactDistance, Color.green, 10f);

        Interactable hittedInteractable = _interactableObjectHitted.collider.GetComponent<Interactable>();

        if (hittedInteractable)
        {
            hittedInteractable.Interact();
        }

    }
}
