using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public RectTransform MapRect;
    public RectTransform Dot;
    public RectTransform playerDot;
    public RectTransform Radar;
    public float Speed = 60f;
    public Transform PlayerPOS;

    public List<Vector3> SubBasePositions = new List<Vector3>();
    public List<Vector3> MainBasePositions = new List<Vector3>();

    private List<RectTransform> enemyDots = new List<RectTransform>();

    public float mapRadius = 100f;
    public float maxDistance = 50f;

    void Update()
    {
        if (Radar != null)
            Radar.Rotate(0f, 0f, -Speed * Time.deltaTime);


        if (playerDot != null)
            playerDot.localPosition = Vector2.zero;

        List<Vector3> Bases = new List<Vector3>();
        Bases.AddRange(SubBasePositions);
        Bases.AddRange(MainBasePositions);

        while (enemyDots.Count < Bases.Count)
        {
            RectTransform Dots = Instantiate(Dot, MapRect);
            enemyDots.Add(Dots);
        }
        while (enemyDots.Count > Bases.Count)
        {
            Destroy(enemyDots[enemyDots.Count - 1].gameObject);
            enemyDots.RemoveAt(enemyDots.Count - 1);
        }

        for (int i = 0; i < Bases.Count; i++)
        {
            Vector3 BasePos = Bases[i];
            Vector3 Di = BasePos - PlayerPOS.position;
            Vector2 Dir2D = new Vector2(Di.x, Di.y);

            float distance = Dir2D.magnitude;
            Vector2 normalizedDir = Dir2D.normalized;

            Vector2 dotPos;

            if (distance > maxDistance)
            {
                dotPos = normalizedDir * mapRadius;
            }
            else
            {
                float scale = (distance / maxDistance) * mapRadius;
                dotPos = normalizedDir * scale;
            }

            enemyDots[i].localPosition = dotPos;
            enemyDots[i].gameObject.SetActive(true);
        }
    }
    public void Remove_Sub_Base(Vector3 pos)
    {
        for (int i = 0; i < SubBasePositions.Count; i++)
        {
            if (Vector3.Distance(SubBasePositions[i], pos) < 0.1f)
            {
                SubBasePositions.RemoveAt(i);
                break;
            }
        }
    }
    public void Remove_Main_Base(Vector3 pos)
    {
        for (int i = 0; i < MainBasePositions.Count; i++)
        {
            if (Vector3.Distance(MainBasePositions[i], pos) < 0.1f)
            {
                MainBasePositions.RemoveAt(i);
                break;
            }
        }
    }
}