using System.Collections.Generic;
using UnityEngine;

namespace GeneralLibrary
{
    [RequireComponent(typeof(Renderer))]
    public class MaterialSwitcher : MonoBehaviour
    {

        [SerializeField] private int targetMatIndex = 0;
        [SerializeReference] private List<Material> materials = new List<Material>();

        private Material defaultMaterial = null;
        private Renderer _renderer = null;

        private void Awake()
        {
            if (_renderer == null)
                _renderer = GetComponent<Renderer>();
            if (_renderer)
                defaultMaterial = _renderer.material;
        }

        public void SwitchMaterial()
        {
            SwitchMaterial(targetMatIndex);
        }

        public void SwitchMaterial(int targetMaterialIndex)
        {
            if (!isValidIndex(targetMaterialIndex))
                return;

            _renderer.material = materials[targetMaterialIndex];
        }

        public void SwitchToDefault()
        {
            _renderer.material = defaultMaterial;
        }

        private bool isValidIndex(int targetIndex)
        {
            if (targetIndex >= materials.Count)
            {
#if UNITY_EDITOR
                Debug.Log("<color=cyan>Material Switch Failed \n Target material out of bound of GameObject of " + name + "</color>");
#endif
                return false;
            }

            return true;
        }
    }
}