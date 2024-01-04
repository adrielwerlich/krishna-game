using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public enum MantraIndex
    {
        WeaponMantra,
        HealingMantra
    }
    public enum BuyLineIndex
    {
        WeaponLine,
        HealingLine
    }

    [SerializeField] private static AudioClip[] mantras;
    [SerializeField] private static AudioClip[] buyLines;

    private static int enemyCount = 0;

    public static int EnemyCount { get => enemyCount; set => enemyCount = value; }

    public static float GetDistanceBetweenTwoObjects(Vector3 element1, Vector3 element2)
    {
        return Vector3.Distance(element1, element2);
    }

    public static IEnumerator Wait(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }

    public static AudioClip[] getMantras()
    {
        if (mantras == null)
        {
            mantras = new AudioClip[2];
            mantras[0] = Resources.Load<AudioClip>("Audio/OmNamoBhagavateVasudevaya");
            mantras[1] = Resources.Load<AudioClip>("Audio/HareKrishnaRama");
        }
        return mantras;
    }
    public static AudioClip[] getBuyLines()
    {
        if (buyLines == null)
        {
            buyLines = new AudioClip[2];
            buyLines[0] = Resources.Load<AudioClip>("Audio/BuyWeaponsLine");
            buyLines[1] = Resources.Load<AudioClip>("Audio/BuyHealingsLine");
        }
        return buyLines;
    }
    
    public static void DumpToConsole(object obj)
    {
        var output = JsonUtility.ToJson(obj, true);
        Debug.Log(output);
    }

    public static bool IsWebGL()
    {
        return Application.platform == RuntimePlatform.WebGLPlayer;
    }

    public static bool isMobile()
    {
        return Application.isMobilePlatform;
    }

    public static bool isEditor()
    {
        return Application.isEditor;
    }
}
