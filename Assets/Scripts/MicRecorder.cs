using UnityEngine;
using System.Collections;
using System.IO;

public class MicRecorder : MonoBehaviour
{
    public int recordTime = 3; // 녹음 시간 (초)
    private AudioClip recordedClip;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RecordMic());
        }
    }

    private IEnumerator RecordMic()
    {
        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("❌ 마이크 장치를 찾을 수 없습니다.");
            yield break;
        }

        string micName = Microphone.devices[0];
        Debug.Log("🎙️ 선택된 마이크: " + micName);
        Debug.Log("🎙️ 녹음 시작");

        recordedClip = Microphone.Start(micName, false, recordTime, 16000);
        yield return new WaitForSeconds(recordTime);

        Microphone.End(micName);
        Debug.Log("🎙️ 녹음 종료");

        // 🔻 여기에 약간의 대기 추가 (1프레임 or 0.1초)
        yield return null; // or yield return new WaitForSeconds(0.1f);

        if (recordedClip == null || recordedClip.samples == 0)
        {
            Debug.LogError("❌ 유효하지 않은 오디오 클립입니다.");
            yield break;
        }

        SavWav.Save("recorded.wav", recordedClip);
        string savePath = Path.Combine(Application.persistentDataPath, "recorded.wav");
        Debug.Log("💾 저장 완료: " + savePath);

        FindObjectOfType<WhisperRunner>()?.RunWhisper();
    }

}
