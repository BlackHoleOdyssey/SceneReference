using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace BHO.SceneReference.Editor
{
    public class BugReporterWindow : EditorWindow
    {
        private enum Labels
        {
            Bug,
            Feedback,
            FeatureRequest
        }

        private const string REPO_OWNER = "Black-Hole-Odyssey";
        private const string REPO_NAME = "SceneReference";

        private string reportTitle = "";
        private string reportBody = "";
        private Labels label = Labels.Bug;

        private string reportButtonLabel = "Send Report";
        private string reportMessage = "";
        
        private Color successColor = new(0.2f, 0.8f, 0.2f);
        private Color errorColor = new(0.8f, 0.2f, 0.2f);

        [MenuItem("BHO/Scene Reference/Report a Bug")]
        public static void ShowWindow()
        {
            BugReporterWindow window = GetWindow<BugReporterWindow>("Report a Bug");
            window.minSize = new Vector2(600, 400);
            window.ShowUtility();
        }

        public void OnGUI()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            
            SceneReferenceHeader.DrawHeader("Scene Reference Report", rect);
            
            GUILayout.Label("Report a Bug", EditorStyles.boldLabel);
            GUILayout.Space(10);
            GUILayout.Label(
                "If you encounter any issues or have suggestions for improving the Scene Reference package, please report them. Your feedback is invaluable in helping us enhance the package and provide a better experience for all users.",
                EditorStyles.wordWrappedLabel);
            GUILayout.Space(20);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUIUtility.labelWidth = 50;
                reportTitle = EditorGUILayout.TextField("Title", reportTitle, GUILayout.Width(400));

                GUILayout.FlexibleSpace();

                EditorGUIUtility.labelWidth = 60;
                label = (Labels)EditorGUILayout.EnumPopup(label, GUILayout.Width(150));

                EditorGUIUtility.labelWidth = 0;
            }

            GUILayout.Space(20);

            GUILayout.Label("Description");
            reportBody = EditorGUILayout.TextArea(reportBody, GUILayout.ExpandHeight(true));

            GUILayout.Space(20);

            if (GUILayout.Button(reportButtonLabel, GUILayout.Height(40)))
            {
                reportMessage = "";
                _ = SendReport();
            }

            GUILayout.Space(20);
            
            GUIStyle messageStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { textColor = reportMessage.Contains("successfully") ? successColor : errorColor },
                wordWrap = true,
                fontSize = 12
            };

            EditorGUILayout.LabelField(reportMessage, messageStyle);
        }

        private async Task SendReport()
        {
            if (string.IsNullOrWhiteSpace(reportTitle) || string.IsNullOrWhiteSpace(reportBody))
            {
                reportMessage = "Please fill in both the title and description before sending the report.";
                return;
            }

            reportButtonLabel = "Sending...";
            
            ReportData data = new ReportData
            {
                title = reportTitle,
                description = reportBody,
                label = label.ToString(),
                repoOwner = REPO_OWNER,
                repoName = REPO_NAME
            };
            
            string json = JsonUtility.ToJson(data);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest("https://unity-reporter.cedric-roux24.workers.dev", "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();

                request.SetRequestHeader("Content-Type", "application/json");

                UnityWebRequestAsyncOperation operation = request.SendWebRequest();

                while (!operation.isDone)
                    await Task.Yield();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    reportMessage = "Report sent successfully!";
                    reportTitle = "";
                    reportBody = "";
                }
                else
                {
                    reportMessage = $"Failed to send report: {request.error}";
                }

                reportButtonLabel = "Send Report";
            }
        }
    }

    [System.Serializable]
    public class ReportData
    {
        public string title;
        public string description;
        public string label;
        public string repoOwner;
        public string repoName;
    }
}