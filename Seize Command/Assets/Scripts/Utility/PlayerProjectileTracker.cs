using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            MirrorProjectile mirrorScript = mirrorProj.GetComponent<MirrorProjectile>();
            mirrorScript.SetMirrorTarget(proj.transform);
        }
    }
}