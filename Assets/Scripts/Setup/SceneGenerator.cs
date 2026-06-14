using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// AUTO SETUP SCRIPT - Automatski kreira scene i postavlja sve
/// Samo dodaj ovo na GameObject u editoru i pokreni!
/// </summary>
public class SceneGenerator : MonoBehaviour
{
    public static void GenerateScene()
    {
        Debug.Log("🎮 Generating Cups and Balls Scene...");

        // Kreiraj praznu scenu
        Scene scene = SceneManager.CreateScene("CupsAndBalls");

        // 1. Kreiraj Lighting
        CreateLighting();

        // 2. Kreiraj XR Origin (kamera)
        CreateXROrigin();

        // 3. Kreiraj 5 čaša
        CreateCups();

        // 4. Kreiraj GameManager
        CreateGameManager();

        // 5. Kreiraj UI
        CreateUI();

        Debug.Log("✅ Scene generation complete!");
    }

    private static void CreateLighting()
    {
        GameObject light = new GameObject("Directional Light");
        Light lightComponent = light.AddComponent<Light>();
        lightComponent.type = LightType.Directional;
        lightComponent.intensity = 1;
        light.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    private static void CreateXROrigin()
    {
        GameObject xrOrigin = new GameObject("XR Origin");
        // Dodaj XROrigin komponentu iz XR Toolkit-a
        Debug.Log("⚠️ Trebalo bi da instaliraš XR Interaction Toolkit iz Package Manager-a!");
    }

    private static void CreateCups()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject cup = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cup.name = "Cup_" + i;
            cup.transform.position = new Vector3(i * 2f - 4f, 0, 0);
            
            // Dodaj Cup skriptu
            Cup cupScript = cup.AddComponent<Cup>();
            cupScript.SetIndex(i);
            
            Debug.Log($"✅ Created Cup {i}");
        }
    }

    private static void CreateGameManager()
    {
        GameObject gameManagerObj = new GameObject("GameManager");
        GameManager gm = gameManagerObj.AddComponent<GameManager>();
        gameManagerObj.AddComponent<CupManager>();
        gameManagerObj.AddComponent<AudioSource>();
        
        Debug.Log("✅ Created GameManager");
    }

    private static void CreateUI()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        
        canvasObj.AddComponent<UIManager>();
        canvasObj.AddComponent<AudioSource>();
        
        Debug.Log("✅ Created UI Canvas");
    }
}
