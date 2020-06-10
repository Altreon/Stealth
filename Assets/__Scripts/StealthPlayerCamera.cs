using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthPlayerCamera : MonoBehaviour {
    static private StealthPlayerCamera _S;

    public enum eCamMode { far, nearL, nearR };

    [Header("Inscribed")]
    public ThirdPersonWallCover  playerInstance;

    [Range(0,1)]
    public float            cameraEasing = 0.25f;
    public Vector3          relativePosFar = Vector3.zero;
    public float            xRotationFar = 60;


    [HideInInspector]
	public eCamMode         camMode = eCamMode.far;

	[Header("Zoom")]
	public Vector3          relativePosNear = Vector3.zero;
	public float            horizontalRotationFar = 0;
	public float			horizontalOffset = 0;


	private void Awake()
	{
        S = this;
        if (relativePosFar == Vector3.zero) {
            relativePosFar = transform.position - playerInstance.transform.position;
        }
	}

	
	void Update () {
        ThirdPersonWallCover.CoverInfo coverInfo = playerInstance.GetCoverInfo();
        if (coverInfo.inCover == -1) {
            camMode = eCamMode.far;
        } else {
			if (!coverInfo.zoomL) {
				camMode = eCamMode.nearL;
			} else if (!coverInfo.zoomR) {
				camMode = eCamMode.nearR;
			} else {
				camMode = eCamMode.far;
			}
        }

        Vector3 pDesired = Vector3.zero; 
        Quaternion rotDesired = Quaternion.identity;
        switch (camMode) {
            case eCamMode.far:
                pDesired = playerInstance.transform.position + relativePosFar;
                rotDesired = Quaternion.Euler(xRotationFar, 0, 0);
                break;
		case eCamMode.nearL:
			pDesired = playerInstance.transform.position;

			switch (coverInfo.inCover) {
			case 0:
				pDesired += relativePosNear + horizontalOffset * -Vector3.right;
				rotDesired = Quaternion.Euler(horizontalRotationFar, 0, 0);
				break;
			case 1:
				pDesired += Quaternion.AngleAxis(90, Vector3.up) * relativePosNear + horizontalOffset * Vector3.forward;
				rotDesired = Quaternion.Euler (horizontalRotationFar, 90, 0);
				break;
			case 2:
				pDesired += Quaternion.AngleAxis(180, Vector3.up) * relativePosNear + horizontalOffset * Vector3.right;
				rotDesired = Quaternion.Euler (180 - horizontalRotationFar, 0, 180);
				break;
			case 3:
				pDesired += Quaternion.AngleAxis(-90, Vector3.up) * relativePosNear + horizontalOffset * -Vector3.forward;
				rotDesired = Quaternion.Euler (180 - horizontalRotationFar, 90, 180);
				break;
			}
			break;
		case eCamMode.nearR:
			pDesired = playerInstance.transform.position;

			switch (coverInfo.inCover) {
			case 0:
				pDesired += relativePosNear + horizontalOffset * Vector3.right;
				rotDesired = Quaternion.Euler(horizontalRotationFar, 0, 0);
				break;
			case 1:
				pDesired += Quaternion.AngleAxis(90, Vector3.up) * relativePosNear + horizontalOffset * -Vector3.forward;
				rotDesired = Quaternion.Euler (horizontalRotationFar, 90, 0);
				break;
			case 2:
				pDesired += Quaternion.AngleAxis(180, Vector3.up) * relativePosNear + horizontalOffset * -Vector3.right;
				rotDesired = Quaternion.Euler (180 - horizontalRotationFar, 0, 180);
				break;
			case 3:
				pDesired += Quaternion.AngleAxis(-90, Vector3.up) * relativePosNear + horizontalOffset * Vector3.forward;
				rotDesired = Quaternion.Euler (180 - horizontalRotationFar, 90, 180);
				break;
			}
			break;
        }

		Vector3 pInterp = (1-cameraEasing)*transform.position + cameraEasing*pDesired;
        transform.position = pInterp;

        Quaternion rotInterp = Quaternion.Slerp(transform.rotation, rotDesired, cameraEasing);
        transform.rotation = rotInterp;
	}


    public void JumpToFarPosition() {
        transform.position = playerInstance.transform.position + relativePosFar;
        transform.rotation = Quaternion.Euler(xRotationFar, 0, 0);
    }

    static private StealthPlayerCamera S
    {
        get { return _S; }
        set
        {
            _S = value;
        }
    }

    static public eCamMode MODE {
        get {
            if (_S == null)
            {
                return eCamMode.far;
            }
            return _S.camMode;
        }
    }

    static public void ResetToFarPosition() {
        S.JumpToFarPosition();
    }

}
