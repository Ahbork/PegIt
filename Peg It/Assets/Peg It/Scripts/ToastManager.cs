using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour {

    public Sprite[] toastSprites;
    public static ToastManager instance;
    public GameObject toastPrefab;

    //private List<Coroutine> toastCoroutines = new List<Coroutine>();
    Coroutine[] toastCoroutines;
    private float toastDuration = 3f;
    private float toastFadeTime = 1f;


    private void Awake()
    {
        instance = this;
        toastCoroutines = new Coroutine[toastSprites.Length];

    }

    public void CreateHint(Transform parent, Vector2 offset, int spriteIndex)
    {
        if(toastCoroutines[spriteIndex] != null) //Do not allow for multiple of the same toasts at once.
        {
            return;
        }
        Image newToast = GameObject.Instantiate(toastPrefab, parent).GetComponent<Image>();
        newToast.sprite = toastSprites[spriteIndex];

        newToast.rectTransform.localPosition = offset;
        newToast.rectTransform.sizeDelta = newToast.sprite.rect.size / 1.75f;

        toastCoroutines[spriteIndex] = StartCoroutine(ToastRoutine(newToast, spriteIndex));
    }

    private IEnumerator ToastRoutine(Image toast, int index)
    {
        yield return new WaitForSeconds(toastDuration);

        float startFade = Time.time;
        float endFade = startFade + toastFadeTime;
        float currentFadeTime = 0;

        while (Time.time < endFade)
        {
            currentFadeTime += Time.deltaTime;
            if (currentFadeTime > toastFadeTime)
            {
                currentFadeTime = toastFadeTime;
            }

            //lerp!
            float percent = (Time.time - startFade) / toastFadeTime;
            toast.color = Color.Lerp(Color.white, new Color(1,1,1,0), percent);
            yield return new WaitForEndOfFrame(); 
        }

        toastCoroutines[index] = null;
        Destroy(toast.gameObject);

    }
}
