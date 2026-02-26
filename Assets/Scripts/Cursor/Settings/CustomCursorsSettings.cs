using DG.Tweening;
using UnityEngine;

namespace InteractiveKitchen.Cursors
{ 
    [CreateAssetMenu(menuName = "ClickKitchen/Settings/Custom Cursors")]
    public class CustomCursorsSettings : ScriptableObject
    {
        //[Header("Global Settings")]
        [Header("Fork Cursor")]
        public float forkAnimPixelsLength;
        public float forkAnimTime;
        public Ease forkAnimEase;

        [Header("Knife Cursor")]
        public float knifeSlashRotationDegrees;
        public float knifeSlashInDuration;
        public Ease knifeSlashInEase;
        public float knifeSlashOutDuration;
        public Ease knifeSlashOutEase;
    }
}
