using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAttackActivate : MonoBehaviour
{
    [SerializeField] RawImage[] _images;

    private void Start()
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].gameObject.GetComponent<PlayerAttackCanvas>().GetActivator(this);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(ActivateImages(0));
    }

    public void DeactivateImages(float attackSpeed)
    {
        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].gameObject.SetActive(false);
        }

        StartCoroutine(ActivateImages(attackSpeed));
    }

    private IEnumerator ActivateImages(float attackSpeed)
    {
        yield return new WaitForSeconds(attackSpeed);

        for (int i = 0; i < _images.Length; i++)
        {
            _images[i].gameObject.SetActive(true);
        }
    }

//    private void OnDisable()
//    {
//        StopAllCoroutines();
//    }
}
