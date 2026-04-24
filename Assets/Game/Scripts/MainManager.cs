using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    [Tooltip("��������, ���������� �� ������ ���� ������� � ����")]
    public GameObject eventManager;

    [Tooltip("��������, ���������� �� ������ ���� �������� � ����")]
    public GameObject effectManager;

    [Tooltip("��������, ���������� �� ������ ���� ��������� � �� �������������� � ����")]
    public GameObject mainItemManager;

    public GameObject waveManager;

    public GameObject mainPlayer;
    public GameObject UIManager;
    public Camera mainCamera;
    public SUIData mainUiData;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
