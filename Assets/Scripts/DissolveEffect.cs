using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public Material dissolveMaterial;
    private float dissolveSpeed = 0.25f;
    private float dissolveAmount = 0f;
    private bool isDissolving = false;
    private bool isAppearing = false;

    private float seconds = 0f;

    private void Start()
    {
        Reset();

        if (IsApparateScene(Teleport.scene))
        {
            dissolveAmount = 1f;
            Apparate();
        }
    }

    void Update()
    {
        seconds += Time.deltaTime;

        if (isDissolving)
        {
            dissolveAmount = Mathf.Min(1f, dissolveAmount + Time.deltaTime * dissolveSpeed);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            if (dissolveAmount >= 1f) isDissolving = false;
        }

        if (isAppearing)
        {
            dissolveAmount = Mathf.Max(0f, dissolveAmount - Time.deltaTime * dissolveSpeed);
            dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
            if (dissolveAmount <= 0f) isAppearing = false;
        }

        if (IsDisapparateScene(Teleport.scene))
        {
            if (seconds >= 3 && seconds < 7f)
            {
                Disapparate();
            }
            if (seconds >= 7f && Teleport.scene == 11)
            {
                Apparate();
            }
        }
    }

    public void Disapparate()
    {
        isDissolving = true;
        isAppearing = false;
    }

    public void Apparate()
    {
        isAppearing = true;
        isDissolving = false;
    }

    public void Reset()
    {
        seconds = 0f;
        isDissolving = false;
        isAppearing = false;
        dissolveAmount = 0.5f;
        gameObject.SetActive(true);
        dissolveMaterial.SetFloat("_DissolveAmount", dissolveAmount);
    }

    private bool IsApparateScene(int scene)
    {
        return scene == 2 || scene == 4 || scene == 6 || scene == 8 || scene == 10;
    }

    private bool IsDisapparateScene(int scene)
    {
        return scene == 3 || scene == 5 || scene == 7 || scene == 9 || scene == 11;
    }
}
