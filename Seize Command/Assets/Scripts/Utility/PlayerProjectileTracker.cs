using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SeizeCommand.Referencing;

namespace SeizeCommand.Utility
{
    public class PlayerProjectileTracker : MonoBehaviour
    {
        [SerializeField] private GameObject mirrorProjectile;
        [SerializeField] private Transform mirrorProjectileParent;

        public void CreateMirrorProjectile(GameObject proj)
        {
            Vector2 projPositionData = proj.transform.position;
            Quaternion projRotationData = proj.transform.rotation;

            GameObject spawnedObject = Instantiate(mirrorProjectile, projPositionData,
                projRotationData, mirrorProjectileParent);

            References references = spawnedObject.GetComponent<References>();
            references.AddReference("Mirror Target", spawnedObject);

            MirrorTransform mirrorScript = spawnedObject.GetComponent<MirrorTransform>();
            mirrorScript.StartMirroring();
        }
    }
}