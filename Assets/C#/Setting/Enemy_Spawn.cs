using System.Collections.Generic;
using UnityEngine;

public class Spawn_Build : MonoBehaviour
{
    [SerializeField] private SpriteRenderer BackgroundRenderer;
    Bounds MapBounds;
    public MiniMap MapUI;

    public GameObject Player;
    public GameObject Sub_Base;
    public GameObject Main_Base;
    public GameObject Enemy;
    public GameObject BulletPre;
    public GameObject Sub_TurretPre;
    public GameObject Main_TurretPre_1;
    public GameObject Main_TurretPre_2;

    public int Sub_Base_Count   = 3;
    public int Main_Base_Count  = 1;
    public int Sub_Base_Enemy_Count   = 2;
    public int Main_Base_Enemy_Count  = 3;
    public static int Base_Count = 0;

    // Distance
    private Vector3 PlayerPOS;
    public float Enemy_Distance     = 15.0f;
    public float SubBase_Distance   = 10.0f;
    public float MainBase_Distance  = 20.0f;
    public float SB_Enemy_Distance  = 5.0f;
    public float MB_Enemy_Distance  = 7.5f;

    // Enemy Offset
    public float Spawn_Sub_Offset   = 7.5f;
    public float Spawn_Main_Offset  = 10.0f;

    private List<Vector3> UsedPositions     = new List<Vector3>();
    private List<Vector3> SubBasePositions  = new List<Vector3>();
    private List<Vector3> MainBasePositions = new List<Vector3>();

    private Vector3 MapMin, MapMax;
    private Vector3 SB_Min, SB_Max;
    private Vector3 MB_Min, MB_Max;
    private Vector3 Enemy_Min, Enemy_Max;

    float SB_ExtentX, SB_ExtentY;
    float MB_ExtentX, MB_ExtentY;
    float Enemy_ExtentX, Enemy_ExtentY;

    public AudioClip EnemyDeath;
    public AudioClip SBaseDeath;
    public AudioClip MBaseDeath;

    void Start()
    {
        UsedPositions.Clear();
        PlayerPOS = Player.transform.position;

        Vector3 SB_Position = Vector3.zero;
        Vector3 MB_Position = Vector3.zero;
        Vector3 E_Position  = Vector3.zero;

        Vector2 Offset;
        float[] Angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
        float Random_Angle;
        Quaternion Random_Rotation;

        bool Check;

        if (BackgroundRenderer != null)
        {
            MapBounds = BackgroundRenderer.bounds;
        }
        else
        {
            Debug.LogError("Can`t Find BackgroundRenderer.");
        }


        MapMin = MapBounds.min;
        MapMax = MapBounds.max;

        var SBase_Bounds = Sub_Base.GetComponent<SpriteRenderer>().bounds;
        SB_ExtentX = SBase_Bounds.extents.x;
        SB_ExtentY = SBase_Bounds.extents.y;

        var MBase_Bounds = Main_Base.GetComponent<SpriteRenderer>().bounds;
        MB_ExtentX = MBase_Bounds.extents.x;
        MB_ExtentY = MBase_Bounds.extents.y;

        var Enemy_Bounds = Enemy.GetComponent<SpriteRenderer>().bounds;
        Enemy_ExtentX = Enemy_Bounds.extents.x;
        Enemy_ExtentY = Enemy_Bounds.extents.y;

        SB_Min = MapMin + new Vector3(SB_ExtentX, SB_ExtentY, 0);
        SB_Max = MapMax - new Vector3(SB_ExtentX, SB_ExtentY, 0);

        MB_Min = MapMin + new Vector3(MB_ExtentX, MB_ExtentY, 0);
        MB_Max = MapMax - new Vector3(MB_ExtentX, MB_ExtentY, 0);

        Enemy_Min = MapMin + new Vector3(Enemy_ExtentX, Enemy_ExtentY, 0);
        Enemy_Max = MapMax - new Vector3(Enemy_ExtentX, Enemy_ExtentY, 0);

        // Make Sub Base
        for (int i = 0; i < Sub_Base_Count; i++)
        {
            do
            {
                SB_Position = new Vector3(
                    Random.Range(SB_Min.x, SB_Max.x),
                    Random.Range(SB_Min.y, SB_Max.y),
                    0f);

                Check = true;
                foreach (var Used in UsedPositions)
                {
                    if (Vector3.Distance(Used, SB_Position) < SubBase_Distance)
                    {
                        Check = false;
                        break;
                    }
                }

                if (Vector3.Distance(PlayerPOS, SB_Position) < Enemy_Distance)
                {
                    Check = false;
                }

            } while (!Check);

            GameObject SB_Base_Obj = Instantiate(Sub_Base, SB_Position, Quaternion.identity);
            SB_Base_Obj.tag = "Sub_Base";

            AudioSource audio = SB_Base_Obj.AddComponent<AudioSource>();
            audio.clip = SBaseDeath;
            audio.playOnAwake = false;

            if (SB_Base_Obj.GetComponent<Sub_Base_HP>() == null)
                SB_Base_Obj.AddComponent<Sub_Base_HP>();

            if (Sub_TurretPre != null)
            {
                GameObject Turret = Instantiate(Sub_TurretPre, SB_Base_Obj.transform);
                Turret.transform.localPosition = Vector3.zero;

                Turret.transform.localRotation = Quaternion.identity;

                var Aiming = Turret.GetComponent<TurretAiming>();
                if (Aiming != null)
                    Aiming.Player = Player.transform;

                var Shooting = Turret.GetComponent<TurretShooting>();
                if (Shooting != null)
                    Shooting.Player = Player.transform;
            }
            var HP = SB_Base_Obj.AddComponent<Sub_Base_HP>();

            SubBasePositions.Add(SB_Position);
            UsedPositions.Add(SB_Position);
            Base_Count++;
        }

        // Make Main Base
        for (int i = 0; i < Main_Base_Count; i++)
        {
            do
            { 
                MB_Position = new Vector3(
                Random.Range(MB_Min.x, MB_Max.x),
                Random.Range(MB_Min.y, MB_Max.y),
                0f);
                Random_Angle = Angles[Random.Range(0, Angles.Length)];
                Random_Rotation = Quaternion.Euler(0, 0, Random_Angle);

                Check = true;
                foreach (var Used in UsedPositions)
                {
                    if (Vector3.Distance(Used, MB_Position) < MainBase_Distance)
                    {
                        Check = false;
                        break;
                    }
                }

                if (Vector3.Distance(PlayerPOS, MB_Position) < MainBase_Distance)
                {
                    Check = false;
                }

            } while (!Check);

            GameObject MB_Base_Obj = Instantiate(Main_Base, MB_Position, Random_Rotation);
            MB_Base_Obj.transform.localScale = new Vector3(5.0f, 5.0f, 1.0f);
            MB_Base_Obj.tag = "Main_Base";

            AudioSource audio = MB_Base_Obj.AddComponent<AudioSource>();
            audio.clip = MBaseDeath;
            audio.playOnAwake = false;

            if (MB_Base_Obj.GetComponent<Main_Base_HP>() == null)
                MB_Base_Obj.AddComponent<Main_Base_HP>();

            Vector3[] turret1Offsets = new Vector3[]
            {
                new Vector3(-0.265f, 0.55f, 0f),
                new Vector3(0.265f, 0.55f, 0f)
            };
            Vector3 turret2Offset = new Vector3(0f, -0.64f, 0f);

            if (Main_TurretPre_1 != null)
            {
                foreach (Vector3 offset in turret1Offsets)
                {
                    GameObject Turret = Instantiate(Main_TurretPre_1, MB_Base_Obj.transform);
                    Turret.transform.localPosition = offset;
                    Turret.transform.localRotation = Quaternion.identity;

                    var Aiming = Turret.GetComponent<TurretAiming>();
                    if (Aiming != null)
                        Aiming.Player = Player.transform;

                    var Shooting = Turret.GetComponent<TurretShooting>();
                    if (Shooting != null)
                        Shooting.Player = Player.transform;
                }
            }

            if (Main_TurretPre_2 != null)
            {
                GameObject Turret = Instantiate(Main_TurretPre_2, MB_Base_Obj.transform);
                Turret.transform.localPosition = turret2Offset;
                Turret.transform.localRotation = Quaternion.identity;

                var Aiming = Turret.GetComponent<TurretAiming>();
                if (Aiming != null)
                    Aiming.Player = Player.transform;

                var Shooting = Turret.GetComponent<TurretShooting>();
                if (Shooting != null)
                    Shooting.Player = Player.transform;
            }
            var HP = MB_Base_Obj.AddComponent<Main_Base_HP>();

            MainBasePositions.Add(MB_Position);
            UsedPositions.Add(MB_Position);
            Base_Count++;
        }

        // Make Enemy In Sub Base
        foreach (var SubBasePos in SubBasePositions)
        {
            for (int i = 0; i < Sub_Base_Enemy_Count; i++)
            {
                int TryCount = 0;
                bool IsUsedPosition = false;

                do
                {
                    TryCount++;
                    if (TryCount > 10) break;

                    Offset = Random.insideUnitCircle.normalized * Spawn_Sub_Offset;
                    E_Position = SubBasePos + new Vector3(Offset.x, Offset.y, 0);

                    if (E_Position.x >= Enemy_Min.x && E_Position.x <= Enemy_Max.x &&
                        E_Position.y >= Enemy_Min.y && E_Position.y <= Enemy_Max.y)
                    {
                        bool NoConflict = true;

                        foreach (var pos in UsedPositions)
                        {
                            if (Vector3.Distance(pos, E_Position) < SB_Enemy_Distance)
                            {
                                NoConflict = false;
                                break;
                            }
                        }

                        if (NoConflict)
                        {
                            IsUsedPosition = true;
                        }
                    }

                } while (!IsUsedPosition);

                if (IsUsedPosition)
                {
                    Spawn_Base_Enemy(E_Position);
                }
                else
                {
                    Debug.LogWarning($"Sub Spawn Failed at base {SubBasePos} after {TryCount} tries.");
                }
            }
        }

        // Make Enemy In Main Base
        foreach (var MainBasePos in MainBasePositions)
        {
            for (int i = 0; i < Main_Base_Enemy_Count; i++)
            {
                int TryCount = 0;
                bool IsUsedPosition = false;

                do
                {
                    TryCount++;
                    if (TryCount > 10) break;

                    Offset = Random.insideUnitCircle.normalized * Spawn_Main_Offset;
                    E_Position = MainBasePos + new Vector3(Offset.x, Offset.y, 0);

                    if (E_Position.x >= Enemy_Min.x && E_Position.x <= Enemy_Max.x &&
                        E_Position.y >= Enemy_Min.y && E_Position.y <= Enemy_Max.y)
                    {
                        bool NoConflict = true;

                        foreach (var pos in UsedPositions)
                        {
                            if (Vector3.Distance(pos, E_Position) < MB_Enemy_Distance)
                            {
                                NoConflict = false;
                                break;
                            }
                        }

                        if (NoConflict)
                        {
                            IsUsedPosition = true;
                        }
                    }

                } while (!IsUsedPosition);

                if (IsUsedPosition)
                {
                    Spawn_Base_Enemy(E_Position);
                }
                else
                {
                    Debug.LogWarning($"Sub Spawn Failed at base {MainBasePos} after {TryCount} tries.");
                }
            }
        }
        if (MapUI != null)
        {
            MapUI.SubBasePositions = SubBasePositions;
            MapUI.MainBasePositions = MainBasePositions;
        }
    }

    void Spawn_Base_Enemy(Vector3 POS)
    {
        GameObject EnemyObj = new GameObject("Enemy");
        EnemyObj.tag = "Enemy";
        EnemyObj.transform.position = POS;

        AudioSource audio = EnemyObj.AddComponent<AudioSource>();
        audio.clip = EnemyDeath;
        audio.playOnAwake = false;

        Vector2 Dir = (Player.transform.position - POS).normalized;
        float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        EnemyObj.transform.rotation = Quaternion.Euler(0, 0, angle);

        var Control = EnemyObj.AddComponent<EnemyControl>();
        Control.Player = Player.transform;
        Control.EnemySprite = Enemy.GetComponent<SpriteRenderer>().sprite;

        var Aim = EnemyObj.AddComponent<Enemy_Aim>();
        Aim.Player = Player.transform;
        Aim.BulletPre = BulletPre;

        EnemyObj.AddComponent<EnemyHealth>();

        UsedPositions.Add(POS);
    }
}