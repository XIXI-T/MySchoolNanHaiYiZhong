using UnityEngine;

public class CameraFilter : MonoBehaviour
{
    public Color filterColor = Color.white;
    public float intensity = 0.5f;
    
    private Material filterMaterial;
    
    void Start()
    {
        //创建一个简单的颜色叠加材质
        filterMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
    }
    
    void OnGUI()
    {
        if (Event.current.type == EventType.Repaint)
        {
            //在摄像机画面上覆盖一个半透明颜色层
            GUI.color = new Color(filterColor.r, filterColor.g, filterColor.b, intensity);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
        }
    }
}