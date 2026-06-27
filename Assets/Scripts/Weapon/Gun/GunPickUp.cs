using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    #region Component
    private Rigidbody2D rb;
    private Collider2D c;
    private SpriteRenderer sr;
    #endregion

    [SerializeField] private GunData gunData;
    [Header("DOTween")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private float height = .2f;

    private Tweener floatTween;

    private void Start()
    {
        floatTween = transform
            .DOMoveY(transform.position.y + height, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnValidate()
    {
        if (gunData is null)
        {
            return;
        }

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = gunData.sprite;
        gameObject.name = "Object - " + gunData.gunName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        if (gunData is null)
        {
            return;
        }

        GunController gunController = collision.GetComponent<GunController>();

        if (gunController is null)
        {
            return;
        }

        gunController.EquipGun(gunData);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        floatTween?.Kill();
    }
}
