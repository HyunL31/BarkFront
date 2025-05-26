using UnityEngine;

public class LevenshteinDistance : MonoBehaviour
{
    public static int Compute(string a, string b)
    {
        a = a.ToLower();
        b = b.ToLower();

        int n = a.Length;
        int m = b.Length;
        int[,] d = new int[n + 1, m + 1];
        //초기화
        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for(int j=0; j<=m; j++) d[0, j] = j;
        //거리 계산
        for (int i = 1; i <= n; i++)
        {
            for (int j = 0; j <= m; j++)
            {
                int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                d[i, j] = Mathf.Min(
                    d[i - 1, j] + 1,
                    d[i, j - 1] + 1,
                    d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }

    public static float Similarity(string a, string b)
    {
        int dist= Compute(a.ToLower(), b.ToLower());
        return 1.0f - (float)dist / Mathf.Max(a.Length, b.Length); //1.0에 가까울수록 유사 
    }
}
