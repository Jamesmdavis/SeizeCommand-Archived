using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.References;

namespace SeizeCommand.Utility
{
    public class PlayerProjectileTracker : MonoBehaviour
    {
        [SerializeField] private GameObject mirrorProjectile;
        [SerializeField] private Transform mirrorProjectileParent;

        public void CreateMirrorProjectile(GameObject proj)
        {
            Transform projTransform = proj.transform;
            GameObject mirrorProj = Instantiate(mirrorProjectile, projTransform.position,
                projTransform.rotation, mirrorProjectileParent);

            GameObjectReference mirrorProjRef = mirrorProj.GetComponent<GameObjectReference>();
            mirrorProjRef.Reference = proj;

            MirrorTransform mirrorScript = mirrorProj.GetComponent<MirrorTransform>();
            mirrorScript.StartMirroring();
        }
    }
}