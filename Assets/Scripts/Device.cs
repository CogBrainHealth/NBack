using UnityEngine;

public class Device : MonoBehaviour
{
    void Awake()
    {
        setRect();
    }

    public void setRect()
    {
        // ���� GameObject�� ������ Camera ������Ʈ�� �������� �ڵ�
        Camera cam = GetComponent<Camera>();

        // ���� ī�޶��� ����Ʈ ������ �������� �ڵ�
        Rect viewportRect = cam.rect;

        float deviceWidth = (float)Screen.width;
        float deviceHeight = (float)Screen.height; 
        float targetWidth = 1080f;
        float targetHeight = 2340f;

        //Screen.SetResolution(targetWidth, (int)(((float)deviceHeight / deviceWidth) * targetWidth), FullScreenMode.Windowed);

        if (targetWidth / targetHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)targetWidth / targetHeight) / ((float)deviceWidth / deviceHeight);
            //Screen.SetResolution((int)newWidth, (int)deviceHeight, FullScreenMode.Windowed);
            cam.rect = new Rect((1f - newWidth) / 2f, 0, newWidth, 1f);
            Debug.Log("�ʺ� ����");
        }
        else
        {
            float newHeight = (deviceWidth / deviceHeight) / (targetWidth / targetHeight);
            //Screen.SetResolution((int)deviceWidth, (int)newHeight, FullScreenMode.Windowed);
            cam.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);
            Debug.Log("���� ����");
        }
    }

    private void OnPreCull()
    {
        GL.Clear(true, true, new Color(163/256f, 202/256f, 132/256f,1f));
    }
}
