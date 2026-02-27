using System.ComponentModel;
using DG.Tweening;
using InteractiveKitchen.Cursors;
using InteractiveKitchen.InteractionSystem;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;


/// <summary>
/// Class for 3D objects that you wish to drag with your mouse. The object will be dragged on a plane, not based on a distance from the camera 
/// Remember to use an interactionDetector which supports mouse drag
/// </summary>
public class PlaneDraggableObject : MonoBehaviour
{
    [Inject] Camera mainCam;
    [Inject] InteractableObjectsSettings settings;
    IContinousInteractionDetector<PointerEventData, CustomCursor> dragDetector;
    Plane dragPlane;    //Plane on which the object will be dragged
    Vector3 positionBeforeDrag;

    Tween flyBackTween;

    void Awake()
    {
        dragPlane = new Plane(settings.PlaneNormal.normalized, settings.PlanePoint);
        dragDetector = GetComponent<IContinousInteractionDetector<PointerEventData, CustomCursor>>();
    }

    void OnEnable()
    {
        dragDetector.OnInteractionStart.AddListener(HandelDragStart);
        dragDetector.OnInteractionTick.AddListener(HandelDragTick);
        dragDetector.OnInteractionEnd.AddListener(HandelDragEnd);
    }
    void OnDisable()
    {
        dragDetector.OnInteractionStart.RemoveListener(HandelDragStart);
        dragDetector.OnInteractionTick.RemoveListener(HandelDragTick);
        dragDetector.OnInteractionEnd.RemoveListener(HandelDragEnd);
    }
    void HandelDragStart(PointerEventData eventData, CustomCursor cursos)
    {
        if(flyBackTween != null)
        {
            flyBackTween.Kill(true);
        }
        else
        {
            positionBeforeDrag = transform.position;
        }
    }
    void HandelDragTick(PointerEventData eventData, CustomCursor cursos)
    {
        transform.position = GetPlanePointByMouse(eventData.position, mainCam, dragPlane);
    }
    void HandelDragEnd(PointerEventData eventData, CustomCursor cursos)
    {
        flyBackTween = transform.DOMove(positionBeforeDrag, settings.DraggableObjFlyBackTime)   
                                .SetEase(settings.DraggableObjFlyBackEase);
    }
    Vector3 GetPlanePointByMouse(Vector2 cursorScreenPosition, Camera camera, Plane plane)
    {
        Ray ray = camera.ScreenPointToRay(cursorScreenPosition);
        plane.Raycast(ray, out float distanceFromCamToRayPlaneIntersect);
        Vector3 rayPlaneIntersectPoint = ray.GetPoint(distanceFromCamToRayPlaneIntersect);
        return rayPlaneIntersectPoint;
    }


}
