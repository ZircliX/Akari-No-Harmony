using UnityEngine;
using UnityEngine.VFX;

namespace GameUI
{
    [System.Serializable]
    public struct VFXAsset
    {
        public string name;
        public VisualEffect vfx;
    }

    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance;

        public VFXAsset[] vfxAssets;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayVFX(string name)
        {
            foreach (var asset in vfxAssets)
            {
                if (asset.name == name)
                {
                    asset.vfx.Play();
                }
            }
        }
    }
}