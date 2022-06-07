using UnityEngine;

public class MeditationLocomotionTeleport : LocomotionTeleport
{
    [SerializeField]
    private enum OrientationMode
    { 
        None,
        Thump,
        Gaze
    }
    [SerializeField] private OrientationMode orientationMode = OrientationMode.None;
    public override void DoTeleport()
    {
        Quaternion preRotation = LocomotionController.CharacterController.transform.rotation;
        base.DoTeleport();
        
        if(orientationMode == OrientationMode.None)
            LocomotionController.CharacterController.transform.rotation = preRotation;
    }

    public override void OnUpdateTeleportDestination(bool isValidDestination, Vector3? position, Quaternion? rotation, Quaternion? landingRotation)
    {
        switch (orientationMode)
        {
            case OrientationMode.None:
                rotation = Quaternion.identity;
                break;
            case OrientationMode.Thump:
                break;
            case OrientationMode.Gaze:
                rotation = LocomotionController.CharacterController.transform.rotation;
                break;
        }
        base.OnUpdateTeleportDestination(isValidDestination, position, rotation, landingRotation);


    }
}
