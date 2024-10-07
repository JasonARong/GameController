using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public SerialHandler serialHandler;
    public GameObject cube; // Cubeオブジェクトをインスペクターで設定

    private float xPos;
    private float yPos;
    private int sw;
    private int rs;

    private float logInterval = 1.0f; // ログを出力する間隔（秒）
    private float timeSinceLastLog = 0.0f;

    void Start()
    {
        // シグナルを受信したときの処理
        serialHandler.OnDataReceived += OnDataReceived;
    }

    void Update()
    {
        // Cubeの位置をXとYに基づいて更新
        cube.transform.position = new Vector3(cube.transform.position.x + xPos / 1000.0f, cube.transform.position.y, cube.transform.position.z + yPos / -1000.0f);

        // Cubeの回転をRSの値に基づいて更新
        if (rs == 1)
        {
            cube.transform.Rotate(Vector3.up, 10.0f); // 右回転
        }
        else if (rs == -1)
        {
            cube.transform.Rotate(Vector3.up, -10.0f); // 左回転
        }

        // 一定間隔で変数の値をログ出力
        timeSinceLastLog += Time.deltaTime;
        if (timeSinceLastLog >= logInterval)
        {
            Debug.Log($"Current values - X: {xPos}, Y: {yPos}, SW: {sw}, RS: {rs}");
            timeSinceLastLog = 0.0f;
        }
    }

    // 受信した信号(message)に対する処理
    void OnDataReceived(string message)
    {
        var data = message.Split(
            new string[] { " " }, System.StringSplitOptions.None);

        try
        {
            xPos = float.Parse(data[1]); // Xの値
            yPos = float.Parse(data[4]); // Yの値
            sw = int.Parse(data[7]);     // SWの値
            rs = int.Parse(data[10]);     // RSの値

            Debug.Log($"Received data  X: {xPos}, Y: {yPos}, SW: {sw}, RS: {rs}");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Error parsing data: " + e.Message);
        }
    }
}
