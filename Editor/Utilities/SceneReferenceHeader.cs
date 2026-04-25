using UnityEditor;
using UnityEngine;

namespace BHO.SceneReference.Editor
{
    public static class SceneReferenceHeader
    {
        public static void DrawHeader(string title, Rect rect)
        {
            EditorGUI.DrawRect(rect, new Color32(167, 239, 241, 255));
            
            GUIStyle style = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white },
                font = Resources.Load<Font>("JetBrainsMonoNL-Italic"),
                fontSize = 18
            };
            
            Texture2D logo = Resources.Load<Texture2D>("scene_reference_icon");
            float imageSize = 30;
            float spacing = 10;
            
            float textWidth = style.CalcSize(new GUIContent(title)).x;
            float totalWidth = imageSize + spacing + textWidth;
            
            float startX = rect.x + (rect.width - totalWidth) / 2;
            
            Rect imageRect = new Rect(startX, rect.y + (rect.height - imageSize) / 2, imageSize, imageSize);
            GUI.DrawTexture(imageRect, logo, ScaleMode.ScaleToFit);
            
            EditorGUI.LabelField(new Rect(startX + imageSize + spacing, rect.y + 10, textWidth, rect.height - 20), title, style);
        }
    }
}
