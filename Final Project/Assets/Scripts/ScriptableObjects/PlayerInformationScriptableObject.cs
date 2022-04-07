using UnityEngine;

//Written by Benedict
[CreateAssetMenu]
public class PlayerInformationScriptableObject : ScriptableObject
{
    [SerializeField] private float m_Health;

    public float GetHealth()
    {
        return m_Health;
    }


}
