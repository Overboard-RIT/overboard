using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Windows.Kinect;
using Kinect = Windows.Kinect;

public class PlayerPositionDetection : MonoBehaviour
{
    public BodySourceManager bodySourceManager;
    public GameObject leftFoot;
    public GameObject rightFoot;
    private List<PlayerPosition> activePlayerPositions;
    private PlayerPosition playerPosition;
    public GameObject boundsManager;
    public bool fakeData = true;

    void Start()
    {
        playerPosition = new PlayerPosition(
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 0, 0)
        );
    }

    private PlayerPosition GetFakeVector()
    {
        Vector3 center = new Vector3(
                Mathf.Cos(Time.time) * 10,
                0,
                -30 + Mathf.Sin(Time.time) * 10
            );
        Vector3 leftFoot = new Vector3(
            center.x - 1,
            center.y,
            center.z
        );
        Vector3 rightFoot = new Vector3(
            center.x + 1,
            center.y,
            center.z
        );

        return new PlayerPosition(center, leftFoot, rightFoot);
    }

    // Update is called once per frame
    void Update()
    {
        activePlayerPositions = new List<PlayerPosition>();

        if (fakeData)
        {
            for (int i = 0; i < 6; i++)
            {
                activePlayerPositions.Add(GetFakeVector());
            }
            return;
        }
        List<PlayerPosition> positions = GetPlayerPositions();
        if (positions.Count == 0) return;

        BoundsManager bounds = boundsManager.GetComponent<BoundsManager>();

        Debug.Log(positions.Count);
        foreach (PlayerPosition position in positions)
        {
            if (position.center.x == 0 && position.center.y == 0 && position.center.z == 0) continue;
            if (!bounds.CheckInBounds(position.center)) continue;
            //Debug.Log($"Player Position: {position.center.x}, {position.center.y}, {position.center.z}");
            activePlayerPositions.Add(new PlayerPosition(position.center, position.leftFoot, position.rightFoot));
        }

    }

    public struct PlayerPosition
    {
        public Vector3 center;
        public Vector3 leftFoot;
        public Vector3 rightFoot;

        public PlayerPosition(Vector3 center, Vector3 leftFoot, Vector3 rightFoot)
        {
            this.center = center;
            this.leftFoot = leftFoot;
            this.rightFoot = rightFoot;

            this.leftFoot.y = 5;
            this.rightFoot.y = 5;
        }
    }

    public List<PlayerPosition> GetActivePositions()
    {
        return activePlayerPositions;
    }

    public List<PlayerPosition> GetPlayerPositions()
    {
        Body[] data = bodySourceManager.GetData();
        List<PlayerPosition> positions = new List<PlayerPosition>();
        foreach (var body in data)
        {
            Kinect.Joint footLeft = body.Joints[Kinect.JointType.FootLeft];
            Kinect.Joint ankleLeft = body.Joints[Kinect.JointType.FootLeft];
            Kinect.Joint footRight = body.Joints[Kinect.JointType.FootRight];
            Kinect.Joint ankleRight = body.Joints[Kinect.JointType.FootRight];
            Vector3 playerCenter = AverageJointPosition(footLeft, footRight);
            positions.Add(
                new PlayerPosition(
                    playerCenter,
                    (GetVector3FromJoint(footLeft) + GetVector3FromJoint(ankleLeft)) * 0.5f,
                    (GetVector3FromJoint(footRight) + GetVector3FromJoint(ankleRight)) * 0.5f
                )
            );
        }
        return positions;
    }

    private Vector3 AverageJointPosition(Kinect.Joint joint1, Kinect.Joint joint2)
    {
        Vector3 v1 = GetVector3FromJoint(joint1);
        Vector3 v2 = GetVector3FromJoint(joint2);

        return new Vector3(
            0.5f * (v1.x + v2.x),
            0.5f * (v1.y + v2.y),
            0.5f * (v1.z + v2.z)
        );
    }

    private Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, 5, joint.Position.Z * -10);
    }

    void OnDrawGizmos()
    {
        foreach (var position in GetPlayerPositions())
        {
            if (activePlayerPositions.Contains(position))
            {
                Gizmos.color = Color.yellow;
            }
            else
            {
                Gizmos.color = Color.red;
            }

            Gizmos.DrawSphere(position.center, 0.5f);
        }
    }

}