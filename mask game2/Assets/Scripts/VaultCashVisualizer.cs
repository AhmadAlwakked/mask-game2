using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
public class VaultCashVisualizer : MonoBehaviour
{
    [Header("Link the Vault")]
    public Vault vault;

    [Header("Simulation Settings")]
    public int simulationRuns = 10000;

    [Header("Results (Read-Only)")]
    public float chance1;
    public float chance2;
    public float chance3;
    public float chance4;
    public float chance5;

    void OnValidate()
    {
        if (vault != null)
            SimulateCashSpawns();
    }

    void SimulateCashSpawns()
    {
        int c1 = 0, c2 = 0, c3 = 0, c4 = 0, c5 = 0;

        for (int i = 0; i < simulationRuns; i++)
        {
            int spawned = SimulateSingleVault();

            switch (spawned)
            {
                case 1: c1++; break;
                case 2: c2++; break;
                case 3: c3++; break;
                case 4: c4++; break;
                case 5: c5++; break;
            }
        }

        chance1 = (float)c1 / simulationRuns * 100f;
        chance2 = (float)c2 / simulationRuns * 100f;
        chance3 = (float)c3 / simulationRuns * 100f;
        chance4 = (float)c4 / simulationRuns * 100f;
        chance5 = (float)c5 / simulationRuns * 100f;
    }

    int SimulateSingleVault()
    {
        // ✅ juiste veldnaam
        if (Random.value > vault.cashSpawnChance)
            return 0;

        int cash = 1;

        for (int i = 1; i < vault.maxCashCount; i++)
        {
            if (Random.value <= vault.extraCashChance)
                cash++;
            else
                break;
        }

        return cash;
    }
}

#if UNITY_EDITOR
public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}
#endif
