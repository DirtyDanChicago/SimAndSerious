
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
//Code we are using
//https://answers.unity.com/questions/1142547/2d-platformer-sidekick-helper-follower.html
public class FollowScriptt : MonoBehaviour
{
    [SerializeField]
    public float jumpForce = 10;
    [SerializeField]
    public Rigidbody2D myRigidBody;

    //For animation
    private Animator myAnimator;

    public LayerMask groundLayer;
    public bool isOnGround;
    public float jumpPosition;

    public Transform target = null;                    // follow by target
    public float reactDelay = 0.5f;                    // how fast it react to target moves [edit it in edit mode only]
    public float recordFPS = 60f;                    // how many record data in one second [edit it in edit mode only]
    public float followSpeed = 5f;                    // follow speed
    public float delayTime = 0f;
    public float targetDistance = 0f;                // don't move closer than this value
    public bool targetDistanceY = false;            // if near then update y only
    public bool targetPositionOnStart = false;        // set the same position as target on start
    public bool start = false;                        // start or stop follow    
    public List<float> storeJumpPosition;
#if UNITY_EDITOR
    public bool gizmo = true;                        // draw gizmos
#endif
    [Serializable]
    public class TargetRecord
    {
        public Vector3 position;    // world position

        public TargetRecord(Vector3 position)
        {
            this.position = position;
        }

    }

    private TargetRecord[] _records = null;            // keeps target data in time
    private float _t = 0f;
    private int _i = 0;                                // keeps current index for recorder
    private int _j = 1;                                // keeps current index for follower
    public int _L = 0;
    private float _interval = 0f;
    private TargetRecord _record = null;            // current
    private bool _recording = true;                    // stop recording if true
    private int _arraySize = 2;
    private Collision2D box;

    public void Awake()
    {
    }

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (targetPositionOnStart)
            transform.position = target.position;

        _interval = 1 / recordFPS;

        _arraySize = Mathf.CeilToInt(recordFPS * reactDelay);
        if (_arraySize == 0)
            _arraySize = 2;

        _records = new TargetRecord[_arraySize];

    }

    bool isGrounded()
    {
        //Debug.Log("Called");
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;
        Debug.DrawRay(position, direction, Color.green);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            //Debug.Log("Ground");
            return true;
        }
        //To check if it works
        //Debug.Log("Off");
        return false;
    }

    void OnCollisionEnter2D(Collision2D collisionCheck)
    {
        if (collisionCheck.gameObject.tag == "Player")
        {
            start = true;
        }
    }

    private void Update()
    {
    }

    // update this transform data
    public void LateUpdate()
    {
        if (start)

        {
            // can be move into the Update or LateUpdate if needed
            RecordData(Time.deltaTime);
            //Simply checks if follower is on the ground and if the jump button has been pressed.
            //Did Distance/speed = time to get how long to delay the jump so the follower jumps at same location of where the player had jumped
            if (Input.GetButtonDown("Jump") && isGrounded())
            {
                jumpPosition = Math.Abs(target.position.x - transform.position.x);
                transform.position = Vector3.MoveTowards(transform.position, new Vector3((transform.position.x + jumpPosition), target.position.y, transform.position.z), Time.deltaTime * followSpeed);
                delayTime = (Math.Abs(target.position.x - transform.position.x) / followSpeed);
                Debug.Log(delayTime);
                Invoke("HandleJumpInput", delayTime);
            }
            ///
            //This stops the followers from worrying about keeping distance from the player. This helps the followers jump the right distance so they can land on the platforms
            ///
            if (isGrounded() == false)
            {
                float tempDistance = targetDistance;
                targetDistance = 0;
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, transform.position.z), Time.deltaTime * followSpeed);
                targetDistance = tempDistance;
            }

            //if (isGrounded() && transform.position.y != target.position.y)
            //{
            //    transform.position = new Vector3(target.position.x - 2, target.position.y, target.position.z);
            //}

            ////move to the target
            if (targetDistance <= 0f)
            {
                if (_record != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(_record.position.x, transform.position.y, transform.position.z), Time.deltaTime * followSpeed);
                }

            }
            //This moves to the target
            if ((target.position - transform.position).magnitude > targetDistance)
            {
                if (!_recording)
                {
                    ResetRecordArray();
                    _recording = true;
                }

                if (_record != null)
                {
                    //make a line which slows down the follower when they are close to player so it doesn't look so stop and go and stop clunky.
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(_record.position.x, transform.position.y, transform.position.z), Time.deltaTime * followSpeed);
                }
            }

            else if (targetDistanceY && Mathf.Abs(target.position.y - transform.position.y) > 0.05f && isGrounded())
            {
                if (_record != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(_record.position.x, transform.position.y, transform.position.z), Time.deltaTime * followSpeed);
                }

            }

            else
            {
                _recording = false;
            }
        }

    }

    private void RecordData(float deltaTime)
    {
        if (!_recording)
            return;

        // check intervals
        if (_t < _interval)
        {
            _t += deltaTime;
        }
        // record this frame
        else
        {
            // record target data
            _records[_i] = new TargetRecord(target.position);

            // set next record index
            if (_i < _records.Length - 1)
                _i++;
            else
                _i = 0;

            // set next follow index
            if (_j < _records.Length - 1)
                _j++;
            else
                _j = 0;

            // handle current record
            _record = _records[_j];

            _t = 0f;
        }
    }

    // used if distance is small
    public void ResetRecordArray()
    {
        _i = 0;
        _j = 1;
        _t = 0f;

        _records = new TargetRecord[_arraySize];

        for (int i = 0; i < _records.Length; i++)
        {
            _records[i] = new TargetRecord(transform.position);
        }

        _record = _records[_j];
    }

    /// <summary>
    /// Gets the current record.
    /// </summary>
    public TargetRecord currentRecord
    {
        get
        {
            return _record;
        }
    }




    //Jump function.
    private void HandleJumpInput()
    {
        myRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    //If follower falls off platform, respawns them right behind player
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "DeathCollider")
        {
            Debug.Log("Follower entered death collider.");
            transform.position = new Vector3(target.position.x - 2, target.position.y, target.position.z);
        }
    }




#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (gizmo)
        {
            if (_records == null || _records.Length < 2)
                return;

            Gizmos.color = Color.red;
            for (int i = 0; i < _i - 1; i++)
            {
                if (_records[i] != null && _records[i + 1] != null)
                    Gizmos.DrawLine(_records[i].position, _records[i + 1].position);
            }

            //Gizmos.color = Color.green;
            for (int j = _j; j < _records.Length - 1; j++)
            {
                if (_records[j] != null && _records[j + 1] != null)
                    Gizmos.DrawLine(_records[j].position, _records[j + 1].position);
            }



            //Gizmos.color = Color.yellow;
            if (_records[0] != null && _records[_records.Length - 1] != null)
                Gizmos.DrawLine(_records[0].position, _records[_records.Length - 1].position);

            Gizmos.color = Color.white;
            if (_record != null)
                Gizmos.DrawLine(_record.position, transform.position);
        }
    }

#endif
}