using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections;

public class WhisperRunner : MonoBehaviour
{
    public float accuracy = 0f;
    public string resultText = "";
    public bool isReady = false;

    public RecognitionUI recognitionUI; // Inspector에서 연결 권장

    private void Start()
    {
        if (recognitionUI == null)
        {
            recognitionUI = FindAnyObjectByType<RecognitionUI>();
            if (recognitionUI == null)
                UnityEngine.Debug.LogWarning("⚠️ RecognitionUI 연결 안됨!");
        }
    }

    public void RunWhisper()
    {
        StartCoroutine(RunWhisperCoroutine());
    }

    private IEnumerator RunWhisperCoroutine()
    {
        string pythonExe = @"C:\Users\user\AppData\Local\Programs\Python\Python311\python.exe";
        string scriptPath = @"C:\Users\user\Documents\WhisperTest\check_accuracy.py";
        string wavPath = Path.Combine(Application.persistentDataPath, "recorded.wav");

        UnityEngine.Debug.Log($"📍 Python Script: {scriptPath}");
        UnityEngine.Debug.Log($"📍 WAV File: {wavPath}");

        StringBuilder outputBuilder = new StringBuilder();
        StringBuilder errorBuilder = new StringBuilder();

        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = pythonExe,
            Arguments = $"\"{scriptPath}\" \"{wavPath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8,
            CreateNoWindow = true
        };

        using (Process process = new Process())
        {
            process.StartInfo = psi;

            process.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    outputBuilder.AppendLine(e.Data);
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                    errorBuilder.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            while (!process.HasExited)
                yield return null;
        }

        string output = outputBuilder.ToString();
        string error = errorBuilder.ToString();

        UnityEngine.Debug.Log("📤 Python Output:\n" + output);
        if (!string.IsNullOrEmpty(error))
        {
            UnityEngine.Debug.LogWarning("🐍 Python stderr (무시 가능):\n" + error);
        }

        // 결과 파싱
        string recognized = "";
        float parsedAccuracy = 0f;
        string[] lines = output.Split('\n');
        foreach (string line in lines)
        {
            if (line.StartsWith("Recognized text"))
                recognized = line.Replace("Recognized text:", "").Trim();
            if (line.StartsWith("Accuracy"))
            {
                string[] parts = line.Split(':');
                if (parts.Length > 1)
                    float.TryParse(parts[1].Trim(), out parsedAccuracy);
            }
        }

        accuracy = parsedAccuracy;
        resultText = recognized;
        isReady = true;

        UnityEngine.Debug.Log($" Final Parsed Result: \"{recognized}\" with accuracy {accuracy}");

        if (recognitionUI != null)
        {
            bool isCorrect = accuracy >= 0.5f;
            UnityEngine.Debug.Log("🟩 Calling UpdateUI...");
            recognitionUI.UpdateUI(resultText, accuracy, isCorrect);
        }
        else
        {
            UnityEngine.Debug.LogWarning("⚠ UI 연결 안 되어 있어 업데이트 실패");
        }
    }
}
